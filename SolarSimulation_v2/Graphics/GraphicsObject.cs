using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation_v2.Graphics
{
    class GraphicsObject
    {
        public double[] Scale
        { get; set; }

        public int CastIndex;

        public GraphicsObject(int castIndex, double[] scale)
        {
            Scale = scale;
            CastIndex = castIndex;
        }
    }
}
