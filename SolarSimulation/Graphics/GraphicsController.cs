using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SolarSimulation.Graphics
{
	class GraphicsController
	{
        List<GraphicsObject> gfxObjs = new List<GraphicsObject>();
        List<List<Triangle>> shapes = new List<List<Triangle>>();
        float[] defaultMaterial =
                new float[] {0.2f, 0.2f, 0.2f,
                             0.5f, 0.5f, 0.5f,
                             0.8f, 1.0f, 0.8f, 10};

        private int shapeIndex = 0;

        public List<Triangle> GetShape(int index)
        { return shapes[index]; }

        public GraphicsObject CreateGraphicsObj(int shape, float[] color)
        { 
            return new GraphicsObject(
            new List<Vertex>(gfxObjs[shape].vertices), 
            new List<Vertex>(gfxObjs[shape].normals),
            shape, color); 
        }

        public GraphicsObject CreateGraphicsObj(int shape)
        { return CreateGraphicsObj(shape, defaultMaterial); }

        public void ReadObjFile(String filename)
        {
            List<Vertex> newVertices = new List<Vertex>();
            List<Vertex> newNormals = new List<Vertex>();
            List<Triangle> newTriangles = new List<Triangle>();

            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(filename))
            {
                String curLine = "";
                String[] curLineData;

                // Neccessary for double string conversion.
                // This is due to commas being used instead of punctuation in some cultures.
                System.Globalization.NumberFormatInfo numFormat = 
                    System.Globalization.CultureInfo.GetCultureInfo("en-US").NumberFormat;


                while (!streamReader.EndOfStream)
                {
                    curLine = streamReader.ReadLine();
                    curLineData = curLine.Split(' ');
                    switch (curLineData[0])
                    {
                        case "v":
                            {
                                Vertex newVertex = new Vertex();
                                newVertex.x = Double.Parse(curLineData[1], numFormat);
                                newVertex.y = Double.Parse(curLineData[2], numFormat);
                                newVertex.z = Double.Parse(curLineData[3], numFormat);

                                double[] texCoords = GetTextureCoord(newVertex.x, newVertex.y, newVertex.z);
                                newVertex.u = texCoords[0];
                                newVertex.v = texCoords[1];

                                newVertices.Add(newVertex);
                                break;
                            }
                        case "vn":
                            {
                                Vertex newNormal = new Vertex();
                                newNormal.x = Double.Parse(curLineData[1], numFormat);
                                newNormal.y = Double.Parse(curLineData[2], numFormat);
                                newNormal.z = Double.Parse(curLineData[3], numFormat);
                                newNormals.Add(newNormal);
                                break;
                            }
                        case "f":
                            {
                                Triangle newTriangle = new Triangle();
                                StringBuilder strSanitizer = new StringBuilder();

                                /* 
                                 * Creates a stringbuilder to edit input data for easier manipulation. 
                                 * The string sanitizer creates a new string where all values are seperated
                                 * by '/', and then resplits that string into a new array.
                                 */
                                strSanitizer.Append(curLineData[1] + '/' + curLineData[2] + '/' + curLineData[3]);
                                strSanitizer.Replace("//", "/");
                                String[] vertice_normal_array = strSanitizer.ToString().Split('/');

                                // Conversion from .obj files 1-indexing to ordinary programming happens here.
                                newTriangle.vi1 = Int32.Parse(vertice_normal_array[0]) - 1;
                                newTriangle.ni1 = Int32.Parse(vertice_normal_array[1]) - 1;
                                newTriangle.vi2 = Int32.Parse(vertice_normal_array[2]) - 1;
                                newTriangle.ni2 = Int32.Parse(vertice_normal_array[3]) - 1;
                                newTriangle.vi3 = Int32.Parse(vertice_normal_array[4]) - 1;
                                newTriangle.ni3 = Int32.Parse(vertice_normal_array[5]) - 1;

                                strSanitizer.Clear();
                                newTriangles.Add(newTriangle);
                                break;
                            }
                        default: throw new ArgumentException("Input file contains line not starting with v, vn, or f. Please check input.");
                    }
                }
            }
            
            GraphicsObject newGraphObj = new GraphicsObject(newVertices, newNormals, shapeIndex, defaultMaterial);
            this.gfxObjs.Add(newGraphObj);
            this.shapes.Add(newTriangles);
            shapeIndex++;
        }
        public void ReadSkyObjFile(String filename)
        {
            List<Vertex> newVertices = new List<Vertex>();
            List<Triangle> newTriangles = new List<Triangle>();

            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(filename))
            {
                String curLine = "";
                String[] curLineData;

                // Neccessary for double string conversion.
                // This is due to commas being used instead of punctuation in some cultures.
                System.Globalization.NumberFormatInfo numFormat =
                    System.Globalization.CultureInfo.GetCultureInfo("en-US").NumberFormat;


                while (!streamReader.EndOfStream)
                {
                    curLine = streamReader.ReadLine();
                    curLineData = curLine.Split(' ');
                    switch (curLineData[0])
                    {
                        case "v":
                            {
                                Vertex newVertex = new Vertex();
                                newVertex.x = Double.Parse(curLineData[1], numFormat);
                                newVertex.y = Double.Parse(curLineData[2], numFormat);
                                newVertex.z = Double.Parse(curLineData[3], numFormat);
                                newVertices.Add(newVertex);
                                break;
                            }
                        case "f":
                            {
                                Triangle newTriangle = new Triangle();
                                StringBuilder strSanitizer = new StringBuilder();

                                /* 
                                 * Creates a stringbuilder to edit input data for easier manipulation. 
                                 * The string sanitizer creates a new string where all values are seperated
                                 * by '/', and then resplits that string into a new array.
                                 */
                                strSanitizer.Append(curLineData[1] + '/' + curLineData[2] + '/' + curLineData[3]);
                                strSanitizer.Replace("//", "/");
                                String[] vertice_normal_array = strSanitizer.ToString().Split('/');

                                // Conversion from .obj files 1-indexing to ordinary programming happens here.
                                newTriangle.vi1 = Int32.Parse(vertice_normal_array[0]) - 1;
                                newTriangle.ni1 = Int32.Parse(vertice_normal_array[1]) - 1;
                                newTriangle.vi2 = Int32.Parse(vertice_normal_array[2]) - 1;
                                newTriangle.ni2 = Int32.Parse(vertice_normal_array[3]) - 1;
                                newTriangle.vi3 = Int32.Parse(vertice_normal_array[4]) - 1;
                                newTriangle.ni3 = Int32.Parse(vertice_normal_array[5]) - 1;

                                strSanitizer.Clear();
                                newTriangles.Add(newTriangle);
                                break;
                            }
                        default: throw new ArgumentException("Input file contains line not starting with v, vn, or f. Please check input.");
                    }
                }
            }

            //GraphicsObject newGraphObj = new GraphicsObject(newVertices, newNormals, shapeIndex, defaultMaterial);
            //this.gfxObjs.Add(newGraphObj);
            this.shapes.Add(newTriangles);
            shapeIndex++;
            //new GraphicsObject(newVertices, newNormals, newTriangles);
        }

        /*
         * Found on OpenTK documentation.
         * Link:
         * www.opentk.com/doc/grahics/textures/loading
         */
        public int LoadTexture(String filename)
        {
            int id = OpenTK.Graphics.OpenGL.GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            /*
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Repeat);
            */
            Bitmap bmp = new Bitmap(filename);
            BitmapData bmp_data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height), 
                ImageLockMode.ReadOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );

            GL.TexImage2D(TextureTarget.Texture2D, 0, 
                PixelInternalFormat.Rgba, 
                bmp_data.Width, bmp_data.Height, 0, 
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
                PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            return id;
        }

        /*
         * Algorithm described at:
         * https://www.cs.unc.edu/~rademach/xroads-RT/RTarticle.html
         */
        public double[] GetTextureCoord(double x, double y, double z)
        {
            double u, v;
            var Ve = new Vector3(0, 0, 1);
            var Vn = new Vector3(0, 1, 0);

            var Vp = new Vector3((float)x, (float)y, (float)z);

            Vp.X = (float)(Vp.X / Length(Vp));
            Vp.Y = (float)(Vp.Y / Length(Vp));
            Vp.Z = (float)(Vp.Z / Length(Vp));

            var phi = Math.Acos( Dot(Vn, Vp));

            v = phi / Math.PI;

            var theta = (Math.Acos(Dot(Vp, Ve) / Math.Sin(phi))) / (2 * Math.PI);

            if (Dot(Cross(Vn, Ve), Vp) > 0)
            { u = theta; }
            else
            { u = 1 - theta; }

            return new double[] {u, v};
        }

        double Length(Vector3 v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
        }
        double Dot(Vector3 v, Vector3 n)
        { return v.X * n.X + v.Y * n.Y + v.Z * n.Z; }

        Vector3 Cross(Vector3 v, Vector3 n)
        {
            return new Vector3(
                v.Y * n.Z - v.Z * n.Y,  // X-coord
                v.X * n.Z - v.Z * n.X,  // Y-coord
                v.X * n.Y - v.Y * n.X   // Z-coord
            );
        }
        public Vector4 Homogenize(Vertex v)
        { return new Vector4((float)v.x, (float)v.y, (float)v.z, 1.0f); }

        public void Dehomogenize(Vector4 v, ref Vertex u)
        {
            u.x = v.X;
            u.y = v.Y;
            u.z = v.Z;
        }
	}
}