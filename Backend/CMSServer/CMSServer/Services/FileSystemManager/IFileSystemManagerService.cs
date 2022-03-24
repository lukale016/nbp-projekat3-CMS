namespace CMSServer.Services.FileSystemManager;
public interface IFileSystemManagerService
{
    /// <summary>
    /// Retreives opened file stream. Be sure to dispose retreived stream.
    /// </summary>
    /// <param name="path">File path</param>
    /// <returns>Opened file stream</returns>
    FileStream GetFile(string path);
    void StoreFile(IFormFile file);
    void CreateDirectory(string parent, string name);
}
