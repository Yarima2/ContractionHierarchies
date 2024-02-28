using ContractionHierarchies.GraphImpl;

namespace ContractionHierarchies.CHAlgorithm
{
    interface IContractionOrder
    {
        int NextVertex(StreetGraph g, ISet<int> unContracted);
    }
}
