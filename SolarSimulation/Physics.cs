﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation
{
    class Physics
    {


        public void Update(List<SimObject> objects, double timeSinceLastFrame)
        {
            //Calculates the forces on each of the objects and updates the acceleration.
            calcForces(objects);

            //Calculates and updates the velocity for each object
            calcVelocity(objects, timeSinceLastFrame);

            //Moves the objects to their new position.
            updatePosition(objects, timeSinceLastFrame);

        }

        private void updatePosition(List<SimObject> objects, double timeSinceLastFrame)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = 0; j < objects[i].PhysicObj.Velocity.Length; j++)
                {
                    objects[i].Position[j] += objects[i].Position[j] + objects[i].PhysicObj.Velocity[j] * timeSinceLastFrame;
                }
            }
        }

        private void calcVelocity(List<SimObject> objects, double timeSinceLastFrame)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = 0; j < objects[i].PhysicObj.Velocity.Length; j++)
                {
                    objects[i].PhysicObj.Velocity[j] += objects[i].PhysicObj.Velocity[j] + objects[i].PhysicObj.Acceleration[j]*timeSinceLastFrame;
                }
            }
        }

        private void calcForces(List<SimObject> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = 0; j < objects.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    double[] force = calcForce(objects[i], objects[j]);
                    for (int k = 0; k < objects[i].PhysicObj.Acceleration.Length; k++)
                    {
                        objects[i].PhysicObj.Acceleration[k] += force[k] / objects[i].PhysicObj.mass;
                    }
                }
            }
        }


        private double[] calcForce(SimObject obj1, SimObject obj2)
        {
            double gravityConst = 6.673*Math.Pow(10.0,-11.0);
            double[] force = new double[obj1.Position.Length];
            double dist = 0.0;
            // Calculation the distance between the objects
            for(int i = 0; i<obj1.Position.Length; i++){
                dist += Math.Pow((obj1.Position[i] - obj2.Position[i]), 2);
            }
            dist = Math.Sqrt(dist);

            double forceValue = gravityConst * (obj1.PhysicObj.mass*obj2.PhysicObj.mass)/Math.Pow(dist,2.0);

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
    }
}
