using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace SolarSimulation_v2.Graphics
{
	public struct Triangle
	{
        public int vi1, vi2, vi3;
        public int ni1, ni2, ni3;

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