using ContractionHierarchies.GraphImpl;

namespace ContractionHierarchies.CHAlgorithm
{
    interface ICHPreProcessor
    {
        void PreProcess(IContractionOrder contractionOrder, IContractor contractor, StreetGraph graph);
    }
}