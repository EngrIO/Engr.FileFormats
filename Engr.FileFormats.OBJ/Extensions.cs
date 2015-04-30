using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Engr.Maths.Vectors;


namespace Engr.FileFormats.OBJ
{
    public static class Extensions
    {
        public static float ParseFloat(this string f)
        {
            return float.Parse(f, CultureInfo.InvariantCulture.NumberFormat);
        }

        public static Vect3f ToVect3f(this string data)
        {
            return new Vect3f(data.Split(null).Select(s => s.ParseFloat()).ToArray());
        }

        
    }
}