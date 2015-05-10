using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation
{
    class SimObjectFactory
    {
        public SimObject CreateSimObject(int shapeIndex, double mass, double radius, double[] initialVelocity)
        {
            return new SimObject();
        }

        private Physics.PhysicsObject ConstructPhysicsObject(double mass, double radius, double[] initialVelocity)
        {
            Physics.PhysicsObject simPhys = new Physics.PhysicsObject(mass, radius, initialVelocity);
            return simPhys;
        }
    }
}
