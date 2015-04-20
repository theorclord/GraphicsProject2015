using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation.Graphics
{
	public struct GraphicsObject
	{
		public List<Vertex> vertices;
		public List<Vertex> normals;
		public List<Triangle> triangles;

        public GraphicsObject(List<Vertex> vertices, List<Vertex> normals, List<Triangle> triangles)
        {
            this.vertices = vertices;
            this.normals = normals;
            this.triangles = triangles;
        }
	}
}