using ContractionHierarchies.GraphImpl;
using ContractionHierarchies.Io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractionHierarchies.CHAlgorithm
{
    class SingleThreadCHPreProcessor : ICHPreProcessor
    {



        public ContractionOrder PreProcess(IContractionOrder contractionOrder, IContractor contractor, StreetGraph graph)
        {
            bool[] contracted = new bool[graph.VertexCount];
            ConsoleProgressBar progressBar = new();
            for (int i = 0; i < graph.VertexCount; i++)
            {
                int contractionId = contractionOrder.NextVertex(graph, contracted);
                contractor.Contract(graph, contractionId, contracted);
                contracted[contractionId] = true;
                progressBar.ReportProgress((i + 1f) / graph.VertexCount);
            }
            return contractionOrder.ContractionOrder;
        }
    }
}
