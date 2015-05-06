using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class GraphGenerator
    {
        private class Edge : IComparable<Edge>
        {
            private readonly int _v;
            private readonly int _w;

            public Edge(int v, int w)
            {
                if (v < w)
                {
                    _v = v;
                    _w = w;
                }
                else
                {
                    _v = w;
                    _w = v;
                }
            }

            public int CompareTo(Edge that)
            {
                if (_v < that._v)
                {
                    return -1;
                }
                if (_v > that._v)
                {
                    return +1;
                }
                if (_w < that._w)
                {
                    return -1;
                }
                if (_w > that._w)
                {
                    return +1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Returns a random simple graph containing V vertices and E edges.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="e">the number of edges</param>
        /// <returns>a random simple graph on V vertices, containing a total of E edges</returns>
        public static Graph Simple(int v, int e)
        {
            if (e > (long)v * (v - 1) / 2)
            {
                throw new ArgumentException("Too many edges");
            }
            if (e < 0)
            {
                throw new ArgumentException("Too few edges");
            }
            Graph g = new Graph(v);
            Set<Edge> set = new Set<Edge>();
            while (g.E() < e)
            {
                int vCount = StdRandom.Uniform(v);
                int wCount = StdRandom.Uniform(v);
                Edge edge = new Edge(vCount, wCount);
                if ((vCount != wCount) && !set.Contains(edge))
                {
                    set.Add(edge);
                    g.AddEdge(vCount, wCount);
                }
            }
            return g;
        }

        /// <summary>
        /// Returns a random simple graph on V vertices, with an 
        /// edge between any two vertices with probability p. This is sometimes
        /// referred to as the Erdos-Renyi random graph model.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="p">the probability of choosing an edge</param>
        /// <returns>a random simple graph on V vertices, with an edge between any two vertices with probability p</returns>
        public static Graph Simple(int v, double p)
        {
            if (p < 0.0 || p > 1.0)
            {
                throw new ArgumentException("Probability must be between 0 and 1");
            }
            Graph g = new Graph(v);
            for (int vertex = 0; vertex < v; vertex++)
            {
                for (int w = vertex + 1; w < v; w++)
                {
                    if (StdRandom.Bernoulli(p))
                    {
                        g.AddEdge(vertex, w);
                    }
                }
            }
            return g;
        }

        /// <summary>
        /// Returns the complete graph on V vertices.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <returns>the complete graph on V vertices</returns>
        public static Graph Complete(int v)
        {
            return Simple(v, 1.0);
        }

        /// <summary>
        /// Returns a complete bipartite graph on V1 and V2 vertices.
        /// </summary>
        /// <param name="v1">the number of vertices in one partition</param>
        /// <param name="v2">the number of vertices in the other partition</param>
        /// <returns>a complete bipartite graph on V1 and V2 vertices</returns>
        public static Graph CompleteBipartite(int v1, int v2)
        {
            return Bipartite(v1, v2, v1 * v2);
        }

        /// <summary>
        /// Returns a random simple bipartite graph on V1 and V2 vertices
        /// with E edges.
        /// </summary>
        /// <param name="v1">the number of vertices in one partition</param>
        /// <param name="v2">the number of vertices in the other partition</param>
        /// <param name="e">the number of edges</param>
        /// <returns>a random simple bipartite graph on V1 and V2 vertices, containing a total of E edges</returns>
        public static Graph Bipartite(int v1, int v2, int e)
        {
            if (e > (long)v1 * v2)
            {
                throw new ArgumentException("Too many edges");
            }
            if (e < 0)
            {
                throw new ArgumentException("Too few edges");
            }
            Graph g = new Graph(v1 + v2);

            int[] vertices = new int[v1 + v2];
            for (int i = 0; i < v1 + v2; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);

            Set<Edge> set = new Set<Edge>();
            while (g.E() < e)
            {
                int i = StdRandom.Uniform(v1);
                int j = v1 + StdRandom.Uniform(v2);
                Edge edge = new Edge(vertices[i], vertices[j]);
                if (!set.Contains(edge))
                {
                    set.Add(edge);
                    g.AddEdge(vertices[i], vertices[j]);
                }
            }
            return g;
        }

        /// <summary>
        /// Returns a random simple bipartite graph on V1 and V2 vertices,
        /// containing each possible edge with probability p.
        /// </summary>
        /// <param name="v1">the number of vertices in one partition</param>
        /// <param name="v2">the number of vertices in the other partition</param>
        /// <param name="p">the probability that the graph contains an edge with one endpoint in either side</param>
        /// <returns>a random simple bipartite graph on V1 and V2 vertices, containing each possible edge with probability p</returns>
        public static Graph Bipartite(int v1, int v2, double p)
        {
            if (p < 0.0 || p > 1.0)
            {
                throw new ArgumentException("Probability must be between 0 and 1");
            }
            int[] vertices = new int[v1 + v2];
            for (int i = 0; i < v1 + v2; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);
            Graph g = new Graph(v1 + v2);
            for (int i = 0; i < v1; i++)
            {
                for (int j = 0; j < v2; j++)
                {
                    if (StdRandom.Bernoulli(p))
                    {
                        g.AddEdge(vertices[i], vertices[v1 + j]);
                    }
                }
            }
            return g;
        }

        /// <summary>
        /// Returns a path graph on V vertices.
        /// </summary>
        /// <param name="v">the number of vertices in the path</param>
        /// <returns>a path graph on V vertices</returns>
        public static Graph Path(int v)
        {
            Graph g = new Graph(v);
            int[] vertices = new int[v];
            for (int i = 0; i < v; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);
            for (int i = 0; i < v - 1; i++)
            {
                g.AddEdge(vertices[i], vertices[i + 1]);
            }
            return g;
        }

        /// <summary>
        /// Returns a complete binary tree graph on V vertices.
        /// </summary>
        /// <param name="v">the number of vertices in the binary tree</param>
        /// <returns>a complete binary tree graph on V vertices</returns>
        public static Graph BinaryTree(int v)
        {
            Graph g = new Graph(v);
            int[] vertices = new int[v];
            for (int i = 0; i < v; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);
            for (int i = 1; i < v; i++)
            {
                g.AddEdge(vertices[i], vertices[(i - 1) / 2]);
            }
            return g;
        }

        /// <summary>
        /// Returns a cycle graph on V vertices.
        /// </summary>
        /// <param name="v">the number of vertices in the cycle</param>
        /// <returns>a cycle graph on V vertices</returns>
        public static Graph Cycle(int v)
        {
            Graph g = new Graph(v);
            int[] vertices = new int[v];
            for (int i = 0; i < v; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);
            for (int i = 0; i < v - 1; i++)
            {
                g.AddEdge(vertices[i], vertices[i + 1]);
            }
            g.AddEdge(vertices[v - 1], vertices[0]);
            return g;
        }

        /// <summary>
        /// Returns a wheel graph on V vertices.
        /// </summary>
        /// <param name="v">the number of vertices in the wheel</param>
        /// <returns>a wheel graph on V vertices: a single vertex connected to every vertex in a cycle on V-1 vertices</returns>
        public static Graph Wheel(int v)
        {
            if (v <= 1)
            {
                throw new ArgumentException("Number of vertices must be at least 2");
            }
            Graph g = new Graph(v);
            int[] vertices = new int[v];
            for (int i = 0; i < v; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);

            // simple cycle on V-1 vertices
            for (int i = 1; i < v - 1; i++)
            {
                g.AddEdge(vertices[i], vertices[i + 1]);
            }
            g.AddEdge(vertices[v - 1], vertices[1]);

            // connect vertices[0] to every vertex on cycle
            for (int i = 1; i < v; i++)
            {
                g.AddEdge(vertices[0], vertices[i]);
            }

            return g;
        }

        /// <summary>
        /// Returns a star graph on V vertices.
        /// </summary>
        /// <param name="v">the number of vertices in the star</param>
        /// <returns>a star graph on V vertices: a single vertex connected to every other vertex</returns>
        public static Graph Star(int v)
        {
            if (v <= 0)
            {
                throw new ArgumentException("Number of vertices must be at least 1");
            }
            Graph g = new Graph(v);
            int[] vertices = new int[v];
            for (int i = 0; i < v; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);

            // connect vertices[0] to every other vertex
            for (int i = 1; i < v; i++)
            {
                g.AddEdge(vertices[0], vertices[i]);
            }

            return g;
        }

        /// <summary>
        /// Returns a uniformly random k-regular graph on V vertices
        /// (not necessarily simple). The graph is simple with probability only about e^(-k^2/4),
        /// which is tiny when k = 14.
        /// </summary>
        /// <param name="v">the number of vertices in the graph</param>
        /// <param name="k"></param>
        /// <returns>a uniformly random k-regular graph on V vertices.</returns>
        public static Graph Regular(int v, int k)
        {
            if (v * k % 2 != 0)
            {
                throw new ArgumentException("Number of vertices * k must be even");
            }
            Graph g = new Graph(v);

            // create k copies of each vertex
            int[] vertices = new int[v * k];
            for (int vertex = 0; vertex < v; vertex++)
            {
                for (int j = 0; j < k; j++)
                {
                    vertices[vertex + v * j] = vertex;
                }
            }

            // pick a random perfect matching
            StdRandom.Shuffle(vertices);
            for (int i = 0; i < v * k / 2; i++)
            {
                g.AddEdge(vertices[2 * i], vertices[2 * i + 1]);
            }
            return g;
        }

        /// <summary>
        /// Returns a uniformly random tree on V vertices.
        /// This algorithm uses a Prufer sequence and takes time proportional to <em>V log V</em>.
        /// http://www.proofwiki.org/wiki/Labeled_Tree_from_Prüfer_Sequence
        /// http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.36.6484&amp;rep=rep1&amp;type=pdf
        /// </summary>
        /// <param name="v">the number of vertices in the tree</param>
        /// <returns>a uniformly random tree on V vertices</returns>
        public static Graph Tree(int v)
        {
            Graph g = new Graph(v);

            // special case
            if (v == 1)
            {
                return g;
            }

            // Cayley's theorem: there are V^(V-2) labeled trees on V vertices
            // Prufer sequence: sequence of V-2 values between 0 and V-1
            // Prufer's proof of Cayley's theorem: Prufer sequences are in 1-1
            // with labeled trees on V vertices
            int[] prufer = new int[v - 2];
            for (int i = 0; i < v - 2; i++)
            {
                prufer[i] = StdRandom.Uniform(v);
            }

            // degree of vertex v = 1 + number of times it appers in Prufer sequence
            int[] degree = new int[v];
            for (int vertex = 0; vertex < v; vertex++)
            {
                degree[vertex] = 1;
            }
            for (int i = 0; i < v - 2; i++)
            {
                degree[prufer[i]]++;
            }

            // pq contains all vertices of degree 1
            MinPQ<int> pq = new MinPQ<int>();
            for (int vertex = 0; vertex < v; vertex++)
            {
                if (degree[vertex] == 1)
                {
                    pq.Insert(vertex);
                }
            }

            // repeatedly DelMin() degree 1 vertex that has the minimum index
            for (int i = 0; i < v - 2; i++)
            {
                int vertex = pq.DelMin();
                g.AddEdge(vertex, prufer[i]);
                degree[vertex]--;
                degree[prufer[i]]--;
                if (degree[prufer[i]] == 1)
                {
                    pq.Insert(prufer[i]);
                }
            }
            g.AddEdge(pq.DelMin(), pq.DelMin());
            return g;
        }

        /// <summary>
        /// Unit tests the GraphGenerator library.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            int v = int.Parse(args[0]);
            int e = int.Parse(args[1]);
            int v1 = v / 2;
            int v2 = v - v1;

            StdOut.PrintLn("complete graph");
            StdOut.PrintLn(Complete(v));
            StdOut.PrintLn();

            StdOut.PrintLn("simple");
            StdOut.PrintLn(Simple(v, e));
            StdOut.PrintLn();

            StdOut.PrintLn("Erdos-Renyi");
            double p = e / (v * (v - 1) / 2.0);
            StdOut.PrintLn(Simple(v, p));
            StdOut.PrintLn();

            StdOut.PrintLn("complete bipartite");
            StdOut.PrintLn(CompleteBipartite(v1, v2));
            StdOut.PrintLn();

            StdOut.PrintLn("bipartite");
            StdOut.PrintLn(Bipartite(v1, v2, e));
            StdOut.PrintLn();

            StdOut.PrintLn("Erdos Renyi bipartite");
            double q = (double)e / (v1 * v2);
            StdOut.PrintLn(Bipartite(v1, v2, q));
            StdOut.PrintLn();

            StdOut.PrintLn("path");
            StdOut.PrintLn(Path(v));
            StdOut.PrintLn();

            StdOut.PrintLn("cycle");
            StdOut.PrintLn(Cycle(v));
            StdOut.PrintLn();

            StdOut.PrintLn("binary tree");
            StdOut.PrintLn(BinaryTree(v));
            StdOut.PrintLn();

            StdOut.PrintLn("tree");
            StdOut.PrintLn(Tree(v));
            StdOut.PrintLn();

            StdOut.PrintLn("4-regular");
            StdOut.PrintLn(Regular(v, 4));
            StdOut.PrintLn();

            StdOut.PrintLn("star");
            StdOut.PrintLn(Star(v));
            StdOut.PrintLn();

            StdOut.PrintLn("wheel");
            StdOut.PrintLn(Wheel(v));
            StdOut.PrintLn();
        }
    }
}