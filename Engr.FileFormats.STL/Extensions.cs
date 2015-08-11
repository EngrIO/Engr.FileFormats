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

        public static Vect3 ToVect3(this IEnumerable<float> data)
        {
            return new Vect3(data.ToArray());
        }

        public static void Write(this BinaryWriter br, Vect3 v)
        {
            br.Write((float)v.X);
            br.Write((float)v.Y);
            br.Write((float)v.Z);
        }
    }
}