using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SolarSimulation_v2.Physics
{
    class PhysicsController
    {

        /// <summary>
        /// Updates the physic objects and returns the related translation matrices
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="timeSinceLastFrame"></param>
        /// <returns>Opentk Matrix4 used for translation</returns>
        public List<Matrix4> Update(List<SimObject> objects, double timeSinceLastFrame)
        {
            checkCollision(objects);

            //Calculates the forces on each of the objects and updates the acceleration.
            calcForces(objects);

            //Calculates and updates the velocity for each object
            calcVelocity(objects, timeSinceLastFrame);

            //Moves the objects to their new position.
            return updatePosition(objects, timeSinceLastFrame);

        }
        private void calcImpuls(SimObject o1, SimObject o2)
        {
            double[] tempV1 = o1.PhysicsObj.Velocity;
            for(int i = 0; i<3;i++)
            {
                o1.PhysicsObj.Velocity[i] = o2.PhysicsObj.Mass / o1.PhysicsObj.Mass * o2.PhysicsObj.Velocity[i];
            }
            for (int i = 0; i < 3; i++)
            {
                o2.PhysicsObj.Velocity[i] = o1.PhysicsObj.Mass / o2.PhysicsObj.Mass * tempV1[i];
            }
        }

        private void checkCollision(List<SimObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i+1; j < list.Count; j++)
                {
                    if (distance(list[i], list[j]) < list[i].PhysicsObj.Radius + list[j].PhysicsObj.Radius)
                    {
                        calcImpuls(list[i], list[j]);
                    }
                }
            }
        }

        private List<Matrix4> updatePosition(List<SimObject> objects, double timeSinceLastFrame)
        {
            List<Matrix4> translationMatrices = new List<Matrix4>();
            Vector3 transVector = new Vector3();
            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = 0; j < objects[i].PhysicsObj.Velocity.Length; j++)
                {
                    var posChange = (objects[i].PhysicsObj.Velocity[j] * timeSinceLastFrame) / 1000;
                    objects[i].Position[j] += posChange;
                    transVector[j] = (float)posChange;
                }
                Matrix4 transMatrix = Matrix4.CreateTranslation(transVector);
                translationMatrices.Add(transMatrix);
            }
            return translationMatrices;
        }

        private void calcVelocity(List<SimObject> objects, double timeSinceLastFrame)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = 0; j < objects[i].PhysicsObj.Velocity.Length; j++)
                {
                    objects[i].PhysicsObj.Velocity[j] += objects[i].PhysicsObj.Acceleration[j]*timeSinceLastFrame;
                }
            }
        }

        private void calcForces(List<SimObject> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].PhysicsObj.Acceleration = new double[3];
                for (int j = 0; j < objects.Count; j++)
                {
                    if (i == j)
                    { continue; }

                    double[] force = calcForce(objects[i], objects[j]);
                    for (int k = 0; k < objects[i].PhysicsObj.Acceleration.Length; k++)
                    {
                        objects[i].PhysicsObj.Acceleration[k] += -(force[k] / objects[i].PhysicsObj.Mass);
                    }
                }
            }
        }


        private double[] calcForce(SimObject obj1, SimObject obj2)
        {
            double gravityConst = 6.673 * Math.Pow(10.0, -11.0);
            double[] force = new double[obj1.Position.Length];
            
            double dist = distance(obj1, obj2);

            double forceValue = gravityConst * (obj1.PhysicsObj.Mass * obj2.PhysicsObj.Mass) / Math.Pow(dist * Math.Pow(10, 3), 2.0);

            for (int i = 0; i < obj1.Position.Length; i++)
            {
                force[i] = obj1.Position[i] - obj2.Position[i];
            }
            force = norm(force);

            for (int i = 0; i < obj1.Position.Length; i++)
            {
                force[i] = force[i] * forceValue;
            }

            return force;
        }

        private double[] norm(double[] arr)
        {
            double length = 0.0;
            for (int i = 0; i < arr.Length; i++)
            {
                length += Math.Pow(arr[i], 2);
            }
            length = Math.Sqrt(length);

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = arr[i] / length;
            }
            return arr;
        }

        private double distance(SimObject obj1, SimObject obj2)
        {
            double dist = 0.0;
            // Calculation the distance between the objects
            for (int i = 0; i < obj1.Position.Length; i++)
            {
                dist += Math.Pow((obj1.Position[i] - obj2.Position[i]), 2);
            }
            return dist = Math.Sqrt(dist);
        }
    }
}
