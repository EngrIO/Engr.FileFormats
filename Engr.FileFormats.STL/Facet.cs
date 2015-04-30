
using Engr.Maths.Vectors;

namespace Engr.FileFormats.STL
{
    public class Facet
    {
        public Vect3f Normal { get; set; }
        public Vect3f Vertex1 { get; set; }
        public Vect3f Vertex2 { get; set; }
        public Vect3f Vertex3 { get; set; }
        public Facet(Vect3f normal, Vect3f vertex1, Vect3f vertex2, Vect3f vertex3)
        {
            Normal = normal;
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Vertex3 = vertex3;
        }
    }
}