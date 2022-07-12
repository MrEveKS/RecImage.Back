namespace RecImage.Business.Services;

internal interface IFileService
{
    Stream OpenRead(string fullFileName);
}