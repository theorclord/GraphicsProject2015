using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarSimulation.Graphics
{
	interface IGraphicsController
	{
		void Draw(List<SimObject> simObjects);
		GraphicsObject ReadObjFile(String fileName);
	}
}