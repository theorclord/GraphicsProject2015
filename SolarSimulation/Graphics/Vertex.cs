using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation.Graphics
{
	public struct Vertex
	{
		public double x;
		public double y;
		public double z;

        public Vertex(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
	}
}