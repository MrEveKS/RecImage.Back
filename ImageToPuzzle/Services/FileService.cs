using System.IO;

namespace ImageToPuzzle.Services
{
    public class FileService : IFileService
    {
        public Stream OpenRead(string fullFileName)
        {
            return File.OpenRead(fullFileName);
        }
    }
}
