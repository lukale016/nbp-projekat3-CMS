namespace CMSServer.Services.FileSystemManager;
public interface IFileSystemManagerService
{
    /// <summary>
    /// Retreives opened file stream. Be sure to dispose retreived stream.
    /// </summary>
    /// <param name="path">File path</param>
    /// <returns>Opened file stream</returns>
    FileStream GetFile(string path);
    Task<StoredFile> StoreFile(FilePostDto dto);
    void CreateDirectory(string parent, string name);
    void CreateDirectory(string path);
}
