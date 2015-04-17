using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolarSimulation.Graphics;

namespace SolarSimulation
{
    class SimObject
    {
        public double[] Position
        {
            get;
            set;
        }

        public double[] Scale
        {
            get;
            set;
        }
        public PhysicObject PhysicObj
        {
            get;
            set;
        }
		public GraphicsObject GraphicsObj
		{
			get;
			set;
		}
    }
}
