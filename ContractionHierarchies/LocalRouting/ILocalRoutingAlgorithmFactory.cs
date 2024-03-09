using ContractionHierarchies.GraphImpl;

namespace ContractionHierarchies.LocalRouting
{
    interface ILocalRoutingAlgorithmFactory
    {
        ILocalRoutingAlgorithm Create(StreetGraph graph, bool[] blocked);
    }
}
