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
            bool[] contracted = new bool[graph.VertexCount];
            for(int i = 0; i < graph.VertexCount; i++)
            {
                int contractionId = contractionOrder.NextVertex(graph, contracted);
                contractor.Contract(graph, contractionId, contracted);
                contracted[contractionId] = true;
            }
            IsPreProcessingDone = true;
        }
    }
}
