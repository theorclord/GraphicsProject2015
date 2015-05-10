using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SolarSimulation_v2.Graphics
{
    struct GraphicsCast
    {
        public List<V3N3T2Vertex> vertices;
        public List<Triangle> triangles;

        public GraphicsCast(List<V3N3T2Vertex> newVertices, List<Triangle> newTriangles)
        {
            vertices = newVertices;
            triangles = newTriangles;
        }
        
    }
}
