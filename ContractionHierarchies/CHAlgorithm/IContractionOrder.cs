using ContractionHierarchies.GraphImpl;

namespace ContractionHierarchies.CHAlgorithm
{
    interface IContractionOrder
    {
        int NextVertex(StreetGraph g, bool[] contracted);

        ContractionOrder ContractionOrder { get; }
    }
}
