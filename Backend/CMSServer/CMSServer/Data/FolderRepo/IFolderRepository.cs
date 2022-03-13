namespace CMSServer.Data.FolderRepo;

public interface IFolderRepository
{
    Task<FolderGetDto> GetFolderContent(string path);
    Task CreateFolder(FolderPostDto folder);
    Task<string> UpdateFolder(string oldFolderPath, string newFolderPath);
    Task DeleteFolder(string path);
}
