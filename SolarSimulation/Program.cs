using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Diagnostics;
using SolarSimulation.Graphics;
using SolarSimulation.Physics;

namespace SolarSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            GraphicsController graphCon = new GraphicsController();
            List<SimObject> simulatedObjects = new List<SimObject>();
            List<Matrix4> transList = new List<Matrix4>();
            PhysicsController physics = new PhysicsController();
            //float[] curColor = new float[10];

            /* HANDLE GRAPHICS */
            graphCon.ReadObjFile("sphere.obj");

            // The Sun
            GraphicsObject sunGraphics = graphCon.CreateGraphicsObj(0);

            // Earth
            GraphicsObject earthGraphics = graphCon.CreateGraphicsObj(0);

            // Mercury
            GraphicsObject mercuryGraphics = graphCon.CreateGraphicsObj(0);

            // Venus
            GraphicsObject venusGraphics = graphCon.CreateGraphicsObj(0);

            // Mars
            GraphicsObject marsGraphics = graphCon.CreateGraphicsObj(0);
            
            // Jupiter
            GraphicsObject jupiterGraphics = graphCon.CreateGraphicsObj(0);

            // Saturn
            GraphicsObject saturnGraphics = graphCon.CreateGraphicsObj(0);

            /* GFX HANDLING DONE */


            /* HANDLE PHYSICS */
            PhysicObject sunPhys = new PhysicObject();
            sunPhys.mass = 1.98843 * Math.Pow(10, 30);
            sunPhys.radius = 6963420 * 2;
            sunPhys.Velocity = new double[3];
            sunPhys.Acceleration = new double[3];
            
            PhysicObject earthPhys = new PhysicObject();
            earthPhys.mass = 5.97223 * Math.Pow(10, 24);
            earthPhys.radius = 6371000 * 2;
            earthPhys.Velocity = new double[] { 0, 0, 29.78 * 1000 };
            earthPhys.Acceleration = new double[3];

            PhysicObject mercuryPhys = new PhysicObject();
            mercuryPhys.mass = 3.3022 * Math.Pow(10, 23);
            mercuryPhys.radius = 2439000.7*2;
            mercuryPhys.Velocity = new double[] { 0, 0, 47.362 * 1000};
            mercuryPhys.Acceleration = new double[3];

            PhysicObject venusPhys = new PhysicObject();
            venusPhys.mass = 4.8676 * Math.Pow(10, 24);
            venusPhys.radius = 6051000.8 * 2;
            venusPhys.Velocity = new double[] { 0, 0, 35.02 * 1000 };
            venusPhys.Acceleration = new double[3];
            
            PhysicObject marsPhys = new PhysicObject();
            marsPhys.mass = 6.4185 * Math.Pow(10, 23);
            marsPhys.radius = 3389000.5 * 2;
            marsPhys.Velocity = new double[] { 0, 0, 24.077 * 1000 };
            marsPhys.Acceleration = new double[3];

            PhysicObject jupiterPhys = new PhysicObject();
            jupiterPhys.mass = 1.8986 * Math.Pow(10, 27);
            jupiterPhys.radius = 69911000 * 2;
            jupiterPhys.Velocity = new double[] { 0, 0, 13.07 * 1000 };
            jupiterPhys.Acceleration = new double[3];

            PhysicObject saturnPhys = new PhysicObject();
            saturnPhys.mass = 5.6846 * Math.Pow(10, 26);
            saturnPhys.radius = 60268000 * 2;
            saturnPhys.Velocity = new double[] { 0, 0, 9.69 * 1000 };
            saturnPhys.Acceleration = new double[3];

            /* PHYSICS HANDLING DONE*/


            /* PEPARE SIMOBJECTS */
            
            SimObject sunSim = new SimObject(new double[] { 0, 0, 0 }, sunPhys, sunGraphics);
            //SimObject sunSim = new SimObject(new double[] {0, 0, 0}, sunPhys, graphCon.ReadObjFile("sphere.obj"));
            sunSim.Scale = new double[] { sunPhys.radius, sunPhys.radius, sunPhys.radius };

            SimObject earthSim = new SimObject(new double[] { 149598261.0, 0, 0 }, earthPhys, earthGraphics);
            //SimObject earthSim = new SimObject(new double[] { 147098073, 0, 0 }, earthPhys, graphCon.ReadObjFile("sphere.obj"));
            earthSim.Scale = new double[] { earthPhys.radius, earthPhys.radius, earthPhys.radius };

            SimObject mercurySim = new SimObject(new double[] { 57909050.0, 0, 0 }, mercuryPhys, mercuryGraphics);
            //SimObject mercurySim = new SimObject(new double[] { 57909050, 0, 0 }, mercuryPhys, graphCon.ReadObjFile("sphere.obj"));
            mercurySim.Scale = new double[] { mercuryPhys.radius, mercuryPhys.radius, mercuryPhys.radius };

            SimObject venusSim = new SimObject(new double[] { 108208000.0, 0, 0 }, venusPhys, venusGraphics);
            //SimObject venusSim = new SimObject(new double[] { 108208000, 0, 0 }, venusPhys, graphCon.ReadObjFile("sphere.obj"));
            venusSim.Scale = new double[] { venusPhys.radius, venusPhys.radius, venusPhys.radius };

            SimObject marsSim = new SimObject(new double[] { 227939100.0, 0, 0 }, marsPhys, marsGraphics);
            //SimObject marsSim = new SimObject(new double[] { 227939100, 0, 0 }, marsPhys, graphCon.ReadObjFile("sphere.obj"));
            marsSim.Scale = new double[] { marsPhys.radius, marsPhys.radius, marsPhys.radius };

            SimObject jupiterSim = new SimObject(new double[] { 778547200.0, 0, 0 }, jupiterPhys, jupiterGraphics);
            //SimObject marsSim = new SimObject(new double[] { 227939100, 0, 0 }, marsPhys, graphCon.ReadObjFile("sphere.obj"));
            jupiterSim.Scale = new double[] { jupiterPhys.radius, jupiterPhys.radius, jupiterPhys.radius };

            SimObject saturnSim = new SimObject(new double[] { 1433449370.0, 0, 0 }, saturnPhys, saturnGraphics);
            //SimObject marsSim = new SimObject(new double[] { 227939100, 0, 0 }, marsPhys, graphCon.ReadObjFile("sphere.obj"));
            saturnSim.Scale = new double[] { saturnPhys.radius, saturnPhys.radius, saturnPhys.radius };
            
            /* SIMOBJECTS PREPARED */

            /* Setup SimObjList & GraphicsController */
            RenderWindow renderWindow = new RenderWindow(1024, 768, OpenTK.Graphics.GraphicsMode.Default, "Solar Simulation");
            renderWindow.AddDrawObj(sunSim);
            renderWindow.AddDrawObj(earthSim);
            renderWindow.AddDrawObj(mercurySim);
            renderWindow.AddDrawObj(venusSim);
            renderWindow.AddDrawObj(marsSim);
            renderWindow.AddDrawObj(jupiterSim);
            renderWindow.AddDrawObj(saturnSim);
            renderWindow.InitScene();
            renderWindow.Run();
        }
    }
}
