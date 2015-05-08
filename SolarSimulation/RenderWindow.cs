﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SolarSimulation.Graphics;
using SolarSimulation.Physics;

namespace SolarSimulation
{
    class RenderWindow : GameWindow
    {
        List<SimObject> drawObjList = new List<SimObject>();
        Graphics.GraphicsObject skyBox;

        //private int cometCount = 0;
        //private Dictionary<int,SimObject> comets= new Dictionary<int,SimObject>();

        PhysicsController physController;
        CameraController camController;
        GraphicsController graphController;

        float[] light_position = { -2, 0, 0 }; //{ 10000.0f, 10000000.0f, 2000.0f, 1.0f };
        float[] light_ambient = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] material_ambient = { 0.8f, 0.8f, 0.8f, 1.0f };
        float[] material_diffuse = { 0.5f, 0.5f, 0.5f, 1.0f };
        float[] material_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] material_shininess = { 50.0f };
        float[] eye = { 0.0f, 4.0f, 30.0f };

        public int[] textureIds;

        /*
         * Textures found at: 
         * http://www.solarsystemscope.com/nexus/textures/planet_textures/
         */
        public RenderWindow(int width, int height, OpenTK.Graphics.GraphicsMode mode, string title) : base(width, height, mode, title)
        {
            physController = new PhysicsController();
            camController = new CameraController(new double[] { 0, 0.0f, -10000000 });//new double[] { -1000000.0f, 0.0f, -1000000.0f });
            graphController = new GraphicsController();
            graphController.ReadObjFile("sphere.obj");
            graphController.ReadSkyObjFile("skybox.obj");

            int[] newTextureIds = new int[7];
            newTextureIds[0] = graphController.LoadTexture("Textures/texture_sun.jpg");
            newTextureIds[1] = graphController.LoadTexture("Textures/texture_earth_clouds.jpg");
            newTextureIds[2] = graphController.LoadTexture("Textures/texture_mercury.jpg");
            newTextureIds[3] = graphController.LoadTexture("Textures/texture_venus_surface.jpg");
            newTextureIds[4] = graphController.LoadTexture("Textures/texture_mars.jpg");
            newTextureIds[5] = graphController.LoadTexture("Textures/texture_moon.jpg");
            newTextureIds[6] = graphController.LoadTexture("Textures/texture_skybox.jpg");
            textureIds = newTextureIds;

            // Skybox
            /*
            skyBox = graphController.CreateSkyboxObj(1);
            var skyMat = new Matrix4(
                (float)Math.Pow(10, 12), 0, 0, (float)Math.Pow(10, 10) / 2,
                0, (float)Math.Pow(10, 12), 0, (float)Math.Pow(10, 10) / 2,
                0, 0, (float)Math.Pow(10, 12), (float)Math.Pow(10, 10) / 2,
                0, 0, 0, 1
                );
            TransformObj(skyBox, skyMat);
             */
        }

        public void AddDrawObj(SimObject sObj)
        {
            // Create and apply initial position and scale matrices.
            Matrix4 transMat = Matrix4.CreateTranslation(
                new Vector3((float)sObj.Position[0], (float)sObj.Position[1], (float)sObj.Position[2])
                );
            Matrix4 scaleMat = Matrix4.CreateScale(
                new Vector3((float)sObj.Scale[0], (float)sObj.Scale[1], (float)sObj.Scale[2])
                );


            transMat.Row0 = scaleMat.Row0;
            transMat.Row1 = scaleMat.Row1;
            transMat.Row2 = scaleMat.Row2;
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
            Vertex v, n;
            for (int i = 0; i < gObj.vertices.Count; i++)
            {
                v = gObj.vertices[i];
                graphController.Dehomogenize(Vector4.Transform(graphController.Homogenize(gObj.vertices[i]), transMatrix), ref v);
                gObj.vertices[i] = v;
            }

            for (int i = 0; i < gObj.normals.Count; i++)
            {
                n = gObj.normals[i];
                graphController.Dehomogenize(Vector4.Transform(graphController.Homogenize(gObj.normals[i]), transMatrix), ref n);
                Normalize(n);
                gObj.normals[i] = n;
            }
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

        /* 
         * Most code inspired by code found on
         * OpenTK documentation:
         * www.opentk.com/node/3181
         * 
         * And on learnit, specifically Topic4
         */
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            double timeSinceLastFrame = e.Time;
            //Spawn comets
            if (OpenTK.Input.Keyboard.GetState().IsKeyDown( OpenTK.Input.Key.Space))
            {
                PhysicObject physObj = new PhysicObject();
                physObj.mass = 1000 * Math.Pow(10, 16);
                physObj.radius = 200000;
                physObj.Velocity = new double[] { 1.0, 1.0, 1.0 };
                physObj.Acceleration = new double[3];

                var spawnPos = new double[] {-camController.Position[0], -camController.Position[1], -camController.Position[2]};
                spawnPos[2] += 25000;
                SimObject simObj = new SimObject(
                    spawnPos, physObj, graphController.CreateGraphicsObj(0));
                simObj.Scale = new double[] { physObj.radius, physObj.radius, physObj.radius };
                //comets.Add(drawObjList.Count, simObj);
                AddDrawObj(simObj);
            }

            // Hold T to accelerate to 1 day / frame
            if (OpenTK.Input.Keyboard.GetState().IsKeyDown( OpenTK.Input.Key.T))
            { timeSinceLastFrame *= 86400; }

            // Hold T + Y tp accelerate to 10 days / frame.
            // Y calculates only 10 seconds on it's own.
            if (OpenTK.Input.Keyboard.GetState().IsKeyDown(OpenTK.Input.Key.Y))
            { timeSinceLastFrame *= 10; }
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            List<Matrix4> transMats = physController.Update(drawObjList, timeSinceLastFrame);
            TransformObjs(transMats);

            //TransformObjs(physController.Update(drawObjList, e.Time));
            Matrix4 transMat = camController.Update(e.Time);
            //transMat = Matrix4.Mult(transMat, transMats[4]);
            Matrix4 camMat = transMat;
            GL.MultMatrix(ref camMat);
            //GL.MultMatrix(ref transMat);
            //double[] rotations = camController.Update(e.Time);
            //GL.Rotate((float)rotations[0], new Vector3(1.0f, 0, 0));
            //GL.Rotate((float)rotations[1], new Vector3(0, 1.0f, 0));
            //GL.Rotate((float)rotations[2], new Vector3(0, 0, 1.0f));
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

            // Setup view and projection matrices.
            Matrix4 persMat, eyeMat;

            // Create and apply projection matrix.
            GL.MatrixMode(MatrixMode.Projection);
            float persAspect = (float)ClientRectangle.Width / (float)ClientRectangle.Height;
            persMat = OpenTK.Matrix4.CreatePerspectiveFieldOfView(degs2rads(60.0f), persAspect, 5000.0f, 300000000000.0f);
            GL.LoadMatrix(ref persMat);

            // Create and apply view matrix.
            GL.MatrixMode(MatrixMode.Modelview);
            eyeMat = OpenTK.Matrix4.LookAt(
                eye[0], eye[1], eye[2],
                0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f
                );
            GL.MultMatrix(ref eyeMat);

            GL.Translate(new Vector3((float)camController.Position[0], (float)camController.Position[1], (float)camController.Position[2]));
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
            var TexCoord = new double[2];

            // Enable Texturing and setup.
            GL.Enable(EnableCap.Texture2D);
            /*
            List<Triangle> skyboxTriangles = graphController.GetShape(skyBox.ShapeIndex);
            GL.BindTexture(TextureTarget.Texture2D, textureIds[6]);
            for (int i = 0; i < skyboxTriangles.Count; i++)
            {
                v1 = skyBox.vertices[skyboxTriangles[i].vi1];
                v2 = skyBox.vertices[skyboxTriangles[i].vi2];
                v3 = skyBox.vertices[skyboxTriangles[i].vi3];

                GL.Begin(PrimitiveType.Triangles);

                GL.TexCoord2(v1.u, v1.v);
                GL.Vertex3(v1.x, v1.y, v1.z);

                GL.TexCoord2(v2.u, v2.v);
                GL.Vertex3(v2.x, v2.y, v2.z);

                GL.TexCoord2(v3.u, v3.v);
                GL.Vertex3(v3.x, v3.y, v3.z);

                GL.End();
            }*/

                for (int j = 0; j < drawObjList.Count; j++)
                {
                    var curSimObj = drawObjList[j];
                    Graphics.GraphicsObject curGraphObj = curSimObj.GraphicsObj;
                    List<Triangle> shape = graphController.GetShape(curGraphObj.ShapeIndex);

                    /*
                    // Setup Color:
                    color = curGraphObj.Color;
                    OpenTK.Graphics.Color4 material_ambient = new OpenTK.Graphics.Color4(color[0], color[1], color[2], 1.0f);
                    OpenTK.Graphics.Color4 material_diffuse = new OpenTK.Graphics.Color4(color[3], color[4], color[5], 1.0f);
                    OpenTK.Graphics.Color4 material_specular = new OpenTK.Graphics.Color4(color[6], color[7], color[8], 1.0f);
                     */

                    if (j < 5)
                    { GL.BindTexture(TextureTarget.Texture2D, textureIds[j]); }
                    else
                    { GL.BindTexture(TextureTarget.Texture2D, textureIds[5]); }

                    for (int i = 0; i < shape.Count; i++)
                    {
                        v1 = curGraphObj.vertices[shape[i].vi1];
                        v2 = curGraphObj.vertices[shape[i].vi2];
                        v3 = curGraphObj.vertices[shape[i].vi3];

                        n1 = curGraphObj.vertices[shape[i].ni1];
                        n2 = curGraphObj.vertices[shape[i].ni2];
                        n3 = curGraphObj.vertices[shape[i].ni3];

                        GL.Begin(PrimitiveType.Triangles);

                        /*
                        // Setup material reflection
                        GL.Material(MaterialFace.Front, MaterialParameter.Ambient, material_ambient);
                        GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, material_diffuse);
                        GL.Material(MaterialFace.Front, MaterialParameter.Specular, material_specular);
                        GL.Material(MaterialFace.Front, MaterialParameter.Shininess, material_shininess);
                         */

                        GL.TexCoord2(v1.u, v1.v);
                        GL.Normal3(n1.x, n1.y, n1.z);
                        GL.Vertex3(v1.x, v1.y, v1.z);

                        GL.TexCoord2(v2.u, v2.v);
                        GL.Normal3(n2.x, n2.y, n2.z);
                        GL.Vertex3(v2.x, v2.y, v2.z);

                        GL.TexCoord2(v3.u, v3.v);
                        GL.Normal3(n3.x, n3.y, n3.z);
                        GL.Vertex3(v3.x, v3.y, v3.z);

                        GL.End();
                    }
                }


        }

        private void ShadeVertex(Vertex v, Vertex n, float[] color)
        {
            double result_r, result_g, result_b;
            Vertex l = new Vertex { x = light_position[0], y = light_position[1], z = light_position[2] };
            /**/
            float Ka_r = color[0];
            float Ka_g = color[1];
            float Ka_b = color[2];
            float Kd_r = color[3];
            float Kd_g = color[4];
            float Kd_b = color[5];
            float Ks_r = color[6];
            float Ks_g = color[7];
            float Ks_b = color[8];
            float Kexp = color[9];
            /**/
            /*
            float Kd_r = material_ambient[0];
            float Kd_g = material_ambient[1];
            float Kd_b = material_ambient[2];
            float Ka_r = material_diffuse[0];
            float Ka_g = material_diffuse[1];
            float Ka_b = material_diffuse[2];
            float Ks_r = material_specular[0];
            float Ks_g = material_specular[1];
            float Ks_b = material_specular[2];
            float Kexp = material_shininess[0];
            */
            Vertex v_to_l = new Vertex { x = l.x - v.x, y = l.y - v.y, z = l.z - v.z };
            Normalize(v_to_l);
            Vertex l_reflect = Reflect(v_to_l, n);
            Vertex v_to_eye = new Vertex { x = eye[0] - v.x, y = eye[1] - v.y, z = eye[2] - v.z };
            Normalize(v_to_eye);

            float l_distance = (float)Length(new Vector3((float)(l.x - v.x), (float)(l.y - v.y), (float)(l.z - v.z) ));
            float l_intensity = 700000 / l_distance;

            //GL.Disable(EnableCap.Lighting);
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
            result_r += Kd_r * Nonnegative_Dot(v_to_l, n) * l_intensity;
            result_g += Kd_g * Nonnegative_Dot(v_to_l, n) * l_intensity;
            result_b += Kd_b * Nonnegative_Dot(v_to_l, n) * l_intensity;

            // ADD SPECULAR COMPONENT.  (Try using nonnegative_dot() and pow().)
            result_r += Ks_r * Math.Pow(Nonnegative_Dot(l_reflect, v_to_eye), Kexp) * l_intensity;
            result_g += Ks_g * Math.Pow(Nonnegative_Dot(l_reflect, v_to_eye), Kexp) * l_intensity;
            result_b += Ks_b * Math.Pow(Nonnegative_Dot(l_reflect, v_to_eye), Kexp) * l_intensity;
            /**/

            // Apply final lighting result to vertex.
            GL.Color3(result_r, result_g, result_b);
        }

        private void Normalize(Vertex v)
        {
            double length = Length(v);
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
        
        double Length(Vertex v)
        {
            return Math.Sqrt(v.x*v.x + v.y*v.y + v.z*v.z);
        }

        double Length(Vector3 v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
        }
    }
}
