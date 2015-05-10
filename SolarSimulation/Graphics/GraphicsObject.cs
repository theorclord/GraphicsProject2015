using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSimulation.Graphics
{
	public class GraphicsObject
	{
		public List<Vertex> vertices;
		public List<Vertex> normals;
        
        public int ShapeIndex;

        public GraphicsObject(List<Vertex> vertices, List<Vertex> normals, int shapeIndex)
        {
            this.vertices = vertices;
            this.normals = normals;
            this.ShapeIndex = shapeIndex;
        }

	}
}