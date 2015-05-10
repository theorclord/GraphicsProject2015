using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation.Physics
{
    class PhysicsObject
    {
        public double Mass { get; set; }
        public double Radius { get; set; }
        public double[] Velocity { get; set; }
        public double[] Acceleration { get; set;}

        public PhysicsObject()
        { }

        public PhysicsObject(double mass, double radius, double[] velocity) : this()
        {
            Mass = mass;
            Radius = radius;
            Velocity = velocity;
            Acceleration = new double[3]; 
        }
    }
}
