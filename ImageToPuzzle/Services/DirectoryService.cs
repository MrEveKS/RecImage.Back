using System.IO;

namespace ImageToPuzzle.Services
{
	public class DirectoryService : IDirectoryService
	{
		public FileInfo[] GetFiles(string fullPath)
		{
			return new DirectoryInfo(fullPath)
				.GetFiles();
		}
	}
}
