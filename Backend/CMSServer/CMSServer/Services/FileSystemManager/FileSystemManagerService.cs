namespace CMSServer.Services.FileSystemManager;
public class FileSystemManagerService : IFileSystemManagerService
{
    public void CreateDirectory(string parent, string name)
    {
        if (string.IsNullOrWhiteSpace(parent) || string.IsNullOrWhiteSpace(name))
            throw new ResponseException(400, "Parameters not set for directory");

        Directory.SetCurrentDirectory(FolderNameConsts.ContentRootDir);

        if(!Directory.Exists(parent))
        {
            Directory.SetCurrentDirectory("..");
            throw new ResponseException(404, "Parent directory not found");
        }

        Directory.CreateDirectory(@$"{parent}\{name}");

        Directory.SetCurrentDirectory("..");
    }

    public FileStream GetFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ResponseException(400, "Path is not set");

        return File.OpenRead(path);
    }

    public void StoreFile(IFormFile file)
    {
        throw new NotImplementedException();
    }
}
