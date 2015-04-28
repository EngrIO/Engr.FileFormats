using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Engr.Maths.Engr.Maths;

namespace Engr.FileFormats.STL
{
    public class STLFile
    {
        public STLType STLType { get; set; }
        public IList<Facet> Facets { get; set; }
        public string Header { get; set; }

        public STLFile()
        {
            STLType = STLType.Binary;
            Facets = new List<Facet>();
        }

        public STLFile(string path)
        {
            using(var stream = File.Open(path, FileMode.Open))
            {
                Read(stream);
            }
        }

        public STLFile(Stream stream)
        {
            Read(stream);
        }


        private void Read(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                var line = sr.ReadLine();
                if (line != null && Regex.Match(line, "^solid \\S+$").Success)
                {
                    STLType = STLType.ASCII;
                    Facets = ReadASCII(stream).ToList();
                }
                else
                {
                    STLType = STLType.Binary;
                    Facets = ReadBinary(stream).ToList();
                }
                
            }
        }

        private IEnumerable<Facet> ReadBinary(Stream stream)
        {
            stream.Position = 0;
            using (var br = new BinaryReader(stream))
            {
                Header = Encoding.UTF8.GetString(br.ReadBytes(80)).Replace("\0", String.Empty).Trim();
                var count = (int)br.ReadUInt32();
                for (var i = 0; i < count; i++)
                {
                    var normal = br.ReadSingles(3).ToVect3();
                    var v1 = br.ReadSingles(3).ToVect3();
                    var v2 = br.ReadSingles(3).ToVect3();
                    var v3 = br.ReadSingles(3).ToVect3();
                    br.ReadUInt16();//attributeByteCount
                    yield return new Facet(normal,v1,v2,v3);
                }
            }
        }

        private IEnumerable<Facet> ReadASCII(Stream stream)
        {
            stream.Position = 0;
            Vect3 normal = null;
            var vertices = new Vect3[3];
            var i = 0;
            const NumberStyles style = NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign | NumberStyles.Number;
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Trim().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    switch (split[0].ToLower())
                    {
                        case "solid":
                            if (split.Length > 1)
                            {
                                Header = split[1];
                            }
                            break;
                        case "facet":
                            normal = new Vect3(float.Parse(split[2], style), float.Parse(split[3], style), float.Parse(split[4], style));
                            break;
                        case "outer":
                            break;
                        case "vertex":
                            vertices[i++] = new Vect3(float.Parse(split[1], style),float.Parse(split[2], style),float.Parse(split[3], style));
                            break;
                        case "endloop":
                            break;
                        case "endfacet":
                            if (normal != null && vertices.All(v => v != null))
                            {
                                yield return new Facet(normal, vertices[0], vertices[1], vertices[2]);
                            }
                            i = 0;
                            break;
                        case "endsolid":
                            break;
                    }
                }
            }
        }

        public STLFile FromString(string text)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
            {
                return new STLFile(stream);
            }
        }
    }
}

