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
		//public List<Triangle> triangles;
        public int ShapeIndex;
        public float[] Color;

//      public GraphicsObject(List<Vertex> vertices, List<Vertex> normals, List<Triangle> triangles, double[] color)
        public GraphicsObject(List<Vertex> vertices, List<Vertex> normals, int shapeIndex, float[] color)
        {
            this.Color = color;
            if (this.Color.Length < 10)
            {
                float[] tmp = new float[10];
                for (int i = 0; i < this.Color.Length; i++ )
                { tmp[i] = this.Color.Length; }
                for (int i = color.Length - 1; i < tmp.Length; i++)
                { tmp[i] = 0; }
                this.Color = tmp;
            }

            this.vertices = vertices;
            this.normals = normals;
            //this.triangles = triangles;
            this.ShapeIndex = shapeIndex;
        }
	}
}