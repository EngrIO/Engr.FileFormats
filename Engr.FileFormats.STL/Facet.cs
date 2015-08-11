using Engr.Geometry.Shapes;
using Engr.Maths.Vectors;

namespace Engr.FileFormats.STL
{
    public class Facet:Triangle
    {
        public Facet(Vect3 normal, Vect3 v1, Vect3 v2, Vect3 v3):
            base(v1,v2,v3)
        {

        }
    }
}