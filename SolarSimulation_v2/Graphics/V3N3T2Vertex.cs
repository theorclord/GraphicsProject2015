using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SolarSimulation_v2.Graphics
{
    struct V3N3T2Vertex
    {
        public Vector3 Vertex;
        public Vector3 Normal;
        public Vector2 Texture;

        public V3N3T2Vertex(Vector3 vertex, Vector3 normal, Vector2 uvVector)
        {
            Vertex = vertex;
            Normal = normal;
            Texture = uvVector;
        }
    }
}
