using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engr.Maths.Vectors;

namespace Engr.FileFormats.OBJ
{
    public class OBJFile : BaseFormatReader
    {
        //http://www.martinreddy.net/gfx/3d/OBJ.spec
        public List<MATFile> MaterialFiles { get; set; }
        public List<Material> LoadedMaterials
        {
            get
            {
                return MaterialFiles.SelectMany(file => file.Materials).ToList();
            }
        }
        public List<Vect4f> Vertices { get; set; }
        public List<Vect3f> Normals { get; set; }
        public List<Vect3f> TextureVertices { get; set; }
        public List<Material> Materials { get; set; }
        public List<Face> Faces { get; set; }
        private Material _currentMaterial;
        public OBJFile(string path, IFileSource source)
        {
            MaterialFiles = new List<MATFile>();
            Vertices = new List<Vect4f>();
            TextureVertices = new List<Vect3f>();
            Normals = new List<Vect3f>();
            Materials = new List<Material>();
            Faces = new List<Face>();
            FileSource = source;
            MaterialFiles = new List<MATFile>();
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Read(stream);
            }

        }
        public OBJFile(string path)
            : this(path, new FileSystemSource(new FileInfo(path).DirectoryName))
        {

        }

        protected override void Parse(string keyword, string data)
        {
            switch (keyword.ToLower())
            {
                case "v":
                    Vertices.Add(ParseVertex(data));
                    break;
                case "vp":
                    throw new NotImplementedException();
                    break;
                case "vn":
                    Normals.Add(ParseNormal(data));
                    break;
                case "vt":
                    TextureVertices.Add(ParseTextureVertex(data));
                    break;
                case "g":

                    break;
                case "f":
                    Faces.Add(ParseFace(data));
                    break;
                case "usemtl":
                    Materials.Add(_currentMaterial = LoadedMaterials.First(m => m.Name == data));
                    break;
                case "mtllib":
                    using (var stream = FileSource.Get(data))
                    {
                        MaterialFiles.Add(new MATFile(stream, FileSource));
                    }
                    break;
                default:
                    throw new Exception(keyword);
                    break;
                //
            }
        }
        private Vect3f ParseNormal(string data)
        {
            return new Vect3f(data.Split(null).Select(s => s.ParseFloat()).ToArray());
        }

        private Vect3f ParseTextureVertex(string data)
        {
            var split = data.Split(null);
            switch (split.Length)
            {
                case 3:
                    return new Vect3f(split.Select(s => s.ParseFloat()).ToArray());
                case 2:
                    return new Vect3f(split[0].ParseFloat(), split[1].ParseFloat(), 0.0f);
                case 1:
                    return new Vect3f(split[0].ParseFloat(), 0.0f, 0.0f);
                default:
                    throw new Exception(data);
            }
        }

        public Vect4f ParseVertex(string data)
        {
            var split = data.Split(null);
            switch (split.Length)
            {
                case 4:
                    return new Vect4f(split.Select(s => s.ParseFloat()).ToArray());
                case 3:
                    return new Vect4f(split[0].ParseFloat(), split[1].ParseFloat(), split[2].ParseFloat(), 1.0f);
                default:
                    throw new Exception(data);
            }
        }

        public Face ParseFace(string data)
        {
            var split = data.Split((char[])null,StringSplitOptions.RemoveEmptyEntries);
            var face = new Face
            {
                Vertices = new Vect4f[split.Length],
                Textures = new Vect3f[split.Length],
                Normals = new Vect3f[split.Length],
                Material = _currentMaterial
            };
            for (int i = 0; i < split.Length; i++)
            {
                GetReferences(split[i], out face.Vertices[i], out face.Textures[i], out face.Normals[i]);
            }
            return face;
        }

        private void GetReferences(string data, out Vect4f v, out Vect3f t, out Vect3f n)
        {
            v = null;
            t = null;
            n = null;
            var split = data.Split('/');
            switch (split.Length)
            {
                case 3:
                    n = Normals[int.Parse(split[2]) - 1];
                    goto case 2;
                case 2:
                    t = TextureVertices[int.Parse(split[1]) - 1];
                    goto case 1;
                case 1:
                    v = Vertices[int.Parse(split[0]) - 1];
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}
