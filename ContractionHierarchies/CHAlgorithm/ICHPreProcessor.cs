global using ContractionOrder = int[];
using ContractionHierarchies.GraphImpl;

namespace ContractionHierarchies.CHAlgorithm
{
    interface ICHPreProcessor
    {
        ContractionOrder PreProcess(IContractionOrder contractionOrder, IContractor contractor, StreetGraph graph);
    }
}