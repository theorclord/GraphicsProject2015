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
        float[] light_position = {10.0f, 40.0f, 20.0f, 1.0f};
        float[] light_ambient = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] material_ambient = { 0.0f, 0.0f, 0.2f, 1.0f };
        float[] material_diffuse = { 1.0f, 0.0f, 0.0f, 1.0f };
        float[] material_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] material_shininess = { 50.0f };
        float[] eye = { 0.0f, 0.0f, 5.0f };
		
        public void Draw(List<SimObject> simObjects)
		{
			foreach (SimObject sObj in simObjects)
			{ DrawObj(sObj); }
		}

        private Vertex TranslateVertex(Vertex v, double[] coords)
        {
            v.x += v.x - coords[0];
            v.y += v.y - coords[1];
            v.z += v.z - coords[2];

            return v;
        }

        /* 
         * FOLLOWING CODE HAS BEEN DIRECTLY ADDED FROM
         * TOPIC4, WHICH WAS FOUND ON LEARNIT!
         * Only very minor changes has been made to adjust
         * for a change in programming language.
         */
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

        private void ShadeVertex(Vertex v, Vertex n)
        {
            double result_r, result_g, result_b;
            Vertex l = new Vertex { x = light_position[0], y = light_position[1], z = light_position[2] };
            float Kd_r = material_diffuse[0];
            float Kd_g = material_diffuse[1];
            float Kd_b = material_diffuse[2];
            float Ka_r = material_ambient[0];
            float Ka_g = material_ambient[1];
            float Ka_b = material_ambient[2];
            float Ks_r = material_specular[0];
            float Ks_g = material_specular[1];
            float Ks_b = material_specular[2];
            float Kexp = material_shininess[0] / 5.0f; // HACK! Not sure why, but required to approximate OpenGL results.
            Vertex v_to_l = new Vertex { x = l.x - v.x, y = l.y - v.y, z = l.z - v.z };
            Normalize(v_to_l);
            Vertex l_reflect = Reflect(v_to_l, n);
            Vertex v_to_eye = new Vertex { x = eye[0] - v.x, y = eye[1] - v.y, z = eye[2] - v.z };
            Normalize(v_to_eye);
            
            /**/
            // ACCUMULATE SPECULAR SHADING COLOR:
            // SET TO ZERO TO START.
            result_r = 0.0;
            result_g = 0.0;
            result_b = 0.0;

            // ADD AMBIENT COMPONENT.
            result_r += Ka_r;
            result_g += Ka_g;
            result_b += Ka_b;

            // ADD DIFFUSE COMPONENT.  (Try using nonnegative_dot().)
            result_r += Kd_r * Nonnegative_Dot(v_to_l, n);
            result_g += Kd_g * Nonnegative_Dot(v_to_l, n);
            result_b += Kd_b * Nonnegative_Dot(v_to_l, n);

            // ADD SPECULAR COMPONENT.  (Try using nonnegative_dot() and pow().)
            result_r += Ks_r * Math.Pow(Nonnegative_Dot(l_reflect, v_to_eye), Kexp);
            result_g += Ks_g * Math.Pow(Nonnegative_Dot(l_reflect, v_to_eye), Kexp);
            result_b += Ks_b * Math.Pow(Nonnegative_Dot(l_reflect, v_to_eye), Kexp);
            /**/

            // Apply final lighting result to vertex.
            GL.Color3(result_r, result_g, result_b);
        }

        private void Normalize(Vertex v)
        {
            double length = Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
            v.x /= length;
            v.y /= length;
            v.z /= length;
        }
        Vertex Reflect(Vertex v, Vertex n)
        {
            Vertex v_on_n = new Vertex { x = Dot(v, n) * n.x, y = Dot(v, n) * n.y, z = Dot(v, n) * n.z };
            Vertex result = new Vertex { x = 2.0 * v_on_n.x - v.x, y = 2.0 * v_on_n.y - v.y, z = 2.0 * v_on_n.z - v.z };

            return result;
        }

        double Dot(Vertex v, Vertex n)
        { return v.x * n.x + v.y * n.y + v.z * n.z; }

        double Nonnegative_Dot(Vertex v, Vertex n)
        {
            double result = Dot(v, n);

            if (result < 0.0) return 0.0;
            else return result; 
        }
	}
}