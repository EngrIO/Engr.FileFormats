using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engr.Maths.Vectors;

namespace Engr.FileFormats.STL
{
    public static class Extensions
    {
        public static IEnumerable<float> ReadSingles(this BinaryReader reader, uint length)
        {
            for (var i = 0; i < length; i++)
            {
                yield return reader.ReadSingle();
            }
        }

        public static Vect3f ToVect3f(this IEnumerable<float> data)
        {
            return new Vect3f(data.ToArray());
        }

        public static void Write(this BinaryWriter br, Vect3f v)
        {
            br.Write(v.X);
            br.Write(v.Y);
            br.Write(v.Z);
        }
    }
}