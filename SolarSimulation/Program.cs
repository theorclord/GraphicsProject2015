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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double seconds = stopwatch.ElapsedMilliseconds / 1000.0;
            Physics physics = new Physics();
            physics.Update(new List<SimObject>(), seconds);
        }
    }
}
