namespace CMSServer.Data.FolderRepo;

public interface IFolderRepository
{
    Task<FolderGetDto> GetFolderContent(string path);
    Task<Folder> GetFolder(string path);
    /// <summary>
    /// Should only be called when new user is registered in the system.
    /// </summary>
    Task CreateRootFolderForUser(string username);
    Task CreateFolder(FolderPostDto folder);
    Task<string> UpdateFolder(string oldPath, string newName);
    Task DeleteFolder(string path);
}
