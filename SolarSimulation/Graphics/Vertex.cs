using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation.Graphics
{
	public struct Vertex
	{
		public double x, y, z;
        public double u, v;

        public Vertex(double x, double y, double z, double u, double v)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.u = u;
            this.v = v;
        }
	}
}