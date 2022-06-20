using System.IO;

namespace ImageToPuzzle.Services;

public interface IDirectoryService
{
	FileInfo[] GetFiles(string fullPath);
}