using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Engr.Maths;
using Engr.Maths.Vectors;
using Engr.Maths.Vectors.Engr.Maths;

namespace Engr.FileFormats.OBJ
{
    public class OBJFile
    {
        private readonly IMaterialSource _materialSource;
        //http://www.martinreddy.net/gfx/3d/OBJ.spec

        public List<Vect4f> Vertices { get; set; }
        public List<Vect3f> Normals { get; set; }
        public List<Vect3f> TextureVertices { get; set; }


        public List<MATFile> Materials { get; set; }
        public List<MATFile> LoadedMaterials { get; set; }
        public OBJFile()
        {
            Vertices = new List<Vect4f>();
            TextureVertices = new List<Vect3f>();
            Normals = new List<Vect3f>();
            Materials = new List<MATFile>();
            LoadedMaterials = new List<MATFile>();
        }

        public OBJFile(string path, IMaterialSource materialSource) : this()
        {
            _materialSource = materialSource;


            using(var stream = File.Open(path, FileMode.Open))
            {
                Read(stream);
            }
        }

        public OBJFile(Stream stream)
        {
            Read(stream);
        }

        private void Read(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) continue;
                    if (line[0] == '#') continue;

                    var split = line.Trim().Split(null, 2);
                    Parse(split[0].Trim(), split[1].Trim());
                }
            }
        }

        private MATFile _currentMaterial = null;
        private void Parse(string keyword, string data)
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

                    break;
                case "usemtl":
                    var material = Materials.FirstOrDefault(m => m.Name == data);
                    if (material == null)
                    {
                        material = LoadedMaterials.FirstOrDefault(m => m.Name == data);
                        Materials.Add(material);
                    }
                    _currentMaterial = material;
                    break;
                case "mtllib":
                    LoadedMaterials.AddRange( data.Split(null).SelectMany(lib => _materialSource.Load(lib)));
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
                    return new Vect4f(split[0].ParseFloat(),split[1].ParseFloat(),split[2].ParseFloat(),1.0f);
                default:
                    throw new Exception(data);
            }
        }
    }


    public interface IMaterialSource
    {
        IEnumerable<MATFile> Load(Stream stream);
        IEnumerable<MATFile> Load(string name);
    }


    public abstract class BaseMaterialLoader : IMaterialSource
    {

        public IEnumerable<MATFile> Load(Stream stream)
        {
            List<MATFile> materials = new List<MATFile>();
            List<string> stringBuffer = null;
            using (var sr = new StreamReader(stream))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) continue;
                    if (line[0] == '#') continue;
                    if (line.StartsWith("newmtl"))
                    {
                        if (stringBuffer == null)
                        {
                            stringBuffer = new List<string>() { };
                        }
                        else
                        {
                            materials.Add(new MATFile(stringBuffer));
                            stringBuffer.Clear();
                        }
                    }
                    stringBuffer.Add(line);
                }
                if(stringBuffer == null) throw new Exception("No material found");
                materials.Add(new MATFile(stringBuffer));
                return materials;
            }
        }


        //public IEnumerable<Material> Load(Stream stream)
        //{
        //    var materials = new List<Material>();
        //    Material current = null;
        //    using (var sr = new StreamReader(stream))
        //    {
        //        String line;
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            if (String.IsNullOrWhiteSpace(line)) continue;
        //            if (line[0] == '#') continue;
        //            var split = line.Trim().Split(null, 2);
        //            string keyword = split[0].Trim();
        //            string data = split[1].Trim();

        //            switch (keyword)
        //            {
        //                case "newmtl":
        //                    materials.Add(current = new Material(null));
        //                    break;
        //                case "Kd":
        //                    if (current != null)
        //                    {
        //                        current.Kd = new Vect3f(data.Split(null).Select(s => s.ParseFloat()).ToArray());
        //                    }
        //                    //Vertices.Add(ParseVertex(data));
        //                    break;
        //                default:
        //                    throw new Exception(keyword);
        //                    break;
        //                //
        //            }
        //        }
        //    }
        //    return materials;
        //}


        public abstract IEnumerable<MATFile> Load(string name);
    }


    public class FileMaterialLoader : BaseMaterialLoader
    {
        private readonly string[] _materialFiles;

        public FileMaterialLoader(params string[] materialFiles)
        {
            _materialFiles = materialFiles;
        }

        //public Material LoadMaterials(string fileName)
        //{
        //    var file = _materialFiles.FirstOrDefault(s => Path.GetFileName(s) == fileName);
        //    using (var stream = File.Open(file, FileMode.Open))
        //    {
        //        return file != null ? new Material(stream) : null;
        //    }
        //}

        public override IEnumerable<MATFile> Load(string name)
        {
            var file = _materialFiles.FirstOrDefault(s => Path.GetFileName(s) == name);
            if (file != null)
            {
                using (var stream = File.Open(file, FileMode.Open))
                {
                    return Load(stream);
                }
            }
            return null;
        }
    }

    public abstract class BaseLoader
    {
        
    }
}
