using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolarSimulation_v2.Graphics;
using SolarSimulation_v2.Physics;

namespace SolarSimulation_v2
{
    class Program
    {
        static void Main(string[] args)
        {
            /* HANDLE PHYSICS */
            PhysicsObject sunPhys = new PhysicsObject();
            sunPhys.Mass = 1.98843 * Math.Pow(10, 30);
            sunPhys.Radius = 696342;
            sunPhys.Velocity = new double[3];
            sunPhys.Acceleration = new double[3];

            PhysicsObject mercuryPhys = new PhysicsObject();
            mercuryPhys.Mass = 3.3022 * Math.Pow(10, 23);
            mercuryPhys.Radius = 2439.7;
            mercuryPhys.Velocity = new double[] { 0, 0, 47.362 * 1000 };
            mercuryPhys.Acceleration = new double[3];

            PhysicsObject venusPhys = new PhysicsObject();
            venusPhys.Mass = 4.8676 * Math.Pow(10, 24);
            venusPhys.Radius = 6051.8;
            venusPhys.Velocity = new double[] { 0, 0, 35.02 * 1000 };
            venusPhys.Acceleration = new double[3];

            PhysicsObject earthPhys = new PhysicsObject();
            earthPhys.Mass = 5.97223 * Math.Pow(10, 24);
            earthPhys.Radius = 6371;
            earthPhys.Velocity = new double[] { 0, 0, 29.78 * 1000 };
            earthPhys.Acceleration = new double[3];

            /*
            PhysicsObject moonPhys = new PhysicsObject();
            moonPhys.Mass = 7.3477 * Math.Pow(10, 22);
            moonPhys.Radius = 173700;
            moonPhys.Velocity = new double[] { 0, 0, 1.022 * 1000};
            moonPhys.Acceleration = new double[3];
            */

            PhysicsObject marsPhys = new PhysicsObject();
            marsPhys.Mass = 6.4185 * Math.Pow(10, 23);
            marsPhys.Radius = 3389.5;
            marsPhys.Velocity = new double[] { 0, 0, 24.077 * 1000 };
            marsPhys.Acceleration = new double[3];

            PhysicsObject jupiterPhys = new PhysicsObject();
            jupiterPhys.Mass = 1.8986 * Math.Pow(10, 27);
            jupiterPhys.Radius = 69911;
            jupiterPhys.Velocity = new double[] { 0, 0, 13.07 * 1000 };
            jupiterPhys.Acceleration = new double[3];

            PhysicsObject saturnPhys = new PhysicsObject();
            saturnPhys.Mass = 5.6846 * Math.Pow(10, 26);
            saturnPhys.Radius = 60268;
            saturnPhys.Velocity = new double[] { 0, 0, 9.69 * 1000 };
            saturnPhys.Acceleration = new double[3];

            PhysicsObject uranusPhys = new PhysicsObject();
            uranusPhys.Mass = 8.6810 * Math.Pow(10, 25);
            uranusPhys.Radius = 25362;
            uranusPhys.Velocity = new double[] { 0, 0, 6.8 * 1000 };
            uranusPhys.Acceleration = new double[3];

            PhysicsObject neptunePhys = new PhysicsObject();
            neptunePhys.Mass = 1.0243 * Math.Pow(10, 26);
            neptunePhys.Radius = 24622;
            neptunePhys.Velocity = new double[] { 0, 0, 5.43 * 1000 };
            neptunePhys.Acceleration = new double[3];
            /* -------------- */

            /* NEW SIMOBJECT PREPARATION */
            GraphicsObject sunGraph = new GraphicsObject(0, new double[] { sunPhys.Radius, sunPhys.Radius, sunPhys.Radius });
            GraphicsObject mercuryGraph = new GraphicsObject(0, new double[] { mercuryPhys.Radius, mercuryPhys.Radius, mercuryPhys.Radius });
            GraphicsObject venusGraph = new GraphicsObject(0, new double[] { venusPhys.Radius, venusPhys.Radius, venusPhys.Radius });
            
            GraphicsObject earthGraph = new GraphicsObject(0, new double[] { earthPhys.Radius, earthPhys.Radius, earthPhys.Radius });
            //GraphicsObject moonGraph = new GraphicsObject(0, new double[] { moonPhys.Radius, moonPhys.Radius, moonPhys.Radius });
            
            GraphicsObject marsGraph = new GraphicsObject(0, new double[] { marsPhys.Radius, marsPhys.Radius, marsPhys.Radius });
            GraphicsObject jupiterGraph = new GraphicsObject(0, new double[] { jupiterPhys.Radius, jupiterPhys.Radius, jupiterPhys.Radius });
            GraphicsObject saturnGraph = new GraphicsObject(0, new double[] { saturnPhys.Radius, saturnPhys.Radius, saturnPhys.Radius });
            GraphicsObject uranusGraph = new GraphicsObject(0, new double[] { uranusPhys.Radius, uranusPhys.Radius, uranusPhys.Radius });
            GraphicsObject neptuneGraph = new GraphicsObject(0, new double[] { neptunePhys.Radius, neptunePhys.Radius, neptunePhys.Radius });

            SimObject sunSim = new SimObject(new double[3], sunPhys, sunGraph);
            
            double mercuryDist = 57909050.0;
            double[] mercuryIncl = new double[] { mercuryDist * Math.Cos(3.38 * Math.PI / 180), mercuryDist * Math.Sin(3.38 * Math.PI / 180) };
            SimObject mercurySim = new SimObject(new double[] { mercuryIncl[0], mercuryIncl[1], 0 }, mercuryPhys, mercuryGraph);
            
            double venusDist = 108208000.0;
            double[] venusIncl = new double[] { venusDist * Math.Cos(3.86 * Math.PI / 180), venusDist * Math.Sin(3.86 * Math.PI / 180) };
            SimObject venusSim = new SimObject(new double[] { venusIncl[0], venusIncl[1], 0 }, venusPhys, venusGraph);

            double earthDist = 149598261.0;
            double[] earthIncl = new double[] { earthDist * Math.Cos(7.155 * Math.PI / 180), earthDist * Math.Sin(7.155 * Math.PI / 180) };
            SimObject earthSim = new SimObject(new double[] { earthIncl[0], earthIncl[1], 0 }, earthPhys, earthGraph);

            /*
            double moonDist = earthDist + 384399;
            double[] moonIncl = new double[] { moonDist * Math.Cos(5.145 * Math.PI / 180), moonDist * Math.Sin(5.145 * Math.PI / 180) };
            SimObject moonSim = new SimObject(new double[] { moonIncl[0], moonIncl[1], 0 }, moonPhys, moonGraph);
            */

            double marsDist = 227939100.0;
            double[] marsIncl = new double[] { marsDist * Math.Cos(5.65 * Math.PI / 180), marsDist * Math.Sin(5.65 * Math.PI / 180) };
            SimObject marsSim = new SimObject(new double[] { marsIncl[0], marsIncl[1], 0 }, marsPhys, marsGraph);

            double jupiterDist = 778547200.0;
            double[] jupiterIncl = new double[] { jupiterDist * Math.Cos(6.09 * Math.PI / 180), jupiterDist * Math.Sin(6.09 * Math.PI / 180) };
            SimObject jupiterSim = new SimObject(new double[] { jupiterIncl[0], jupiterIncl[1], 0 }, jupiterPhys, jupiterGraph);

            double saturnDist = 1433449370.0;
            double[] saturnIncl = new double[] { saturnDist * Math.Cos(5.51 * Math.PI / 180), saturnDist * Math.Sin(5.51 * Math.PI / 180) };
            SimObject saturnSim = new SimObject(new double[] { saturnIncl[0], saturnIncl[1], 0 }, saturnPhys, saturnGraph);

            double uranusDist = 2870671400.0;
            double[] uranusIncl = new double[] { uranusDist * Math.Cos(6.48 * Math.PI / 180), uranusDist * Math.Sin(6.48 * Math.PI / 180) };
            SimObject uranusSim = new SimObject(new double[] { uranusIncl[0], uranusIncl[1], 0 }, uranusPhys, uranusGraph);

            double neptuneDist = 4498542600.0;
            double[] neptuneIncl = new double[] { neptuneDist * Math.Cos(6.43 * Math.PI / 180), neptuneDist * Math.Sin(6.43 * Math.PI / 180) };
            SimObject neptuneSim = new SimObject(new double[] { neptuneIncl[0], neptuneIncl[1], 0 }, neptunePhys, neptuneGraph);
            /* ------------------------- */

            /* Setup SimObjList & GraphicsController */
            RenderWindow renderWindow = new RenderWindow(1024, 768, OpenTK.Graphics.GraphicsMode.Default, "Solar Simulation V2");
            renderWindow.AddDrawObj(sunSim);
            renderWindow.AddDrawObj(mercurySim);
            renderWindow.AddDrawObj(venusSim);
            renderWindow.AddDrawObj(earthSim);
            renderWindow.AddDrawObj(marsSim);
            renderWindow.AddDrawObj(jupiterSim);
            renderWindow.AddDrawObj(saturnSim);
            renderWindow.AddDrawObj(uranusSim);
            renderWindow.AddDrawObj(neptuneSim);

            //renderWindow.AddDrawObj(moonSim);

            renderWindow.InitScene();
            renderWindow.Run();
        }
    }
}
