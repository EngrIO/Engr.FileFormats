using System.IO;

namespace Engr.FileFormats.OBJ
{
    public class FileSystemSource:IFileSource
    {
        private readonly string _path;

        public FileSystemSource(string path)
        {
            _path = path;
        }

        public Stream Get(string filename)
        {
            return File.Open(Path.Combine(_path, filename), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}