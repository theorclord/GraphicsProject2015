using System;
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
        SimObject skybox;
        int skyboxTextureId;

        PhysicsController physController;
        CameraController camController;
        GraphicsController graphController;

        float[] light_position = { 0, 0, 0 };
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

            int[] newTextureIds = new int[8];
            GL.GenTextures(8, newTextureIds);
            newTextureIds[0] = graphController.LoadTexture("Textures/texture_sun.jpg");
            newTextureIds[1] = graphController.LoadTexture("Textures/texture_earth_clouds.jpg");
            newTextureIds[2] = graphController.LoadTexture("Textures/texture_mercury.jpg");
            newTextureIds[3] = graphController.LoadTexture("Textures/texture_venus_surface.jpg");
            newTextureIds[4] = graphController.LoadTexture("Textures/texture_mars.jpg");
            newTextureIds[5] = graphController.LoadTexture("Textures/texture_jupiter.jpg");
            newTextureIds[6] = graphController.LoadTexture("Textures/texture_saturn.jpg");
            newTextureIds[7] = graphController.LoadTexture("Textures/texture_moon.jpg");

            skyboxTextureId = graphController.LoadTexture("Textures/texture_skybox.jpg");
            textureIds = newTextureIds;

            // Skybox
            double[] skyboxPos = new double[] {(float)-Math.Pow(10, 11) / 2, (float)-Math.Pow(10, 11) / 2, (float)-Math.Pow(10, 11) / 2};
            skybox = new SimObject(skyboxPos, new PhysicsObject(), graphController.CreateSkyboxObj(1));
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
                PhysicsObject physObj = new PhysicsObject();
                physObj.Mass = 1000 * Math.Pow(10, 16);
                physObj.Radius = 200000;
                physObj.Velocity = new double[] { 1.0, 1.0, 1.0 };
                physObj.Acceleration = new double[3];

                var spawnPos = new double[] {-camController.Position[0], -camController.Position[1], -camController.Position[2]};
                spawnPos[2] += 25000;
                SimObject simObj = new SimObject(
                    spawnPos, physObj, graphController.CreateGraphicsObj(0));
                simObj.Scale = new double[] { physObj.Radius, physObj.Radius, physObj.Radius };
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
            //NewTransformObjs(transMats);
            TransformObjs(transMats);

            Matrix4 transMat = camController.Update(e.Time);
            Matrix4 camMat = transMat;
            GL.MultMatrix(ref camMat);
            DrawObjs();
            //NewDrawObjs();
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
            
            // Enable automatic texture-coordinate generation
            /*
            float[] zPlane = new float[] { 0, 0, 1, 1 };
            float[] yPlane = new float[] { 0, 1, 0, 1 };
            GL.Enable(EnableCap.TextureGenS);
            GL.Enable(EnableCap.TextureGenT);
            GL.TexGen(TextureCoordName.S, TextureGenParameter.TextureGenMode, (int)All.ObjectLinear);
            GL.TexGen(TextureCoordName.T, TextureGenParameter.TextureGenMode, (int)All.ObjectLinear);
            GL.TexGen(TextureCoordName.S, TextureGenParameter.ObjectPlane, zPlane);
            GL.TexGen(TextureCoordName.T, TextureGenParameter.ObjectPlane, yPlane);
             */

            // Setup view and projection matrices.
            Matrix4 persMat, eyeMat;

            // Create and apply projection matrix.
            GL.MatrixMode(MatrixMode.Projection);
            float persAspect = (float)ClientRectangle.Width / (float)ClientRectangle.Height;
            persMat = OpenTK.Matrix4.CreatePerspectiveFieldOfView(degs2rads(60.0f), persAspect, 5500, 300000000000.0f);
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

            var skyGraphics = skybox.GraphicsObj;
            List<Triangle> skyboxTriangles = graphController.GetShape(skyGraphics.ShapeIndex);

            //DrawSkyBox(); // Currently a background (not working)

            GL.Enable(EnableCap.Lighting);

                for (int j = 0; j < drawObjList.Count; j++)
                {
                    var curSimObj = drawObjList[j];
                    Graphics.GraphicsObject curGraphObj = curSimObj.GraphicsObj;
                    List<Triangle> shape = graphController.GetShape(curGraphObj.ShapeIndex);

                    if (j < 7)
                    { GL.BindTexture(TextureTarget.Texture2D, textureIds[j]); }
                    else
                    { GL.BindTexture(TextureTarget.Texture2D, textureIds[7]); }

                    for (int i = 0; i < shape.Count; i++)
                    {
                        v1 = curGraphObj.vertices[shape[i].vi1];
                        v2 = curGraphObj.vertices[shape[i].vi2];
                        v3 = curGraphObj.vertices[shape[i].vi3];

                        n1 = curGraphObj.normals[shape[i].ni1];
                        n2 = curGraphObj.normals[shape[i].ni2];
                        n3 = curGraphObj.normals[shape[i].ni3];

                        GL.Begin(PrimitiveType.Triangles);

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
        
        private void Normalize(Vertex v)
        {
            double length = Length(v);
            v.x /= length;
            v.y /= length;
            v.z /= length;
        }

        double Dot(Vertex v, Vertex n)
        { return v.x * n.x + v.y * n.y + v.z * n.z; }
        
        double Length(Vertex v)
        {
            return Math.Sqrt(v.x*v.x + v.y*v.y + v.z*v.z);
        }

        private void DrawSkyBox()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, 1, 0, 1, 0, 1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.BindTexture(TextureTarget.Texture2D, skyboxTextureId);

            GL.Disable(EnableCap.TextureGenS);
            GL.Disable(EnableCap.TextureGenT);
            GL.DepthMask(false);

            GL.Begin(PrimitiveType.Quads);
            {
                GL.TexCoord2(0.0, 0.0);
                GL.Vertex2(0.0, 0.0);

                GL.TexCoord2(1.0, 0.0);
                GL.Vertex2(1.0, 0.0);

                GL.TexCoord2(1.0, 1.0);
                GL.Vertex2(1.0, 1.0);
                
                GL.TexCoord2(0.0, 1.0);
                GL.Vertex2(0.0, 1.0);
            }
            GL.End();

            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            
            GL.Enable(EnableCap.TextureGenS);
            GL.Enable(EnableCap.TextureGenT);
            GL.DepthMask(true);

        }
    }
}
