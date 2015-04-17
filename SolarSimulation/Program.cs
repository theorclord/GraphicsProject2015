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
            
 
            foreach(DisplayIndex index in Enum.GetValues(typeof(DisplayIndex)))
            {
                DisplayDevice device = DisplayDevice.GetDisplay(index);
                if (device == null)
                {
                    continue;
                }
 
                Console.WriteLine(device.IsPrimary);
                Console.WriteLine(device.Bounds);
                Console.WriteLine(device.RefreshRate);
                Console.WriteLine(device.BitsPerPixel);
 
                foreach (DisplayResolution res in device.AvailableResolutions)
                {
                    Console.WriteLine(res);
                }
            }
        }
    }
}
