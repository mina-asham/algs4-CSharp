using System.Collections.Generic;

namespace algs4.algs4
{
    public class DijkstraAllPairsSP
    {
        private readonly DijkstraSP[] _all;

        /// <summary>
        /// Computes a shortest paths tree from each vertex to to every other vertex in
        /// the edge-weighted digraph G.
        /// </summary>
        /// <param name="g">the edge-weighted digraph</param>
        public DijkstraAllPairsSP(EdgeWeightedDigraph g)
        {
            _all = new DijkstraSP[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                _all[v] = new DijkstraSP(g, v);
            }
        }

        /// <summary>
        /// Returns a shortest path from vertex s to vertex t.
        /// </summary>
        /// <param name="s">the source vertex</param>
        /// <param name="t">the destination vertex</param>
        /// <returns>a shortest path from vertex s to vertex t as an iterable of edges, and null if no such path</returns>
        public IEnumerable<DirectedEdge> Path(int s, int t)
        {
            return _all[s].PathTo(t);
        }

        /// <summary>
        /// Is there a path from the vertex s to vertex t?
        /// </summary>
        /// <param name="s">the source vertex</param>
        /// <param name="t">the destination vertex</param>
        /// <returns>true if there is a path from vertex s to vertex t, and false otherwise</returns>
        public bool HasPath(int s, int t)
        {
            return Dist(s, t) < double.PositiveInfinity;
        }

        /// <summary>
        /// Returns the length of a shortest path from vertex s to vertex t.
        /// </summary>
        /// <param name="s">the source vertex</param>
        /// <param name="t">the destination vertex</param>
        /// <returns>the length of a shortest path from vertex s to vertex t; double.PositiveInfinity if no such path</returns>
        public double Dist(int s, int t)
        {
            return _all[s].DistTo(t);
        }
    }
}