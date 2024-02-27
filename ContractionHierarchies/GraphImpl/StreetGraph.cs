global using Vertex = System.Numerics.Vector2;
using System.Diagnostics;


namespace ContractionHierarchies.GraphImpl
{


    /// <summary>
    /// This struct describes an edge. The source vertex is not defined in this class so intended use is for example: <c>graph.IncidentEdges(vertex)[0].Cost</c>
    /// </summary>
    /// <param name="targetVertexId"> The Id of the vertex the edge points to. </param>
    /// <param name="cost"> The edge cost. </param>
    public struct EdgeTo(int targetVertexId, float cost)
    {
        /// <summary>
        /// The Id of the vertex the edge points to.
        /// </summary>
        public int TargetId { get; set; } = targetVertexId;

        /// <summary>
        /// The edge cost.
        /// </summary>
        public float Cost { get; set; } = cost;

    }

    /// <summary>
    /// An implementation of a directed weighted graph with an given embedding. Its intended use is as a street graph or other sparse graphs.
    /// </summary>
    public class StreetGraph
    {
        /// <summary>
        /// The vertex positions.
        /// </summary>
        public Vertex[] Vertices { get; }

        /// <summary>
        /// For each vertex the outgoing edges.
        /// </summary>
        public List<EdgeTo>[] Edges { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vertices"> The vertex positions. </param>
        /// <param name="edges"> For each vertex the outgoing edges. </param>
        public StreetGraph(Vertex[] vertices, List<EdgeTo>[] edges)
        {
            Debug.Assert(vertices != null);
            Debug.Assert(edges != null);
            Debug.Assert(vertices.Length == edges.Length);
            Vertices = vertices;
            Edges = edges;
        }

        /// <summary>
        /// The number of vertices in the graph.
        /// </summary>
        public int VertexCount
        {
            get => Vertices.Length;
        }

        /// <summary>
        /// Returns the list edges originating from the passed vertex id.
        /// </summary>
        /// <param name="vertexIndex"> The id of the vertex to get the outgoing edges from. </param>
        /// <returns> The list edges originating from the passed vertex id. </returns>
        public List<EdgeTo> EdgesFrom(int vertexIndex)
        {
            return Edges[vertexIndex];
        }

        /// <summary>
        /// Returns the number of distinct vertices reachable from the given vertex with a path length of one. This means edges are NOT traversed the wrong way.
        /// </summary>
        /// <param name="vertexIndex"> The id of the vertex to get the number of neighbours from. </param>
        /// <returns> The number of distinct vertices reachable from the given vertex with a path length of one. </returns>
        public int NeighbourCount(int vertexIndex)
        {
            return Edges[vertexIndex]
                .Select(v => v.TargetId)
                .Distinct()
                .Count();
        }

        /// <summary>
        /// Computes the number of edges in the graph.
        /// </summary>
        /// <returns> The number of edges in the graph. </returns>
        public int ComputeEdgeCount()
        {
            return Edges.Sum(edges => edges.Count);
        }

    }
}
