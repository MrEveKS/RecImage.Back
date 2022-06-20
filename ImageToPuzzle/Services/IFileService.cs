using System.IO;

namespace ImageToPuzzle.Services;

public interface IFileService
{
	Stream OpenRead(string fullFileName);
}