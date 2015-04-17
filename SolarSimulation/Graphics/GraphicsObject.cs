using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation.Graphics
{
	struct GraphicsObject
	{
		List<Vertex> vertices = new List<Vertex>();
		List<Vertex> normals = new List<Vertex>();
		List<Triangle> triangles = new List<Triangle>();
	}
}