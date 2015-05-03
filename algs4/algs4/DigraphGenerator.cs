using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class DigraphGenerator
    {
        private class Edge : IComparable<Edge>
        {
            private readonly int _v;
            private readonly int _w;

            public Edge(int v, int w)
            {
                _v = v;
                _w = w;
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
        /// Returns a random simple digraph containing V vertices and E edges.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="e">the number of vertices</param>
        /// <returns>a random simple digraph on V vertices, containing a total of E edges</returns>
        public static Digraph Simple(int v, int e)
        {
            if (e > (long)v * (v - 1))
            {
                throw new ArgumentException("Too many edges");
            }
            if (e < 0)
            {
                throw new ArgumentException("Too few edges");
            }
            Digraph g = new Digraph(v);
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
        /// Returns a random simple digraph on V vertices, with an 
        /// edge between any two vertices with probability p. This is sometimes
        /// referred to as the Erdos-Renyi random digraph model.
        /// This implementations takes time propotional to V^2 (even if p is small).
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="p">the probability of choosing an edge</param>
        /// <returns>a random simple digraph on V vertices, with an edge between any two vertices with probability p</returns>
        public static Digraph Simple(int v, double p)
        {
            if (p < 0.0 || p > 1.0)
            {
                throw new ArgumentException("Probability must be between 0 and 1");
            }
            Digraph g = new Digraph(v);
            for (int vertex = 0; vertex < v; vertex++)
            {
                for (int w = 0; w < v; w++)
                {
                    if (vertex != w)
                    {
                        if (StdRandom.Bernoulli(p))
                        {
                            g.AddEdge(vertex, w);
                        }
                    }
                }
            }
            return g;
        }

        /// <summary>
        /// Returns the complete digraph on V vertices.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <returns>the complete digraph on V vertices</returns>
        public static Digraph Complete(int v)
        {
            return Simple(v, v * (v - 1));
        }

        /// <summary>
        /// Returns a random simple DAG containing V vertices and E edges.
        /// Note: it is not uniformly selected at random among all such DAGs.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="e">the number of vertices</param>
        /// <returns>a random simple DAG on V vertices, containing a total of E edges</returns>
        public static Digraph DAG(int v, int e)
        {
            if (e > (long)v * (v - 1) / 2)
            {
                throw new ArgumentException("Too many edges");
            }
            if (e < 0)
            {
                throw new ArgumentException("Too few edges");
            }
            Digraph g = new Digraph(v);
            Set<Edge> set = new Set<Edge>();
            int[] vertices = new int[v];
            for (int i = 0; i < v; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);
            while (g.E() < e)
            {
                int vCount = StdRandom.Uniform(v);
                int wCount = StdRandom.Uniform(v);
                Edge edge = new Edge(vCount, wCount);
                if ((vCount < wCount) && !set.Contains(edge))
                {
                    set.Add(edge);
                    g.AddEdge(vertices[vCount], vertices[wCount]);
                }
            }
            return g;
        }

        /// <summary>
        /// Returns a random tournament digraph on V vertices. A tournament digraph
        /// is a DAG in which for every two vertices, there is one directed edge.
        /// A tournament is an oriented complete graph.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <returns>a random tournament digraph on V vertices</returns>
        public static Digraph Tournament(int v)
        {
            Digraph g = new Digraph(v);
            for (int vertex = 0; vertex < g.V(); vertex++)
            {
                for (int w = vertex + 1; w < g.V(); w++)
                {
                    if (StdRandom.Bernoulli(0.5))
                    {
                        g.AddEdge(vertex, w);
                    }
                    else
                    {
                        g.AddEdge(w, vertex);
                    }
                }
            }
            return g;
        }

        /// <summary>
        /// Returns a random rooted-in DAG on V vertices and E edges.
        /// A rooted in-tree is a DAG in which there is a single vertex
        /// reachable from every other vertex.
        /// The DAG returned is not chosen uniformly at random among all such DAGs.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="e">the number of edges</param>
        /// <returns>a random rooted-in DAG on V vertices and E edges</returns>
        public static Digraph RootedInDAG(int v, int e)
        {
            if (e > (long)v * (v - 1) / 2)
            {
                throw new ArgumentException("Too many edges");
            }
            if (e < v - 1)
            {
                throw new ArgumentException("Too few edges");
            }
            Digraph g = new Digraph(v);
            Set<Edge> set = new Set<Edge>();

            // fix a topological order
            int[] vertices = new int[v];
            for (int i = 0; i < v; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);

            // one edge pointing from each vertex, other than the root = vertices[V-1]
            for (int vertex = 0; vertex < v - 1; vertex++)
            {
                int w = StdRandom.Uniform(vertex + 1, v);
                Edge edge = new Edge(vertex, w);
                set.Add(edge);
                g.AddEdge(vertices[vertex], vertices[w]);
            }

            while (g.E() < e)
            {
                int vCount = StdRandom.Uniform(v);
                int wCount = StdRandom.Uniform(v);
                Edge edge = new Edge(vCount, wCount);
                if ((vCount < wCount) && !set.Contains(edge))
                {
                    set.Add(edge);
                    g.AddEdge(vertices[vCount], vertices[wCount]);
                }
            }
            return g;
        }

        /// <summary>
        /// Returns a random rooted-out DAG on V vertices and E edges.
        /// A rooted out-tree is a DAG in which every vertex is reachable from a
        /// single vertex.
        /// The DAG returned is not chosen uniformly at random among all such DAGs.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="e">the number of edges</param>
        /// <returns>a random rooted-out DAG on V vertices and E edges</returns>
        public static Digraph RootedOutDAG(int v, int e)
        {
            if (e > (long)v * (v - 1) / 2)
            {
                throw new ArgumentException("Too many edges");
            }
            if (e < v - 1)
            {
                throw new ArgumentException("Too few edges");
            }
            Digraph g = new Digraph(v);
            Set<Edge> set = new Set<Edge>();

            // fix a topological order
            int[] vertices = new int[v];
            for (int i = 0; i < v; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);

            // one edge pointing from each vertex, other than the root = vertices[V-1]
            for (int vertex = 0; vertex < v - 1; vertex++)
            {
                int wCount = StdRandom.Uniform(vertex + 1, v);
                Edge edge = new Edge(wCount, vertex);
                set.Add(edge);
                g.AddEdge(vertices[wCount], vertices[vertex]);
            }

            while (g.E() < e)
            {
                int vCount = StdRandom.Uniform(v);
                int wCount = StdRandom.Uniform(v);
                Edge edge = new Edge(wCount, vCount);
                if ((vCount < wCount) && !set.Contains(edge))
                {
                    set.Add(edge);
                    g.AddEdge(vertices[wCount], vertices[vCount]);
                }
            }
            return g;
        }

        /// <summary>
        /// Returns a random rooted-in tree on V vertices.
        /// A rooted in-tree is an oriented tree in which there is a single vertex
        /// reachable from every other vertex.
        /// The tree returned is not chosen uniformly at random among all such trees.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <returns>a random rooted-in tree on V vertices</returns>
        public static Digraph RootedInTree(int v)
        {
            return RootedInDAG(v, v - 1);
        }

        /// <summary>
        /// Returns a random rooted-out tree on V vertices. A rooted out-tree
        /// is an oriented tree in which each vertex is reachable from a single vertex.
        /// It is also known as a arborescence or branching.
        /// The tree returned is not chosen uniformly at random among all such trees.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <returns>a random rooted-out tree on V vertices</returns>
        public static Digraph RootedOutTree(int v)
        {
            return RootedOutDAG(v, v - 1);
        }

        /// <summary>
        /// Returns a path digraph on V vertices.
        /// </summary>
        /// <param name="v">the number of vertices in the path</param>
        /// <returns>a digraph that is a directed path on V vertices</returns>
        public static Digraph Path(int v)
        {
            Digraph g = new Digraph(v);
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
        /// Returns a complete binary tree digraph on V vertices.
        /// </summary>
        /// <param name="v">the number of vertices in the binary tree</param>
        /// <returns>a digraph that is a complete binary tree on V vertices</returns>
        public static Digraph BinaryTree(int v)
        {
            Digraph g = new Digraph(v);
            int[] vertices = new int[v];
            for (int i = 0; i < v; i++) vertices[i] = i;
            StdRandom.Shuffle(vertices);
            for (int i = 1; i < v; i++)
            {
                g.AddEdge(vertices[i], vertices[(i - 1) / 2]);
            }
            return g;
        }

        /// <summary>
        /// Returns a cycle digraph on V vertices.
        /// </summary>
        /// <param name="v">the number of vertices in the cycle</param>
        /// <returns>a digraph that is a directed cycle on v vertices</returns>
        public static Digraph Cycle(int v)
        {
            Digraph g = new Digraph(v);
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
        /// Returns a random simple digraph on V vertices, E
        /// edges and (at least) c strong components. The vertices are randomly
        /// assigned int labels between 0 and c-1 (corresponding to 
        /// strong components). Then, a strong component is creates among the vertices
        /// with the same label. Next, random edges (either between two vertices with
        /// the same labels or from a vetex with a smaller label to a vertex with a 
        /// larger label). The number of components will be equal to the number of
        /// distinct labels that are assigned to vertices.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="e">the number of edges</param>
        /// <param name="c">the (maximum) number of strong components</param>
        /// <returns>a random simple digraph on V vertices and E edges, with (at most) c strong components</returns>
        public static Digraph Strong(int v, int e, int c)
        {
            if (c >= v || c <= 0)
            {
                throw new ArgumentException("Number of components must be between 1 and V");
            }
            if (e <= 2 * (v - c))
            {
                throw new ArgumentException("Number of edges must be at least 2(V-c)");
            }
            if (e > (long)v * (v - 1) / 2)
            {
                throw new ArgumentException("Too many edges");
            }

            // the digraph
            Digraph g = new Digraph(v);

            // edges.Added to G (to avoid duplicate edges)
            Set<Edge> set = new Set<Edge>();

            int[] label = new int[v];
            for (int vertex = 0; vertex < v; vertex++)
            {
                label[vertex] = StdRandom.Uniform(c);
            }

            // make all vertices with label c a strong component by
            // combining a rooted in-tree and a rooted out-tree
            for (int i = 0; i < c; i++)
            {
                // how many vertices in component c
                int count = 0;
                for (int vertex = 0; vertex < g.V(); vertex++)
                {
                    if (label[vertex] == i)
                    {
                        count++;
                    }
                }

                // if (count == 0) System.err.println("less than desired number of strong components");

                int[] vertices = new int[count];
                int j = 0;
                for (int vertex = 0; vertex < v; vertex++)
                {
                    if (label[vertex] == i)
                    {
                        vertices[j++] = vertex;
                    }
                }
                StdRandom.Shuffle(vertices);

                // rooted-in tree with root = vertices[count-1]
                for (int vertex = 0; vertex < count - 1; vertex++)
                {
                    int w = StdRandom.Uniform(vertex + 1, count);
                    Edge edge = new Edge(w, vertex);
                    set.Add(edge);
                    g.AddEdge(vertices[w], vertices[vertex]);
                }

                // rooted-out tree with root = vertices[count-1]
                for (int vertex = 0; vertex < count - 1; vertex++)
                {
                    int w = StdRandom.Uniform(vertex + 1, count);
                    Edge edge = new Edge(vertex, w);
                    set.Add(edge);
                    g.AddEdge(vertices[vertex], vertices[w]);
                }
            }

            while (g.E() < e)
            {
                int vCount = StdRandom.Uniform(v);
                int wCount = StdRandom.Uniform(v);
                Edge edge = new Edge(vCount, wCount);
                if (!set.Contains(edge) && vCount != wCount && label[vCount] <= label[wCount])
                {
                    set.Add(edge);
                    g.AddEdge(vCount, wCount);
                }
            }

            return g;
        }

        /// <summary>
        /// Unit tests the DigraphGenerator library.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            int v = int.Parse(args[0]);
            int e = int.Parse(args[1]);
            Console.WriteLine("complete graph");
            Console.WriteLine(Complete(v));
            Console.WriteLine();

            Console.WriteLine("simple");
            Console.WriteLine(Simple(v, e));
            Console.WriteLine();

            Console.WriteLine("path");
            Console.WriteLine(Path(v));
            Console.WriteLine();

            Console.WriteLine("cycle");
            Console.WriteLine(Cycle(v));
            Console.WriteLine();

            Console.WriteLine("binary tree");
            Console.WriteLine(BinaryTree(v));
            Console.WriteLine();

            Console.WriteLine("tournament");
            Console.WriteLine(Tournament(v));
            Console.WriteLine();

            Console.WriteLine("DAG");
            Console.WriteLine(DAG(v, e));
            Console.WriteLine();

            Console.WriteLine("rooted-in DAG");
            Console.WriteLine(RootedInDAG(v, e));
            Console.WriteLine();

            Console.WriteLine("rooted-out DAG");
            Console.WriteLine(RootedOutDAG(v, e));
            Console.WriteLine();

            Console.WriteLine("rooted-in tree");
            Console.WriteLine(RootedInTree(v));
            Console.WriteLine();

            Console.WriteLine("rooted-out DAG");
            Console.WriteLine(RootedOutTree(v));
            Console.WriteLine();
        }
    }
}
