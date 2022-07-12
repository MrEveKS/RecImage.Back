namespace RecImage.Business.Services;

internal interface IDirectoryService
{
    FileInfo[] GetFiles(string fullPath);
}