﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Diagnostics;

namespace SolarSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double seconds = stopwatch.ElapsedMilliseconds / 1000.0;
            Physics physics = new Physics();
            physics.Update(new List<SimObject>(), seconds);

            /* Setup SimObjList & GraphicsController */
            Graphics.GraphicsController graphCon = new Graphics.GraphicsController();
            SimObject newSim = new SimObject();
            newSim.GraphicsObj = graphCon.ReadObjFile("sphere.obj");
            newSim.PhysicObj = new PhysicObject();
            newSim.Position = new double[] { 0.0, 0.0, 0.0 };
            newSim.PhysicObj.Velocity = new double[] { 0.1, 0.0, 0.0 };
            newSim.PhysicObj.Acceleration = new double[] { 0.0, 0.0, 0.0 };
            
            RenderWindow renderWindow = new RenderWindow(800, 600, OpenTK.Graphics.GraphicsMode.Default, "Solar Simulation");
            renderWindow.AddDrawObj(newSim);
            renderWindow.InitScene();
            renderWindow.Run();
        }
    }
}
