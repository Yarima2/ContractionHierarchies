using ContractionHierarchies.GraphImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractionHierarchies.CHAlgorithm
{
    class SingleThreadCHPreProcessor : ICHPreProcessor
    {

        public bool IsPreProcessingDone { get; private set; } = false;


        public void PreProcess(IContractionOrder contractionOrder, IContractor contractor, StreetGraph graph)
        {
            if (IsPreProcessingDone) return;
            HashSet<int> unContracted = Enumerable.Range(0, graph.VertexCount).ToHashSet();
            while(unContracted.Count > 0)
            {
                int contractionId = contractionOrder.NextVertex(graph, unContracted);
                contractor.Contract(graph, contractionId, unContracted);
                unContracted.Remove(contractionId);
            }
            IsPreProcessingDone = true;
        }
    }
}
