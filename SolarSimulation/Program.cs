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

            /* HANDLE GRAPHICS */
            graphCon.ReadObjFile("sphere.obj");
            graphCon.ReadSkyObjFile("skybox.obj");

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
            PhysicsObject sunPhys = new PhysicsObject();
            sunPhys.Mass = 1.98843 * Math.Pow(10, 30);
            sunPhys.Radius = 6963420;
            sunPhys.Velocity = new double[3];
            sunPhys.Acceleration = new double[3];
            
            PhysicsObject earthPhys = new PhysicsObject();
            earthPhys.Mass = 5.97223 * Math.Pow(10, 24);
            earthPhys.Radius = 637100;
            earthPhys.Velocity = new double[] { 0, 0, 29.78 * 1000 };
            earthPhys.Acceleration = new double[3];

            PhysicsObject mercuryPhys = new PhysicsObject();
            mercuryPhys.Mass = 3.3022 * Math.Pow(10, 23);
            mercuryPhys.Radius = 243900.7;
            mercuryPhys.Velocity = new double[] { 0, 0, 47.362 * 1000};
            mercuryPhys.Acceleration = new double[3];

            PhysicsObject venusPhys = new PhysicsObject();
            venusPhys.Mass = 4.8676 * Math.Pow(10, 24);
            venusPhys.Radius = 605100.8;
            venusPhys.Velocity = new double[] { 0, 0, 35.02 * 1000 };
            venusPhys.Acceleration = new double[3];
            
            PhysicsObject marsPhys = new PhysicsObject();
            marsPhys.Mass = 6.4185 * Math.Pow(10, 23);
            marsPhys.Radius = 338900.5;
            marsPhys.Velocity = new double[] { 0, 0, 24.077 * 1000 };
            marsPhys.Acceleration = new double[3];

            PhysicsObject jupiterPhys = new PhysicsObject();
            jupiterPhys.Mass = 1.8986 * Math.Pow(10, 27);
            jupiterPhys.Radius = 699110;
            jupiterPhys.Velocity = new double[] { 0, 0, 13.07 * 1000 };
            jupiterPhys.Acceleration = new double[3];

            PhysicsObject saturnPhys = new PhysicsObject();
            saturnPhys.Mass = 5.6846 * Math.Pow(10, 26);
            saturnPhys.Radius = 602680;
            saturnPhys.Velocity = new double[] { 0, 0, 9.69 * 1000 };
            saturnPhys.Acceleration = new double[3];

            /* PHYSICS HANDLING DONE*/


            /* PEPARE SIMOBJECTS */
            
            SimObject sunSim = new SimObject(new double[] { 0, 0, 0 }, sunPhys, sunGraphics);
            sunSim.Scale = new double[] { sunPhys.Radius, sunPhys.Radius, sunPhys.Radius };

            SimObject earthSim = new SimObject(new double[] { 149598261.0, 0, 0 }, earthPhys, earthGraphics);
            earthSim.Scale = new double[] { earthPhys.Radius, earthPhys.Radius, earthPhys.Radius };

            SimObject mercurySim = new SimObject(new double[] { 57909050.0, 0, 0 }, mercuryPhys, mercuryGraphics);
            mercurySim.Scale = new double[] { mercuryPhys.Radius, mercuryPhys.Radius, mercuryPhys.Radius };

            SimObject venusSim = new SimObject(new double[] { 108208000.0, 0, 0 }, venusPhys, venusGraphics);
            venusSim.Scale = new double[] { venusPhys.Radius, venusPhys.Radius, venusPhys.Radius };

            SimObject marsSim = new SimObject(new double[] { 227939100.0, 0, 0 }, marsPhys, marsGraphics);
            marsSim.Scale = new double[] { marsPhys.Radius, marsPhys.Radius, marsPhys.Radius };

            SimObject jupiterSim = new SimObject(new double[] { 778547200.0, 0, 0 }, jupiterPhys, jupiterGraphics);
            jupiterSim.Scale = new double[] { jupiterPhys.Radius, jupiterPhys.Radius, jupiterPhys.Radius };

            SimObject saturnSim = new SimObject(new double[] { 1433449370.0, 0, 0 }, saturnPhys, saturnGraphics);
            saturnSim.Scale = new double[] { saturnPhys.Radius, saturnPhys.Radius, saturnPhys.Radius };
            
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
