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
    class Graph
    {
        private Dictionary<Vertex, List<Vertex>> AdjacencyList;
        //private Dictionary<Vertex, int> indices; // Look up dictionary for vertices

        public Graph()
        {
            AdjacencyList = new Dictionary<Vertex, List<Vertex>>();
            //indices = new Dictionary<Vertex, int>();
        }

        /// <summary>
        /// Gets the distance using struct Vector stored in Vertices
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>distance</returns>
        public static double distance(Vertex x, Vertex y)
        {
            return Math.Sqrt(Math.Pow(x.pos.x - y.pos.x, 2) +
                             Math.Pow(x.pos.y - y.pos.y, 2) +
                             Math.Pow(x.pos.z - y.pos.z, 2));
        }

        /// <summary>
        /// Gets the array of all Vertices in the graph
        /// </summary>
        /// <returns></returns>
        public Vertex[] getVertices()
        {
            try { return AdjacencyList.Keys.ToArray(); }
            catch { return new Vertex[0]; }
        }

        /// <summary>
        /// Tests if two vertices are adjacent
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if adjacent</returns>
        public bool adjacent(Vertex x, Vertex y)
        {
            try { return AdjacencyList[x].Contains(y); }
            catch { return false; }
        }

        /// <summary>
        /// Returns all neighbors (adjacent nodes) of Vertex x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public Vertex[] neighbors(Vertex x)
        {
            try { return AdjacencyList[x].ToArray(); }
            catch { return new Vertex[0]; }
        }

        /// <summary>
        /// Adds a vertex to the graph
        /// </summary>
        /// <param name="x"></param>
        public void add(Vertex x)
        {
            try
            {
                //indices.Add(x, AdjacencyList.Count);
                AdjacencyList.Add(x, new List<Vertex>());
            }
            catch { return; }
        }

        /// <summary>
        /// Adds an edge from vertex x to y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void add(Vertex x, Vertex y)
        {
            try
            {
                if (!adjacent(x, y))
                {
                    AdjacencyList[x].Add(y);
                    AdjacencyList[y].Add(x);
                }
            }
            catch { return; }
        }

        /// <summary>
        /// Deletes vertex x
        /// </summary>
        /// <param name="x"></param>
        public void delete(Vertex x)
        {
            try { AdjacencyList.Remove(x); }
            catch { return; }
        }

        /// <summary>
        /// Deletes the edge connecting x & y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void delete(Vertex x, Vertex y)
        {
            try
            {
                AdjacencyList[x].Remove(y);
                AdjacencyList[y].Remove(x);
            }
            catch { return; }
        }

        /// <summary>
        /// Gets a vertex by name. WARNING: Costly method.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Vertex getVertex(string name)
        {
            // Warning: Costly method. Avoid using.
            foreach (Vertex x in AdjacencyList.Keys)
                if (x.name.Equals(name)) return x;
            return null;
        }

        /// <summary>
        /// Gets a Vertex by id. Use this method as opposed to get vertex by name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Vertex getVertexByID(long id)
        {
            foreach (Vertex x in AdjacencyList.Keys)
                if (x.id == id) return x;
            return null;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Vertex x in AdjacencyList.Keys)
            {
                sb.Append(x.name + " " + x.pos + ": ");
                foreach (Vertex y in AdjacencyList[x])
                {
                    sb.Append(y.name + ", ");
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
