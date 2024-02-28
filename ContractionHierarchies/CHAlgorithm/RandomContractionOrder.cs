using ContractionHierarchies.GraphImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractionHierarchies.CHAlgorithm
{
    class RandomContractionOrder : IContractionOrder
    {

        public Random rand;

        public RandomContractionOrder(int? seed)
        {
            if(seed != null){
                rand = new Random(seed.Value);
            }
            else
            {
                rand = Random.Shared;
            }
        }

        public int NextVertex(StreetGraph g, ISet<int> unContracted)
        {
            int index = rand.Next(unContracted.Count);
            return unContracted.ElementAt(index);
        }
    }
}
