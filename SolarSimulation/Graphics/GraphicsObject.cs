using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation.Graphics
{
	struct GraphicsObject
	{
		public List<Vertex> vertices = new List<Vertex>();
		public List<Vertex> normals = new List<Vertex>();
		public List<Triangle> triangles = new List<Triangle>();
	}
}