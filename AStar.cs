/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *        _______              __     __                     *
 *       / _____/             / /    / /                     *
 *      / /__ _____   ______ / /___ / /__  ______ __  __     *
 *     / ___// __  | / ____// __  // //_/ / __  // /_/ /     *
 *    / /   / /_/ /_|__  | / / / // / \  / ____//___  /      *
 *   /_/   |_____//_____/ /_/ /_//_/ \_\/_____//_____/       *
 *                                                           *
 * This source file is developed by Joshua J. Nelson         *
 * Contact: foshkey@gmail.com                                *
 *                                                           *
 * Permission is hereby granted, free of charge, to any      *
 * person obtaining a copy of this software and associated   *
 * documentation files (the "Software"), to deal in the      *
 * Software without restriction, including without           *
 * limitation the rights to use, copy, modify, merge,        *
 * publish, distribute, sublicense, and/or sell copies of    *
 * the Software, and to permit persons to whom the Software  *
 * is furnished to do so, subject to the following           *
 * conditions:                                               *
 *                                                           *
 * The above notice and this permission notice shall be      *
 * included in all copies or substantial portions of the     *
 * Software.                                                 *
 *                                                           *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY *
 * KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO    *
 * THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A          *
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL *
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, *
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF       *
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN   *
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS *
 * IN THE SOFTWARE.                                          *
 *                                                           *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Finder
{
    class AStar
    {
        /// <summary>
        /// Returns the shortest path route in terms of total traversal distance
        /// </summary>
        /// <param name="G">Graph to traverse</param>
        /// <param name="start">Starting Vertex</param>
        /// <param name="goal">Goal Vertex. Does not have to be connected</param>
        /// <returns>Optimal Path upon success, null list upon failure</returns>
        public static List<Vertex> find_path(Graph G, Vertex start, Vertex goal)
        {
            List<Vertex> closedset = new List<Vertex>(); // The set of nodes already evaluated
            List<Vertex> openset = new List<Vertex>() { start }; // The set of tentative nodes to be evaluated, initially containing the start node
            Dictionary<Vertex, Vertex> came_from = new Dictionary<Vertex, Vertex>(); // The map of navigated nodes

            Dictionary<Vertex, double> g_score = new Dictionary<Vertex, double>() { {start, 0} }; // Cost from start along best known path
            // Estimated total cost from start to goal through y
            Dictionary<Vertex, double> f_score = new Dictionary<Vertex, double>()
            {
                {start, g_score[start] + Graph.distance(start, goal)}
            };

            while (openset.Count > 0)
            {
                Vertex current = find_lowest(f_score, openset);
                if (current == goal)
                    return reconstruct_path(came_from, goal);

                openset.Remove(current);
                closedset.Add(current);
                Vertex[] neighbors = G.neighbors(current);
                foreach (Vertex neighbor in neighbors)
                {
                    if (closedset.Contains(neighbor))
                        continue;
                    double tentative_g_score = g_score[current] + Graph.distance(current, neighbor);

                    if (!openset.Contains(neighbor) || tentative_g_score < g_score[neighbor])
                    {
                        came_from[neighbor] = current;
                        g_score[neighbor] = tentative_g_score;
                        f_score[neighbor] = g_score[neighbor] + Graph.distance(neighbor, goal);
                        if (!openset.Contains(neighbor))
                            openset.Add(neighbor);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the lowest-scoring vertex in set
        /// </summary>
        /// <param name="score"></param>
        /// <param name="set"></param>
        /// <returns>Vertex with lowest score</returns>
        private static Vertex find_lowest(Dictionary<Vertex,double> score, List<Vertex> set)
        {
            if (set.Count == 0) return null;
            int r = 0;
            double lowest = score[set[r]];
            double current = 0;
            for (int i = 0; i < set.Count; i++)
            {
                current = score[set[i]];
                if (current < lowest)
                {
                    lowest = current;
                    r = i;
                }
            }
            return set[r];
        }

        /// <summary>
        /// Reconstructs the path by back tracing through the mapping came_from
        /// </summary>
        /// <param name="came_from"></param>
        /// <param name="current_node"></param>
        /// <returns>Reconstructed path</returns>
        private static List<Vertex> reconstruct_path(Dictionary<Vertex,Vertex> came_from, Vertex current_node)
        {
            List<Vertex> r;
            if (came_from.ContainsKey(current_node))
                r = reconstruct_path(came_from, came_from[current_node]);
            else
                r = new List<Vertex>();
            r.Add(current_node);
            return r;
        }
    }
}
