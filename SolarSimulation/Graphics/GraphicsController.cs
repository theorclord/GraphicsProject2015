using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolarSimulation;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SolarSimulation.Graphics
{
	class GraphicsController : IGraphicsController
	{
		public void Draw(List<SimObject> simObjects)
		{
			foreach (SimObject sObj in simObjects)
			{ DrawObj(sObj); }
		}
		
		private void DrawObj(SimObject simObject)
		{
			Vertex v1, v2, v3;
			Vertex n1, n2, n3;
			
			GraphicsObject curGraphObj = simObject.GraphicsObj;
			
			for (int i = 0; i < curGraphObj.triangles.Count; i++)
			{
				v1 = curGraphObj.vertices[curGraphObj.triangles[i].vi1];
                v2 = curGraphObj.vertices[curGraphObj.triangles[i].vi2];
                v3 = curGraphObj.vertices[curGraphObj.triangles[i].vi3];

                n1 = curGraphObj.vertices[curGraphObj.triangles[i].ni1];
                n2 = curGraphObj.vertices[curGraphObj.triangles[i].ni2];
                n3 = curGraphObj.vertices[curGraphObj.triangles[i].ni3];
				
				GL.Begin(PrimitiveType.Triangles);
				
				GL.Normal3(n1.x, n1.y, n1.z);
				GL.Vertex3(v1.x, v1.y, v1.z);
				
				GL.Normal3(n2.x, n2.y, n2.z);
				GL.Vertex3(v2.x, v2.y, v2.z);
				
				GL.Normal3(n3.x, n3.y, n3.z);
				GL.Vertex3(v3.x, v3.y, v3.z);
				
				GL.End();
			}
		}
		
		private Vertex TranslateVertex(Vertex v, double[] coords)
		{
			v.x += v.x - coords[0];
			v.y += v.y - coords[1];
			v.z += v.z - coords[2];

            return v;
		}
	}
}