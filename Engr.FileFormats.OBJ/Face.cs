using Engr.Maths.Vectors;

namespace Engr.FileFormats.OBJ
{
    public class Face
    {
        public Vect4f[] Vertices { get; set; }
        public Vect3f[] Textures { get; set; }
        public Vect3f[] Normals { get; set; }
        public Material Material { get; set; }
    }
}