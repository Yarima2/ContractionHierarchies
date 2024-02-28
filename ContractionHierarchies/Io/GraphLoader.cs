using ContractionHierarchies.GraphImpl;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;

namespace ContractionHierarchies.Io
{

    /// <summary>
    /// Static class to load graphs from text files. The files must be of the following format:
    /// 
    /// numberOfVertices
    /// numberOfEdges
    /// v1Latitude v1Longitude
    /// v2Latitude v2Longitude
    /// v3Latitude v3Longitude
    /// ...
    /// edge1VertIdFrom edge1VertIdTo cost
    /// edge2VertIdFrom edge2VertIdTo cost
    /// ...
    /// </summary>
    static class GraphLoader
    {

        private static readonly CultureInfo Culture = new("de-DE");

        private const string DECIMAL_SEPARATOR = ".";

        private const string TOKEN_SEPARATOR = " ";

        static GraphLoader() => Culture.NumberFormat.CurrencyDecimalSeparator = DECIMAL_SEPARATOR;

        /// <summary>
        /// Loads the passed file and parses a StreetGraph from it. The file has to be of the format described in the class comment.
        /// </summary>
        /// <param name="path"> The path of the graph file </param>
        /// <returns> The parsed StreetGraph </returns>
        public static StreetGraph Load(string path)
        {
            Debug.Assert(path != null);
            Debug.Assert(File.Exists(path));
            Console.WriteLine("Reading graph file");
            string[] lines = File.ReadAllLines(path);
            int vertexCount = int.Parse(lines[0]);
            int edgeCount = int.Parse(lines[1]);
            var vertices = new Vertex[vertexCount];
            var edges = new List<EdgeTo>[vertexCount];
            Console.WriteLine("Creating edge lists");
            Parallel.For(0, vertexCount, i =>
            {
                edges[i] = [];
            });
            Console.WriteLine("Parsing vertices");
            Parallel.For(0, vertexCount, i =>
            {
                string[] splitLine = lines[i + 2].Split(TOKEN_SEPARATOR);
                float x = float.Parse(splitLine[0], NumberStyles.Any, Culture);
                float y = float.Parse(splitLine[1], NumberStyles.Any, Culture);
                vertices[i] = new Vertex(x, y);
            });
            Console.WriteLine("Parsing edges ");
            Parallel.For(0, edgeCount, i =>
            {
                string[] splitLine = lines[i + vertexCount + 2].Split(TOKEN_SEPARATOR);
                int srcIndex = int.Parse(splitLine[0]);
                int destIndex = int.Parse(splitLine[1]);
                float cost = float.Parse(splitLine[2], NumberStyles.Any, Culture);
                lock (edges[srcIndex])
                {
                    edges[srcIndex].Add(new EdgeTo(destIndex, cost));
                }
                lock (edges[destIndex])
                {
                    edges[destIndex].Add(new EdgeTo(srcIndex, cost));
                }
            });
            Console.WriteLine($"Loaded graph with {vertexCount} vertices and {edgeCount} edges");
            return new StreetGraph(vertices, edges);
        }

    }
}
