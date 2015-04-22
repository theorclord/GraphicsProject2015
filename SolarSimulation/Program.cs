using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SolarSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Setup SimObjList & GraphicsController */
            Graphics.GraphicsController graphCon = new Graphics.GraphicsController();
            SimObject newSim = new SimObject();
            newSim.GraphicsObj = graphCon.ReadObjFile("sphere.obj");

            RenderWindow renderWindow = new RenderWindow(400, 400, OpenTK.Graphics.GraphicsMode.Default, "Solar Simulation");
            renderWindow.AddDrawObj(newSim);
            renderWindow.InitScene();
            renderWindow.Run();
        }
    }
}
