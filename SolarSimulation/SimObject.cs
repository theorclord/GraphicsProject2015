﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolarSimulation.Graphics;
using SolarSimulation.Physics;

namespace SolarSimulation
{
    struct SimObject
    {
        public double[] Position
        { get; set; }

        public double[] Scale
        { get; set; }

        public PhysicsObject PhysicsObj
        { get; set; }

		public GraphicsObject GraphicsObj
		{ get; set; }

        public SimObject(double[] position, PhysicsObject pObj, GraphicsObject gObj) : this()
        {
            Position = position;
            PhysicsObj = pObj;
            GraphicsObj = gObj;
        }
    }
}
