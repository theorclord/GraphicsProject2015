using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace SolarSimulation.Graphics
{
	public struct Triangle
	{
		public int vi1;
		public int vi2;
		public int vi3;
		public int ni1;
		public int ni2;
		public int ni3;

        public Triangle(int vi1, int vi2, int vi3, int ni1, int ni2, int ni3)
        {
            this.vi1 = vi1;
            this.vi2 = vi2;
            this.vi3 = vi3;
            this.ni1 = ni1;
            this.ni2 = ni2;
            this.ni3 = ni3;
        }
	}
}