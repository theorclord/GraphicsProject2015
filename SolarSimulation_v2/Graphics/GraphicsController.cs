using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace SolarSimulation_v2.Graphics
{
    class GraphicsController
    {
        /* NEW GL IMPLEMENTATION */
        public List<GraphicsCast> gfxCasts = new List<GraphicsCast>();

        public int LoadTexture(String filename)
        {
            int id = OpenTK.Graphics.OpenGL.GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

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

        public void ReadObjFile(String filename)
        {
            List<Vector3> newVertices = new List<Vector3>();
            List<Vector3> newNormals = new List<Vector3>();
            List<Triangle> newTriangles = new List<Triangle>();

            List<V3N3T2Vertex> realVertices = new List<V3N3T2Vertex>();

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
                                Vector3 newVertex = new Vector3();
                                newVertex.X = Single.Parse(curLineData[1], numFormat);
                                newVertex.Y = Single.Parse(curLineData[2], numFormat);
                                newVertex.Z = Single.Parse(curLineData[3], numFormat);

                                newVertices.Add(newVertex);
                                break;
                            }
                        case "vn":
                            {
                                Vector3 newNormal = new Vector3();
                                newNormal.X = Single.Parse(curLineData[1], numFormat);
                                newNormal.Y = Single.Parse(curLineData[2], numFormat);
                                newNormal.Z = Single.Parse(curLineData[3], numFormat);

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

            for (int i = 0; i < newVertices.Count; i++)
            {
                V3N3T2Vertex newVertex = new V3N3T2Vertex(newVertices[i], newNormals[i], CalcUVStretch(newVertices[i]));
                realVertices.Add(newVertex);
            }

            GraphicsCast newGraphCast = new GraphicsCast(realVertices, newTriangles);
            this.gfxCasts.Add(newGraphCast);
        }

        double Length(Vector3 v)
        { return Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z); }

        public Vector2 CalcUVStretch(Vector3 v)
        {
            Vector2 uvVector;
            var Ve = new Vector3(0, 0, -1);
            var Vn = new Vector3(0, 1, 0);

            var Vp = new Vector3(v);

            Vp.X = (float)(Vp.X / Length(Vp));
            Vp.Y = (float)(Vp.Y / Length(Vp));
            Vp.Z = (float)(Vp.Z / Length(Vp));

            var phi = Math.Acos(Dot(Vn, Vp));

            uvVector.X = (float)(phi / Math.PI);

            var theta = (Math.Acos(Dot(Vp, Ve) / Math.Sin(phi))) / (2 * Math.PI);

            if (Dot(Cross(Vn, Ve), Vp) > 0)
            { uvVector.Y = (float)theta; }
            else
            { uvVector.Y = (float)(1 - theta); }

            return uvVector;
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
        
        public Vector2 CalcUV(Vector3 v)
        {
            var dunit = Vector3.Normalize(v);

            var uvVector = new Vector2();
            if (dunit.X != 0)
            { uvVector.X = 0.5f + (float)(arctan2(dunit.Z, dunit.X) / (2 * Math.PI)); }
            else
            { uvVector.X = 0.5f; }

            uvVector.Y = (float)(Math.Asin(dunit.Y) / Math.PI);

            return uvVector;
        }

        double arctan2(double x, double y)
        {
            var determinant = (Math.Sqrt(x*x + y*y) - x) / y;
            return 2 * Math.Atan(determinant);
        }
    }
}
