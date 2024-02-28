using ContractionHierarchies.CHAlgorithm;
using ContractionHierarchies.GraphImpl;
using ContractionHierarchies.Io;

namespace ContractionHierarchies
{
    public class MainClass
    {

        public static void Main(string[] args)
        {
            StreetGraph graph = GraphLoader.Load("germany_sw_50kv.graph");
            ICHPreProcessor preProcessor = new SingleThreadCHPreProcessor();
            preProcessor.PreProcess(new RandomContractionOrder(1), new StandardContractor(), graph);
        }

    }
}
