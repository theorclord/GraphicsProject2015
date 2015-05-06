using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Diagnostics;
using SolarSimulation.Graphics;

namespace SolarSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            GraphicsController graphCon = new GraphicsController();
            List<SimObject> simulatedObjects = new List<SimObject>();
            List<Matrix4> transList = new List<Matrix4>();
            Physics physics = new Physics();
            float[] curColor = new float[10];

            /* HANDLE GRAPHICS */

            // The Sun
            curColor = new float[] {0.5f, 0.5f, 0, 1, 1, 0, 1, 1, 0, 10};
            GraphicsObject sunGraphics = graphCon.CreateGraphicsObj(0, curColor);

            // Earth
            curColor = new float[] {0, 0.1f, 0.2f, 0, 0, 1, 0, 0.5f, 1, 10};
            GraphicsObject earthGraphics = graphCon.CreateGraphicsObj(0, curColor);

            // Mercury
            curColor = new float[] { 0.2f, 0.2f, 0.2f, 0.4f, 0.4f, 0.4f, 0.8f, 0.8f, 0.8f, 10 };
            GraphicsObject mercuryGraphics = graphCon.CreateGraphicsObj(0, curColor);

            // Venus
            curColor = new float[] { 0.2f, 0.2f, 0.18f, 0.5f, 0.5f, 0.44f, 0.96f, 0.96f, 0.86f, 10 };
            GraphicsObject venusGraphics = graphCon.CreateGraphicsObj(0, curColor);

            // Mars
            curColor = new float[] { 0.2f, 0, 0, 0.4f, 0.1f, 0.1f, 1, 0.65f, 0.2f, 10 };
            GraphicsObject marsGraphics = graphCon.CreateGraphicsObj(0, curColor);
            
            /* GFX HANDLING DONE */


            /* HANDLE PHYSICS */
            PhysicObject sunPhys = new PhysicObject();
            sunPhys.mass = 1.98843 * Math.Pow(10, 30);
            sunPhys.radius = 696342;
            sunPhys.Velocity = new double[3];
            sunPhys.Acceleration = new double[3];
            
            PhysicObject earthPhys = new PhysicObject();
            earthPhys.mass = 5.97223 * Math.Pow(10, 24);
            earthPhys.radius = 637100;
            earthPhys.Velocity = new double[] { 0, 0,0};//-107219 / 3600, 0 };
            earthPhys.Acceleration = new double[3];

            PhysicObject mercuryPhys = new PhysicObject();
            mercuryPhys.mass = 3.3022 * Math.Pow(10, 23);
            mercuryPhys.radius = 243900.7;
            mercuryPhys.Velocity = new double[] { 0, 0, 0 };//47362 / 3600, 0 };
            mercuryPhys.Acceleration = new double[3];

            PhysicObject venusPhys = new PhysicObject();
            venusPhys.mass = 4.8676 * Math.Pow(10, 24);
            venusPhys.radius = 605100.8;
            venusPhys.Velocity = new double[] { 0, 0,0};//35.02, 0 };
            venusPhys.Acceleration = new double[3];
            
            PhysicObject marsPhys = new PhysicObject();
            marsPhys.mass = 6.4185 * Math.Pow(10, 23);
            marsPhys.radius = 338900.5;
            marsPhys.Velocity = new double[] { 0, 0, 0 };//24.077, 0 };
            marsPhys.Acceleration = new double[3];

            /* PHYSICS HANDLING DONE*/


            /* PEPARE SIMOBJECTS */
            
            SimObject sunSim = new SimObject(new double[] { 0, 0, 0 }, sunPhys, sunGraphics);
            //SimObject sunSim = new SimObject(new double[] {0, 0, 0}, sunPhys, graphCon.ReadObjFile("sphere.obj"));
            sunSim.Scale = new double[] { sunPhys.radius, sunPhys.radius, sunPhys.radius };

            SimObject earthSim = new SimObject(new double[] { 147098073, 0, 0 }, earthPhys, earthGraphics);
            //SimObject earthSim = new SimObject(new double[] { 147098073, 0, 0 }, earthPhys, graphCon.ReadObjFile("sphere.obj"));
            earthSim.Scale = new double[] { earthPhys.radius, earthPhys.radius, earthPhys.radius };

            SimObject mercurySim = new SimObject(new double[] { 57909050, 0, 0 }, mercuryPhys, mercuryGraphics);
            //SimObject mercurySim = new SimObject(new double[] { 57909050, 0, 0 }, mercuryPhys, graphCon.ReadObjFile("sphere.obj"));
            mercurySim.Scale = new double[] { mercuryPhys.radius, mercuryPhys.radius, mercuryPhys.radius };

            SimObject venusSim = new SimObject(new double[] { 10820800, 0, 0 }, venusPhys, venusGraphics);
            //SimObject venusSim = new SimObject(new double[] { 108208000, 0, 0 }, venusPhys, graphCon.ReadObjFile("sphere.obj"));
            venusSim.Scale = new double[] { venusPhys.radius, venusPhys.radius, venusPhys.radius };

            SimObject marsSim = new SimObject(new double[] { 227939100, 0, 0 }, marsPhys, marsGraphics);
            //SimObject marsSim = new SimObject(new double[] { 227939100, 0, 0 }, marsPhys, graphCon.ReadObjFile("sphere.obj"));
            marsSim.Scale = new double[] { marsPhys.radius, marsPhys.radius, marsPhys.radius };
            
            /* SIMOBJECTS PREPARED */

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
    }
}
