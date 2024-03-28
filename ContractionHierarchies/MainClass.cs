using ContractionHierarchies.CHAlgorithm;
using ContractionHierarchies.GraphImpl;
using ContractionHierarchies.Io;

namespace ContractionHierarchies
{
    public class MainClass
    {

        public static void Main(string[] args)
        {
            StreetGraph graph = GraphLoader.Load("germany_sw_500kv.graph");
            ICHPreProcessor preProcessor = new MultiThreadCHPreProcessor();
            preProcessor.PreProcess(new RandomContractionOrder(graph, 1), new ThreadSafeContractor(graph), graph);
        }

    }
}
