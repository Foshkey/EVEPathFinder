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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Finder
{
    struct Vector
    {
        public double x;
        public double y;
        public double z;

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return String.Format("({0:0.000e+00}, {1:0.000e+00}, {2:0.000e+00})", x, y, z);
        }
    }

    class Vertex
    {
        public long id;
        public string name;
        public Vector pos;

        public Vertex() : this("") { }

        public Vertex(string nData) : this(0, nData, new Vector()) { }

        public Vertex(string nData, Vector nPos) : this(0, nData, nPos) { }

        public Vertex(long nid, string nName, Vector nPos)
        {
            id = nid;
            name = nName;
            pos = nPos;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
