using System.IO;

namespace ImageToPuzzle.Services;

internal sealed class DirectoryService : IDirectoryService
{
	public FileInfo[] GetFiles(string fullPath)
	{
		return new DirectoryInfo(fullPath)
			.GetFiles();
	}
}