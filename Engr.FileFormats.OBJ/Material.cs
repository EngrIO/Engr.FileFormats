using System;
using System.Collections.Generic;
using System.Linq;
using Engr.Maths.Vectors;

namespace Engr.FileFormats.OBJ
{
    public class MATFile
    {
        //http://paulbourke.net/dataformats/mtl/
        public string Name { get; set; }

        public Vect3f Kd { get; set; }
        public Vect3f Ka { get; set; }
        public Vect3f Ks { get; set; }
        public float Tr { get; set; }
        public float Ns { get; set; }

        public int IllumunationModel { get; set; }


        public MATFile(IEnumerable<string> lines)
        {
            foreach (var split in lines.Select(line => line.Trim().Split(null, 2)))
            {
                Parse(split[0], split[1]);
            }
        }

        private void Parse(string keyword, string data)
        {
            switch (keyword)
            {
                case "newmtl":
                    Name = data;
                    break;
                case "Kd":
                    Kd = data.ToVect3f();
                    break;
                case "Ka":
                    Ka = data.ToVect3f();
                    break;
                case "Ks":
                    Ks = data.ToVect3f();
                    break;                
                case "illum":
                    IllumunationModel = int.Parse(data);
                    break;
                case "Tr":
                    //TODO not sure what this is
                    Tr = data.ParseFloat();
                    break;
                case "Ns":
                    Ns = data.ParseFloat();
                    break;
                case "map_Kd":
                    //TODO load image
                    break;
                default:
                    throw new Exception(keyword);
                //
            }
        }
    }
}