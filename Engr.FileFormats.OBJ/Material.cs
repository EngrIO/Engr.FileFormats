using System.Linq;
using Engr.Maths.Vectors;

namespace Engr.FileFormats.OBJ
{
    public class Material
    {
        //http://paulbourke.net/dataformats/mtl/
        public string Name { get; set; }

        public Vect3f Kd { get; set; }
        public Vect3f Ka { get; set; }
        public Vect3f Ks { get; set; }
        public float Tr { get; set; }
        public float Ns { get; set; }

        public int IllumunationModel { get; set; }

        public Material(string name)
        {
            Name = name;
        }
    }
}