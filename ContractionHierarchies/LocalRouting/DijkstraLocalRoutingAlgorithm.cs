using ContractionHierarchies.CHAlgorithm;
using ContractionHierarchies.GraphImpl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractionHierarchies.LocalRouting
{
    class DijkstraLocalRoutingAlgorithm : ILocalRoutingAlgorithm
    {

        private readonly StreetGraph _graph;
        private readonly bool[] _blocked;
        private readonly ReaderWriterLock[]? _locks;

        private int _startId;
        private readonly HashSet<int> _visited = [];
        private readonly Dictionary<int, float> _distances = [];
        private readonly Dictionary<int, int> _predecessors = [];

        public DijkstraLocalRoutingAlgorithm(StreetGraph graph, bool[] blocked, ReaderWriterLock[]? locks = null)
        {
            ArgumentNullException.ThrowIfNull(graph);
            ArgumentNullException.ThrowIfNull(blocked);
            _graph = graph;
            _blocked = blocked;
            _locks = locks;
        }


        public void ComputeShortestPaths(int startId, HashSet<int> targetIds)
        {
            _startId = startId;
            
            PriorityQueue<int, float> queue = new();

            queue.Enqueue(startId, 0);
            _distances[startId] = 0;
            while (queue.Count > 0)
            {
                int vertIndex = queue.Dequeue();
                if (!_visited.Contains(vertIndex) && !_blocked[vertIndex])
                {
                    _visited.Add(vertIndex);
                    targetIds.Remove(vertIndex);
                    if (targetIds.Count == 0)
                    {
                        return;
                    }
                    if (_locks != null && !_locks[vertIndex].IsWriterLockHeld)
                    {
                        _locks?[vertIndex].AcquireReaderLock(MultiThreadCHPreProcessor.LockTimeout);
                    }
                    foreach (EdgeTo neighbour in _graph.EdgesFrom(vertIndex))
                    {
                        if (!_visited.Contains(neighbour.TargetId))
                        {
                            DistanceUpdate(vertIndex, neighbour.TargetId, neighbour.Cost, queue);
                        }
                    }
                    if (_locks != null && _locks[vertIndex].IsReaderLockHeld)
                    {
                        _locks?[vertIndex].ReleaseReaderLock();
                    }
                }
                if(_visited.Count > 500)
                {
                   throw new Exception("Dijkstra too large");
                }
            }
            if(targetIds.Count > 0)
            {
                throw new ArgumentException("Some targets were not reachable from the startId");
            }
        }

        private void DistanceUpdate(int predecessor, int vertex, float distance, PriorityQueue<int, float> queue)
        {
            float alternative = _distances[predecessor] + distance;
            if (!_distances.TryGetValue(vertex, out float origDistance) || alternative < origDistance)
            {
                queue.Enqueue(vertex, alternative);
                _distances[vertex] = alternative;
                _predecessors[vertex] = predecessor;
            }
        }



        public List<int> GetNodesOnShortestPathTo(int targetId)
        {
            if (!_visited.Contains(targetId))
            {
                throw new ArgumentException("The passed vertex was not visited by the dijkstra run");
            }
            var vertices = new List<int>();

            int vertex = targetId;
            while (vertex != _startId)
            {
                vertices.Insert(0, vertex);
                vertex = _predecessors[vertex];
            }
            vertices.Insert(0, vertex);
            return vertices;
        }

        public float GetCostTo(int targetId)
        {
            if (!_visited.Contains(targetId))
            {
                throw new ArgumentException("The passed vertex was not visited by the dijkstra run");
            }
            return _distances[targetId];
        }
    }

    public class DijkstraLocalRoutingAlgorithmFactory : ILocalRoutingAlgorithmFactory
    {
        ILocalRoutingAlgorithm ILocalRoutingAlgorithmFactory.Create(StreetGraph graph, bool[] blocked, ReaderWriterLock[]? locks)
        {
            return new DijkstraLocalRoutingAlgorithm(graph, blocked, locks);
        }
    }
}
