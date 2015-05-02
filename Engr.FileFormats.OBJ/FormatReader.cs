using System;
using System.IO;

namespace Engr.FileFormats.OBJ
{
    public abstract class BaseFormatReader
    {
        protected IFileSource FileSource;
        protected void Read(Stream stream)
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
        protected abstract void Parse(string keyword, string data);
    }
}