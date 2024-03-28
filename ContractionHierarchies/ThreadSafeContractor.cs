using ContractionHierarchies.CHAlgorithm;
using ContractionHierarchies.GraphImpl;
using ContractionHierarchies.LocalRouting;
using System.Diagnostics.Contracts;

namespace ContractionHierarchies
{
    class ThreadSafeContractor : IContractor
    {


        private readonly ReaderWriterLock[] locks = [];

        private readonly ILocalRoutingAlgorithmFactory _routingAlgorithmFactory = new DijkstraLocalRoutingAlgorithmFactory();

        public ThreadSafeContractor(StreetGraph graph)
        {
            locks = new ReaderWriterLock[graph.VertexCount];
            for (int i = 0; i < locks.Length; i++)
            {
                locks[i] = new ReaderWriterLock();
            }
        }


        public void Contract(StreetGraph graph, int contractionId, bool[] contracted)
        {

            locks?[contractionId].AcquireReaderLock(MultiThreadCHPreProcessor.LockTimeout);
            int[] lockedIds = graph.EdgesFrom(contractionId)
                                    .Select(edgeTo => edgeTo.TargetId)
                                    .Append(contractionId)
                                    .ToArray();
            locks?[contractionId].ReleaseReaderLock();
            Array.Sort(lockedIds);
            foreach (var lockedId in lockedIds) {
                locks?[lockedId].AcquireWriterLock(MultiThreadCHPreProcessor.LockTimeout);
            }
            ContractVertex(graph, contractionId, contracted);
            foreach (var lockedId in lockedIds)
            {
                locks?[lockedId].ReleaseWriterLock();
            }
        }

        private void ContractVertex(StreetGraph graph, int contractionId, bool[] contracted)
        {
            int[] neighbours = graph.EdgesFrom(contractionId).Select(edgeTo => edgeTo.TargetId)
                                                            .Where(neighbour => !contracted[neighbour])
                                                            .Distinct()
                                                            .ToArray();
            List<FullEdge> insertedShortcuts = [];
            foreach(int start in neighbours)
            {
                if (contracted[start])
                {
                    continue;
                }
                ILocalRoutingAlgorithm routingAlgorithm = _routingAlgorithmFactory.Create(graph, contracted, locks);
                routingAlgorithm.ComputeShortestPaths(start, neighbours.ToHashSet());
                foreach (int target in neighbours)
                {
                    if (contracted[target])
                    {
                        continue;
                    }
                    List<int> path = routingAlgorithm.GetNodesOnShortestPathTo(target);
                    if (path.Contains(contractionId))
                    {
                        float cost = routingAlgorithm.GetCostTo(target);
                        insertedShortcuts.Add(new FullEdge(start, target, cost));
                    }
                }
            }
            foreach (FullEdge edge in insertedShortcuts)
            {
                InsertShortcut(graph, edge);
            }
        }

        private void InsertShortcut(StreetGraph graph, FullEdge edge)
        {
            if (graph.Edges[edge.StartId] == null)
            {
                graph.Edges[edge.StartId] = [];
            }
            graph.Edges[edge.StartId].Add(new EdgeTo(edge.TargetId, edge.Cost));
        }
    }
}