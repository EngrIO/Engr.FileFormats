using System.IO;

namespace Engr.FileFormats.OBJ
{
    public interface IFileSource
    {
        Stream Get(string filename);
    }
}