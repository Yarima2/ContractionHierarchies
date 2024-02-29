using ContractionHierarchies.GraphImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractionHierarchies.CHAlgorithm
{

    /// <summary>
    /// This is a basic contraction order for testing until the edge difference contraction order is implemented.
    /// </summary>
    class RandomContractionOrder : IContractionOrder
    {

        private int[] contractionOrder;

        private int numContracted = 0;

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
            contractionOrder = Enumerable.Range(0, graph.VertexCount).ToArray();
            rand.Shuffle(contractionOrder);
        }

        public int NextVertex(StreetGraph g, bool[] contracted)
        {
            return contractionOrder[numContracted++];
        }
    }
}
