using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SolarSimulation.Graphics
{
	class GraphicsController
	{
        List<Matrix4> transMatrices = new List<Matrix4>();

        public GraphicsObject ReadObjFile(String fileName)
        {
            List<Vertex> newVertices = new List<Vertex>();
            List<Vertex> newNormals = new List<Vertex>();
            List<Triangle> newTriangles = new List<Triangle>();

            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(fileName))
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
            return new GraphicsObject(newVertices, newNormals, newTriangles);
        }
	}
}