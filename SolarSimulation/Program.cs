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
            PhysicObject sunPhys = new PhysicObject();
            sunPhys.mass = 1.98843 * Math.Pow(10, 30);
            sunPhys.radius = 696342;
            sunPhys.Velocity = new double[3];
            sunPhys.Acceleration = new double[3];
            SimObject sunSim = new SimObject(new double[] {0, 0, 0}, sunPhys, graphCon.ReadObjFile("sphere.obj"));
            sunSim.Scale = new double[] { sunPhys.radius, sunPhys.radius, sunPhys.radius };

            PhysicObject earthPhys = new PhysicObject();
            earthPhys.mass = 5.97223 * Math.Pow(10, 24);
            earthPhys.radius = 6371;
            earthPhys.Velocity = new double[] { 0, 0, -107219 };
            earthPhys.Acceleration = new double[3];
            SimObject earthSim = new SimObject(new double[] { 147098073, 0, 0 }, earthPhys, graphCon.ReadObjFile("sphere.obj"));
            earthSim.Scale = new double[] { earthPhys.radius, earthPhys.radius, earthPhys.radius };

            transList = physics.Update(new List<SimObject>(), seconds);

            PhysicObject mercuryPhys = new PhysicObject();
            mercuryPhys.mass = 3.3022 * Math.Pow(10, 23);
            mercuryPhys.radius = 2439.7;
            mercuryPhys.Velocity = new double[] { 0, 0, 47362 / 3600 };
            mercuryPhys.Acceleration = new double[3];
            SimObject mercurySim = new SimObject(new double[] { 57909050, 0, 0 }, mercuryPhys, graphCon.ReadObjFile("sphere.obj"));
            mercurySim.Scale = new double[] { mercuryPhys.radius, mercuryPhys.radius, mercuryPhys.radius };

            PhysicObject venusPhys = new PhysicObject();
            venusPhys.mass = 4.8676 * Math.Pow(10, 24);
            venusPhys.radius = 6051.8;
            venusPhys.Velocity = new double[] { 0, 0, 35.02 };
            venusPhys.Acceleration = new double[3];
            SimObject venusSim = new SimObject(new double[] { 108208000, 0, 0 }, venusPhys, graphCon.ReadObjFile("sphere.obj"));
            venusSim.Scale = new double[] { venusPhys.radius, venusPhys.radius, venusPhys.radius };

            PhysicObject marsPhys = new PhysicObject();
            marsPhys.mass = 6.4185 * Math.Pow(10, 23);
            marsPhys.radius = 3389.5;
            marsPhys.Velocity = new double[] { 0, 0, 24.077};
            marsPhys.Acceleration = new double[3];
            SimObject marsSim = new SimObject(new double[] { 227939100, 0, 0 }, marsPhys, graphCon.ReadObjFile("sphere.obj"));
            marsSim.Scale = new double[] { marsPhys.radius, marsPhys.radius, marsPhys.radius };

            /* Setup SimObjList & GraphicsController */
            RenderWindow renderWindow = new RenderWindow(1024, 768, OpenTK.Graphics.GraphicsMode.Default, "Solar Simulation");
            renderWindow.AddDrawObj(sunSim);
            renderWindow.AddDrawObj(earthSim);
            renderWindow.AddDrawObj(mercurySim);
            renderWindow.AddDrawObj(venusSim);
            renderWindow.AddDrawObj(marsSim);
            renderWindow.InitScene();
            renderWindow.Run();
        }

        /*
            PhysicObject sunPhys = new PhysicObject();
            sunPhys.mass = 1.98843 * Math.Pow(10, 30);
            sunPhys.Velocity = new double[] {0, 0, 0};
            sunPhys.Acceleration = new double[] { 0, 0, 0 };
            SimObject sunSim = new SimObject(new double[] { 0, 0, 0 }, sunPhys, graphCon.ReadObjFile("sphere.obj"));
            double sunRad = 696342;
         *  sunPhys.radius = sunRad;

            PhysicObject earthPhys = new PhysicObject();
            earthPhys.mass = 5.97223 * Math.Pow(10, 24);
            earthPhys.Velocity = new double[] { 105.448 / 3600, 0, 0 };
            earthPhys.Acceleration = new double[] { 0, 0, 0 };
            SimObject earthSim = new SimObject(new double[] { 0, 0, 147098073 }, earthPhys, graphCon.ReadObjFile("sphere.obj"));
            double earthRad = 12745.591 / 2;
         *  earthPhys.radius = earthRad;
         */
    }
}
