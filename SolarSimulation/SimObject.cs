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

        public SimObject(double[] position, PhysicObject pObj, GraphicsObject gObj)
        {
            this.Position = position;
            this.PhysicObj = pObj;
            this.GraphicsObj = gObj;
        }
    }
}
