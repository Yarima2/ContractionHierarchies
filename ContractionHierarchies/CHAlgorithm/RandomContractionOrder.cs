using ContractionHierarchies.GraphImpl;

namespace ContractionHierarchies.CHAlgorithm
{

    /// <summary>
    /// This is a basic contraction order for testing until the edge difference contraction order is implemented.
    /// </summary>
    class RandomContractionOrder : IContractionOrder
    {

        private int numContracted = 0;

        public ContractionOrder ContractionOrder { get; private set; }

        public RandomContractionOrder(StreetGraph graph, int? seed = null)
        {
            Random rand;
            if(seed != null){
                rand = new Random(seed.Value);
            }
            else
            {
                rand = Random.Shared;
            }
            ContractionOrder = Enumerable.Range(0, graph.VertexCount).ToArray();
            rand.Shuffle(ContractionOrder);
        }

        public int NextVertex(StreetGraph g, bool[] contracted)
        {
            return ContractionOrder[numContracted++];
        }
    }
}
