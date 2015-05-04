using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Engr.FileFormats.OBJ
{
    public class MATFile:BaseFormatReader
    {
        
        public List<Material> Materials { get; set; }
        private Material _current = null;
        public Dictionary<string, Image> Textures { get; set; }

        public MATFile(string path)
            : this(path, new FileSystemSource(new FileInfo(path).DirectoryName))
        {
            
        }

        public MATFile(string path, IFileSource fileSource)
        {
            FileSource = fileSource;
            Materials = new List<Material>();
            Textures = new Dictionary<string, Image>();
            using(var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Read(stream);
            }
        }

        public MATFile(Stream stream, IFileSource fileSource)
        {
            FileSource = fileSource;
            Materials = new List<Material>();
            Textures = new Dictionary<string, Image>();
            Read(stream);
        }



        protected override void Parse(string keyword, string data)
        {
            switch (keyword)
            {
                case "newmtl":
                    Materials.Add(_current = new Material(data));
                    break;
                case "Kd":
                    _current.Kd = data.ToVect3f();
                    break;
                case "Ka":
                    _current.Ka = data.ToVect3f();
                    break;
                case "Ks":
                    _current.Ks = data.ToVect3f();
                    break;                
                case "illum":
                    _current.IllumunationModel = int.Parse(data);
                    break;
                case "Tr":
                    //TODO not sure what this is
                    _current.Tr = data.ParseFloat();
                    break;
                case "Ns":
                    _current.Ns = data.ParseFloat();
                    break;
                case "map_Kd":
                    Textures.Add(data,Image.FromStream(FileSource.Get(data)));
                    //TODO load image
                    break;
                case "map_Ka":
                    break;
                case "r":
                    break;
                case "d":
                    break;
                case "sharpness":
                    break;
                case "Ni":
                    break;
                default:
                    throw new Exception(keyword);
                    //
            }
        }
    }
}