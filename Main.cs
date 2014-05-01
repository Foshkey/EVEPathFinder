/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *        _______              __     __                     *
 *       / _____/             / /    / /                     *
 *      / /__ _____   ______ / /___ / /__  ______ __  __     *
 *     / ___// __  | / ____// __  // //_/ / __  // /_/ /     *
 *    / /   / /_/ /_|__  | / / / // / \  / ____//___  /      *
 *   /_/   |_____//_____/ /_/ /_//_/ \_\/_____//_____/       *
 *                                                           *
 * This source file is developed by Joshua J. Nelson         *
 * Copyright ©2013 Foshkey Productions, foshkey@gmail.com    *
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
 * The above copyright notice and this permission notice     *
 * shall be included in all copies or substantial portions   *
 * of the Software.                                          *
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Path_Finder
{
    public partial class Main : Form
    {
        private Graph mainGraph;
        private const double KILOMETERS_IN_LIGHTYEAR = 9460730472580.800;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // Generate EVE graph and store it in main graph data structure
            mainGraph = getEVEGraph();

            // Load graph into text box
            rtbMain.Text = "Current Graph:\n" + mainGraph;

            // Fill in the drop down boxes
            Vertex[] vertices = mainGraph.getVertices();
            foreach (Vertex v in vertices)
            {
                cmbStart.Items.Add(v);
                cmbGoal.Items.Add(v);
            }
        }

        private void cmbStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePath();
        }

        private void cmbGoal_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePath();
        }

        /// <summary>
        /// Gets the total traversal distance from the list of vertices
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private double getTotalDistance(Vertex[] list)
        {
            double r = 0;
            for (int i = 1; i < list.Length; i++)
            {
                r += Graph.distance(list[i - 1], list[i]);
            }
            return r;
        }

        /// <summary>
        /// Updates the current path, if there is one. This is where the AStar method is called.
        /// </summary>
        private void updatePath()
        {
            if (cmbStart.SelectedItem == null || cmbGoal.SelectedItem == null) return;

            // Get AStar path
            List<Vertex> path = AStar.find_path(mainGraph, (Vertex)cmbStart.SelectedItem, (Vertex)cmbGoal.SelectedItem);

            // Output results
            string txt;
            if (path != null)
            {
                txt = "Found Shortest Path:\n";
                for (int i = 0; i < path.Count; i++)
                {
                    txt += String.Format("{0}: {1}\n", path[i], path[i].pos);
                }
                double totDist = getTotalDistance(path.ToArray());
                txt += String.Format("With a total jump distance of {0:N0} km ({1:N0} light years).\n", totDist, totDist / KILOMETERS_IN_LIGHTYEAR);
            }
            else
            {
                txt = "Path Not Found\n";
            }

            rtbMain.Text = String.Format("{0}\nCurrent Graph:\n{1}", txt, mainGraph);
        }

        /// <summary>
        /// Loads up the universeDataDx.db file as a database. This originates from CCP's database dump.
        /// </summary>
        /// <returns>The loaded graph upon success, null graph on failure</returns>
        private Graph getEVEGraph()
        {
            Graph G = new Graph();

            SQLiteConnection dbconn = new SQLiteConnection("Data Source=universeDataDx.db");
            DataTable dtNodes = new DataTable();
            DataTable dtEdges = new DataTable();
            try
            {
                dbconn.Open();
                SQLiteCommand cmdNodes = new SQLiteCommand("select solarSystemID, solarSystemName, x, y, z from mapSolarSystems where solarSystemID < 31000000", dbconn);
                SQLiteCommand cmdEdges = new SQLiteCommand("select fromSolarSystemID, toSolarSystemID from mapSolarSystemJumps", dbconn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmdNodes);
                da.Fill(dtNodes);
                da = new SQLiteDataAdapter(cmdEdges);
                da.Fill(dtEdges);
            }
            catch
            {
                rtbMain.Text = "Unable to load EVE Graph";
                dbconn.Close();
                return null;
            }
            dbconn.Close();

            // Fill in data
            foreach (DataRow row in dtNodes.Rows)
            {
                var iArr = row.ItemArray;
                if (iArr.Count() < 5) continue;
                long id = (long)iArr[0];
                string name = (string)iArr[1];
                double x = (float)iArr[2];
                double y = (float)iArr[3];
                double z = (float)iArr[4];
                Vertex v = new Vertex(id, name, new Vector(x, y, z));
                G.add(v);
            }

            foreach (DataRow row in dtEdges.Rows)
            {
                var iArr = row.ItemArray;
                if (iArr.Count() < 2) continue;
                G.add(G.getVertexByID((long)iArr[0]), G.getVertexByID((long)iArr[1]));
            }

            return G;
        }

        /// <summary>
        /// Generates a test graph based on the graph featured in the Wikipedia article "Graph (abstract data type)"
        /// Reference: http://en.wikipedia.org/wiki/Graph_(abstract_data_type)
        ///            http://upload.wikimedia.org/wikipedia/commons/5/5b/6n-graf.svg
        /// </summary>
        /// <returns>A generated graph</returns>
        private Graph getTestGraph()
        {
            Graph G = new Graph();
            Vertex[] v = new Vertex[7];
            for (int i = 1; i <= 6; i++)
            {
                v[i] = new Vertex("Node " + i.ToString());
                G.add(v[i]);
            }
            v[1].pos = new Vector(6, 3, 0);
            v[2].pos = new Vector(5, 2, 0);
            v[3].pos = new Vector(3, 1, 0);
            v[4].pos = new Vector(2, 4, 0);
            v[5].pos = new Vector(4, 5, 0);
            v[6].pos = new Vector(1, 6, 0);
            G.add(v[1], v[2]);
            G.add(v[1], v[5]);
            G.add(v[2], v[5]);
            G.add(v[2], v[3]);
            G.add(v[3], v[4]);
            G.add(v[4], v[6]);
            G.add(v[4], v[5]);
            return G;
        }
    }
}
