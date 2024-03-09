using ContractionHierarchies.GraphImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractionHierarchies
{
    interface ILocalRoutingAlgorithm
    {

        void ComputeShortestPaths(int startId, HashSet<int> targetIds);

        List<int> GetNodesOnShortestPathTo(int targetId);

        float GetCostTo(int targetId);

    }
}
