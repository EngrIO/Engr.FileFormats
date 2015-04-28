using Engr.Maths.Engr.Maths;

namespace Engr.FileFormats.STL
{
    public class Facet
    {
        public Vect3 Normal { get; set; }
        public Vect3 Vertex1 { get; set; }
        public Vect3 Vertex2 { get; set; }
        public Vect3 Vertex3 { get; set; }
        public Facet(Vect3 normal, Vect3 vertex1, Vect3 vertex2, Vect3 vertex3)
        {
            Normal = normal;
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Vertex3 = vertex3;
        }
    }
}