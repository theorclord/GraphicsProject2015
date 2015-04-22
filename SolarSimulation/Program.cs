using System;
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
            Graphics.GraphicsController graphCon = new Graphics.GraphicsController();
            Physics physics = new Physics();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double seconds = stopwatch.ElapsedMilliseconds / 1000.0;

            List<Matrix4> transList = new List<Matrix4>();
            SimObject newSim = new SimObject();
            newSim.GraphicsObj = graphCon.ReadObjFile("sphere.obj");
            newSim.PhysicObj = new PhysicObject();
            newSim.Position = new double[] { 0.0, 0.0, 0.0 };
            newSim.PhysicObj.Velocity = new double[] { 0.1, 0.0, 0.0 };
            newSim.PhysicObj.Acceleration = new double[] { 0.0, 0.0, 0.0 };
            
            transList = physics.Update(new List<SimObject>(), seconds);

            /* Setup SimObjList & GraphicsController */
            RenderWindow renderWindow = new RenderWindow(400, 400, OpenTK.Graphics.GraphicsMode.Default, "Solar Simulation");
            renderWindow.AddDrawObj(newSim);
            renderWindow.InitScene();
            renderWindow.Run();
        }
    }
}
