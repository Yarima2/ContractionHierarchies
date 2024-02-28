using ContractionHierarchies.GraphImpl;

namespace ContractionHierarchies.CHAlgorithm
{
    interface IContractor
    {
        void Contract(StreetGraph graph, int contractionId, HashSet<int> unContracted);
    }
}