using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SolarSimulation_v2.Graphics;
using SolarSimulation_v2.Physics;

namespace SolarSimulation_v2
{
    class RenderWindow : GameWindow
    {
        CameraController camController;
        GraphicsController graphController;
        PhysicsController physController;

        float[] light_position = { 0, 0, 0 };
        float[] light_ambient = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] material_ambient = { 0.8f, 0.8f, 0.8f, 1.0f };
        float[] material_diffuse = { 0.5f, 0.5f, 0.5f, 1.0f };
        float[] material_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] material_shininess = { 50.0f };
        float[] eye = { 0.0f, 4.0f, 30.0f };

        List<SimObject> drawObjList = new List<SimObject>();
        Matrix4 scaleMat = Matrix4.Identity;
        Matrix4 giantScaleMat = Matrix4.Identity;
        public int[] textureIds;

        /*
         * Textures found at: 
         * http://www.solarsystemscope.com/nexus/textures/planet_textures/
         */
        public RenderWindow(int width, int height, OpenTK.Graphics.GraphicsMode mode, string title)
            : base(width, height, mode, title)
        {
            physController = new PhysicsController();
            camController = new CameraController(new double[] { 0, 0.0f, -5000000 });
            graphController = new GraphicsController();
            graphController.ReadObjFile("sphere.obj");

            int[] newTextureIds = new int[11];
            newTextureIds[0] = graphController.LoadTexture("Textures/texture_sun.jpg");
            newTextureIds[1] = graphController.LoadTexture("Textures/texture_mercury.jpg");
            newTextureIds[2] = graphController.LoadTexture("Textures/texture_venus_atmosphere.jpg");
            newTextureIds[3] = graphController.LoadTexture("Textures/texture_earth_surface.jpg");
            newTextureIds[4] = graphController.LoadTexture("Textures/texture_mars.jpg");
            newTextureIds[5] = graphController.LoadTexture("Textures/texture_jupiter.jpg");
            newTextureIds[6] = graphController.LoadTexture("Textures/texture_saturn.jpg");
            newTextureIds[7] = graphController.LoadTexture("Textures/texture_uranus.jpg");
            newTextureIds[8] = graphController.LoadTexture("Textures/texture_neptune.jpg");
            newTextureIds[9] = graphController.LoadTexture("Textures/texture_moon.jpg");

            newTextureIds[10] = graphController.LoadTexture("Textures/texture_skybox.jpg");
            textureIds = newTextureIds;
        }

        public void AddDrawObj(SimObject sObj)
        { drawObjList.Add(sObj); }

        private void TransformObjs(List<Matrix4> transMatrix)
        {
            for (int simIndex = 0; simIndex < drawObjList.Count; simIndex++)
            {
                var curTransMatrix = transMatrix[simIndex];
                var curSimObject = drawObjList[simIndex];

                var newX = curSimObject.Position[0] + curTransMatrix.Column0.W;
                var newY = curSimObject.Position[1] + curTransMatrix.Column1.W;
                var newZ = curSimObject.Position[2] + curTransMatrix.Column2.W;

                curSimObject.Position = new double[] { newX, newY, newZ };
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            double timeSinceLastFrame = e.Time;

            // Hold T to accelerate to 1 day / frame
            if (OpenTK.Input.Keyboard.GetState().IsKeyDown(OpenTK.Input.Key.T))
            { timeSinceLastFrame *= 86400; }

            // Hold T + Y tp accelerate to 10 days / frame.
            // Y calculates only 10 seconds on it's own.
            if (OpenTK.Input.Keyboard.GetState().IsKeyDown(OpenTK.Input.Key.Y))
            { timeSinceLastFrame *= 10; }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            TransformObjs(physController.Update(drawObjList, timeSinceLastFrame));

            Matrix4 camMat = camController.Update(e.Time);
            GL.MultMatrix(ref camMat);
            
            DrawObjs();
            SwapBuffers();
        }

        public void DrawObjs()
        {
            Vector3 v1, v2, v3;
            Vector3 n1, n2, n3;
            Vector2 uv1, uv2, uv3;

            GL.Enable(EnableCap.Texture2D);
           
            DrawBackground();

            GL.MatrixMode(MatrixMode.Modelview);

            for (int i = 0; i < drawObjList.Count; i++)
            {
                var curDrawObj = drawObjList[i];
                GraphicsCast curGraphCast = graphController.gfxCasts[curDrawObj.GraphicsObj.CastIndex];

                Vector3 drawObjPos = new Vector3(
                    (float)curDrawObj.Position[0],
                    (float)curDrawObj.Position[1],
                    (float)curDrawObj.Position[2]
                    );
                Vector3 drawObjScale = new Vector3(
                    (float)curDrawObj.GraphicsObj.Scale[0],
                    (float)curDrawObj.GraphicsObj.Scale[1],
                    (float)curDrawObj.GraphicsObj.Scale[2]
                    );

                GL.PushMatrix();
                GL.Translate(drawObjPos);
                GL.Scale(drawObjScale);
                if (i > 0 && i < 5) GL.MultMatrix(ref scaleMat);
                else GL.MultMatrix(ref giantScaleMat);

                if (i < 9)
                { GL.BindTexture(TextureTarget.Texture2D, textureIds[i]); }
                else
                { GL.BindTexture(TextureTarget.Texture2D, textureIds[9]); }

                for (int j = 0; j < curGraphCast.triangles.Count; j++)
                {
                    v1 = curGraphCast.vertices[curGraphCast.triangles[j].vi1].Vertex;
                    v2 = curGraphCast.vertices[curGraphCast.triangles[j].vi2].Vertex;
                    v3 = curGraphCast.vertices[curGraphCast.triangles[j].vi3].Vertex;

                    n1 = curGraphCast.vertices[curGraphCast.triangles[j].ni1].Normal;
                    n2 = curGraphCast.vertices[curGraphCast.triangles[j].ni2].Normal;
                    n3 = curGraphCast.vertices[curGraphCast.triangles[j].ni3].Normal;

                    uv1 = curGraphCast.vertices[curGraphCast.triangles[j].vi1].Texture;
                    uv2 = curGraphCast.vertices[curGraphCast.triangles[j].vi2].Texture;
                    uv3 = curGraphCast.vertices[curGraphCast.triangles[j].vi3].Texture;
 
                    GL.Begin(PrimitiveType.Triangles);

                    GL.TexCoord2(uv1);
                    GL.Normal3(n1);
                    GL.Vertex3(v1);

                    GL.TexCoord2(uv2);
                    GL.Normal3(n2);
                    GL.Vertex3(v2);

                    GL.TexCoord2(uv3);
                    GL.Normal3(n3);
                    GL.Vertex3(v3);

                    GL.End();
                }

                GL.PopMatrix();
            }
        }

        public void InitScene()
        {
            // Setup lighting
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
            float[] xPlane = new float[] { 1.0f, 0, 0, 1 };
            float[] yPlane = new float[] { 0, 1.0f, 0, 1 };
            GL.Enable(EnableCap.TextureGenS);
            GL.Enable(EnableCap.TextureGenT);
            GL.TexGen(TextureCoordName.S, TextureGenParameter.TextureGenMode, (int)All.ObjectLinear);
            GL.TexGen(TextureCoordName.T, TextureGenParameter.TextureGenMode, (int)All.ObjectLinear);
            GL.TexGen(TextureCoordName.S, TextureGenParameter.ObjectPlane, xPlane);
            GL.TexGen(TextureCoordName.T, TextureGenParameter.ObjectPlane, yPlane);

            // Setup view and projection matrices.
            Matrix4 persMat, eyeMat;

            // Create and apply projection matrix.
            GL.MatrixMode(MatrixMode.Projection);
            float persAspect = (float)ClientRectangle.Width / (float)ClientRectangle.Height;
            persMat = OpenTK.Matrix4.CreatePerspectiveFieldOfView(degs2rads(60.0f), persAspect, 5500, (float)(8 * Math.Pow(10, 10)));
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
        { return (float)(degrees * Math.PI / 180); }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            CheckCommand(e.Key);
            base.OnKeyDown(e);
        }

        private void CheckCommand(OpenTK.Input.Key k)
        {
            switch (k)
            {
                case OpenTK.Input.Key.Keypad1: scaleMat = Matrix4.CreateScale(1); giantScaleMat = Matrix4.CreateScale(1); break;
                case OpenTK.Input.Key.Keypad2: scaleMat = Matrix4.CreateScale(10); giantScaleMat = Matrix4.CreateScale(2); break;
                case OpenTK.Input.Key.Keypad3: scaleMat = Matrix4.CreateScale(100); giantScaleMat = Matrix4.CreateScale(4); break;
                case OpenTK.Input.Key.Keypad4: scaleMat = Matrix4.CreateScale(200); giantScaleMat = Matrix4.CreateScale(8); break;
                case OpenTK.Input.Key.Keypad5: scaleMat = Matrix4.CreateScale(300); giantScaleMat = Matrix4.CreateScale(16); break;
                case OpenTK.Input.Key.Keypad6: scaleMat = Matrix4.CreateScale(400); giantScaleMat = Matrix4.CreateScale(24); break;
                case OpenTK.Input.Key.Space: CreateComet(); break;
                default: return;
            }
        }

        private void CreateComet()
        {
            PhysicsObject physObj = new PhysicsObject();
            physObj.Mass = 1 * Math.Pow(10, 22);
            physObj.Radius = 200000;
            physObj.Velocity = new double[] { 1.0, 20.0 * 1000, 1.0 };
            physObj.Acceleration = new double[3];

            var spawnPos = new double[] { -camController.Position[0], -camController.Position[1], -camController.Position[2] };
            spawnPos[2] += 25000;
            SimObject simObj = new SimObject(
                spawnPos, physObj, new Graphics.GraphicsObject(0, new double[] { physObj.Radius, physObj.Radius, physObj.Radius })
                );
            //comets.Add(drawObjList.Count, simObj);
            AddDrawObj(simObj);
        }

        private void DrawBackground()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, 1, 0, 1, -1, 1);

            GL.BindTexture(TextureTarget.Texture2D, textureIds[10]);

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex2(1, 1);

            GL.TexCoord2(0, 1);
            GL.Vertex2(0, 1);

            GL.End();
            GL.PopMatrix();
        }
    }
}
