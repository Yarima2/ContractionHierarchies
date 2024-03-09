using ContractionHierarchies.CHAlgorithm;
using ContractionHierarchies.GraphImpl;
using ContractionHierarchies.LocalRouting;
using System.Diagnostics.Contracts;

namespace ContractionHierarchies
{
    class StandardContractor(bool threadSafe = false) : IContractor
    {

        public bool ThreadSafe { get; } = threadSafe;

        private readonly ILocalRoutingAlgorithmFactory _routingAlgorithmFactory = new DijkstraLocalRoutingAlgorithmFactory();




        public void Contract(StreetGraph graph, int contractionId, bool[] contracted)
        {
            if (ThreadSafe)
            {
                int[] neighbours = graph.EdgesFrom(contractionId).Select(edgeTo => edgeTo.TargetId).ToArray();
                Monitor.Enter(graph.Vertices[contractionId]);
                Array.Sort(neighbours);
                foreach (var neighbour in neighbours) {
                    Monitor.Enter(graph.Vertices[neighbour]);
                }
                ContractVertex(graph, contractionId, contracted);
                Monitor.Exit(graph.Vertices[contractionId]);
                foreach (var neighbour in neighbours)
                {
                    Monitor.Exit(graph.Vertices[neighbour]);
                }
            }
            else
            {
                ContractVertex(graph, contractionId, contracted);
            }
        }

        private void ContractVertex(StreetGraph graph, int contractionId, bool[] contracted)
        {
            int[] neighbours = graph.EdgesFrom(contractionId).Select(edgeTo => edgeTo.TargetId)
                                                            .Where(neighbour => !contracted[neighbour])
                                                            .ToArray();
            foreach(int start in neighbours)
            {
                ILocalRoutingAlgorithm routingAlgorithm = _routingAlgorithmFactory.Create(graph, contracted);
                routingAlgorithm.ComputeShortestPaths(start, neighbours.ToHashSet());
                foreach (int target in neighbours)
                {
                    List<int> path = routingAlgorithm.GetNodesOnShortestPathTo(target);
                    if (path.Contains(contractionId))
                    {
                        float cost = routingAlgorithm.GetCostTo(target);
                        InsertShortcut(graph, start, target, cost);
                    }
                }
            }
        }

        private void InsertShortcut(StreetGraph graph, int start, int target, float cost)
        {
            if (graph.Edges[start] == null)
            {
                graph.Edges[start] = [new EdgeTo(target, cost)];
            }
            else
            {
                graph.Edges[start].Add(new EdgeTo(target, cost));
            }
        }
    }
}