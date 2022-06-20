using System.IO;

namespace ImageToPuzzle.Services;

internal sealed class FileService : IFileService
{
	public Stream OpenRead(string fullFileName)
	{
		return File.OpenRead(fullFileName);
	}
}