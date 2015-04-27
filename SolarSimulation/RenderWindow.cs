using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SolarSimulation.Graphics;

namespace SolarSimulation
{
    class RenderWindow : GameWindow
    {
        List<SimObject> drawObjList = new List<SimObject>();
        Physics physController;
        
        float[] light_position = { 10.0f, 40.0f, 20.0f, 1.0f };
        float[] light_ambient = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] material_ambient = { 0.0f, 0.0f, 0.2f, 1.0f };
        float[] material_diffuse = { 1.0f, 0.0f, 0.0f, 1.0f };
        float[] material_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] material_shininess = { 50.0f };
        float[] eye = { 0.0f, 4.0f, 30.0f };

        public RenderWindow(int width, int height, OpenTK.Graphics.GraphicsMode mode, string title) : base(width, height, mode, title)
        {
            physController = new Physics();
        }

        public void AddDrawObj(SimObject sObj)
        {

            Matrix4 transMat = new Matrix4(1.0f, 0.0f, 0.0f, (float)sObj.Position[0],
                                           0.0f, 1.0f, 0.0f, (float)sObj.Position[1],
                                           0.0f, 0.0f, 1.0f, (float)sObj.Position[2],
                                           0.0f, 0.0f, 0.0f, 1.0f);
            Matrix4 scaleMat = new Matrix4((float)sObj.Scale[0], 0.0f, 0.0f, 0.0f,
                                           0.0f, (float)sObj.Scale[1], 0.0f, 0.0f,
                                           0.0f, 0.0f, (float)sObj.Scale[2], 0.0f,
                                           0.0f, 0.0f, 0.0f, 1.0f);
            scaleMat.Transpose();
            transMat = Matrix4.Mult(transMat, scaleMat);
            transMat.Transpose();
            TransformObj(sObj.GraphicsObj, transMat);
            drawObjList.Add(sObj); 
        }

        public void AddDrawObj(List<SimObject> sObjList)
        {
            foreach (var item in sObjList)
            { AddDrawObj(item); }
        }

        private void TransformObj(GraphicsObject gObj, Matrix4 transMatrix)
        {
            for (int i = 0; i < gObj.vertices.Count; i++)
            { gObj.vertices[i] = Dehomogenize(Vector4.Transform(Homogenize(gObj.vertices[i]), transMatrix)); }

            for (int i = 0; i < gObj.normals.Count; i++)
            { gObj.normals[i] = Dehomogenize(Vector4.Transform(Homogenize(gObj.normals[i]), transMatrix)); }
        }

        private void TransformObjs(List<Matrix4> transMatrix)
        {
            for (int simIndex = 0; simIndex < drawObjList.Count; simIndex++)
            {
                Graphics.GraphicsObject gObj = drawObjList[simIndex].GraphicsObj;
                Matrix4 curTransMat = transMatrix[simIndex];
                TransformObj(gObj, curTransMat);
            }
        }

        public Vector4 Homogenize(Vertex v)
        { return new Vector4((float)v.x, (float)v.y, (float)v.z, 1.0f); }

        public Vertex Dehomogenize(Vector4 v)
        { return new Vertex(v.X, v.Y, v.Z); }


        /* 
         * Most code inspired by code found on
         * OpenTK documentation:
         * www.opentk.com/node/3181
         * 
         * And on learnit, specifically Topic4
         */
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            TransformObjs(physController.Update(drawObjList, e.Time));
            DrawObjs();
            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        public void InitScene()
        {
            // Setup light strength & position.
            GL.Light(LightName.Light0, LightParameter.Position, light_position);
            GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, light_diffuse);
            GL.Light(LightName.Light0, LightParameter.Specular, light_specular);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.Lighting);
            
            // Setup material reflection
            GL.Material(MaterialFace.Front, MaterialParameter.Ambient, material_ambient);
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, material_diffuse);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, material_specular);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, material_shininess);

            // Enable depthtest for backface culling / hidden surface elimination
            GL.Enable(EnableCap.DepthTest);


            // Setup viewport and perspective.
            Matrix4 persMat, eyeMat;

            GL.MatrixMode(MatrixMode.Projection);
            float persAspect = (float)ClientRectangle.Width / (float)ClientRectangle.Height;
            //persMat = OpenTK.Matrix4.CreatePerspectiveOffCenter(-2f, 2f, -2f, 2f, 1, 1000);
            persMat = OpenTK.Matrix4.CreatePerspectiveFieldOfView(degs2rads(60.0f), persAspect, 1.0f, 1000.0f);
            //persMat = OpenTK.Matrix4.CreatePerspectiveFieldOfView(degs2rads(40.0f), persAspect, 1.0f, distanceToEarth + earthRad + 100);
            //OpenTK.Matrix4.Transpose(ref persMat, out persMat);
            GL.LoadMatrix(ref persMat);

            GL.MatrixMode(MatrixMode.Modelview);
            // Eye, Target, Up-direction
            eyeMat = OpenTK.Matrix4.LookAt(
                eye[0], eye[1], eye[2],
                0.0f, 10.0f, 8.0f,
                0.0f, 1.0f, 0.0f
                );
            OpenTK.Matrix4.Transpose(ref eyeMat, out eyeMat);
            //eyeMat.Transpose();
            GL.MultMatrix(ref eyeMat);

            GL.Translate(new Vector3(0.0f, 0.0f, -7.0f));
        }

        private float degs2rads(float degrees)
        { return (float)(degrees * Math.PI/180); }

        /* 
         * FOLLOWING CODE HAS BEEN DIRECTLY ADDED FROM
         * TOPIC4, WHICH WAS FOUND ON LEARNIT!
         * Only very minor changes has been made to adjust
         * for a change in programming language.
         */
        private void DrawObjs()
        {
            Vertex v1, v2, v3;
            Vertex n1, n2, n3;
            foreach (SimObject curSimObj in drawObjList)
            {
                Graphics.GraphicsObject curGraphObj = curSimObj.GraphicsObj;
                for (int i = 0; i < curGraphObj.triangles.Count; i++)
                {
                    v1 = curGraphObj.vertices[curGraphObj.triangles[i].vi1];
                    v2 = curGraphObj.vertices[curGraphObj.triangles[i].vi2];
                    v3 = curGraphObj.vertices[curGraphObj.triangles[i].vi3];

                    n1 = curGraphObj.vertices[curGraphObj.triangles[i].ni1];
                    n2 = curGraphObj.vertices[curGraphObj.triangles[i].ni2];
                    n3 = curGraphObj.vertices[curGraphObj.triangles[i].ni3];

                    GL.Begin(PrimitiveType.Triangles);

                    ShadeVertex(v1, n1);
                    GL.Normal3(n1.x, n1.y, n1.z);
                    GL.Vertex3(v1.x, v1.y, v1.z);

                    ShadeVertex(v2, n2);
                    GL.Normal3(n2.x, n2.y, n2.z);
                    GL.Vertex3(v2.x, v2.y, v2.z);

                    ShadeVertex(v3, n3);
                    GL.Normal3(n3.x, n3.y, n3.z);
                    GL.Vertex3(v3.x, v3.y, v3.z);

                    GL.End();
                }
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
            //result_r += Ks_r * Math.Pow(Nonnegative_Dot(l_reflect, v_to_eye), Kexp);
            //result_g += Ks_g * Math.Pow(Nonnegative_Dot(l_reflect, v_to_eye), Kexp);
            //result_b += Ks_b * Math.Pow(Nonnegative_Dot(l_reflect, v_to_eye), Kexp);
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
