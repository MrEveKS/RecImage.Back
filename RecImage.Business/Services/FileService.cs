namespace RecImage.Business.Services;

internal sealed class FileService : IFileService
{
    public Stream OpenRead(string fullFileName)
    {
        return File.OpenRead(fullFileName);
    }
}