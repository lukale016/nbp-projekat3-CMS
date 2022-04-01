namespace CMSServer.Services.FileSystemManager;
public class FileSystemManagerService : IFileSystemManagerService
{
    private Func<FilePostDto, string> FilePathBuilder = (FilePostDto dto) => $"{dto.Path}\\{dto.File.FileName}";
    public void CreateDirectory(string parent, string name)
    {
        if (string.IsNullOrWhiteSpace(parent) || string.IsNullOrWhiteSpace(name))
            throw new ResponseException(400, "Parameters not set for directory");

        Directory.SetCurrentDirectory(FolderNameConsts.ContentRootDir);

        if(!Directory.Exists(parent))
        {
            Directory.SetCurrentDirectory(FolderNameConsts.ParentFolder);
            throw new ResponseException(404, "Parent directory not found");
        }

        Directory.CreateDirectory(@$"{parent}\{name}");

        Directory.SetCurrentDirectory(FolderNameConsts.ParentFolder);
    }

    public void CreateDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ResponseException(500, "Something went wrong");

        Directory.SetCurrentDirectory(FolderNameConsts.ContentRootDir);
        Directory.CreateDirectory(path);
        Directory.SetCurrentDirectory(FolderNameConsts.ParentFolder);
    }

    public FileStream GetFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ResponseException(400, "Path is not set");
        Directory.SetCurrentDirectory(FolderNameConsts.ContentRootDir);
        FileStream fs = File.OpenRead(path);
        Directory.SetCurrentDirectory(FolderNameConsts.ParentFolder);
        return fs;
    }

    public async Task<StoredFile> StoreFile(FilePostDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            throw new ResponseException(400, "No file data");
        if (string.IsNullOrWhiteSpace(dto.Path))
            throw new ResponseException(400, "Parent path not set");

        StoredFile storedFile = new StoredFile()
        {
            FilePath = FilePathBuilder(dto),
            Type = (dto.File.ContentType.ToLower().Contains("image")) ? "image" : "text",
            ContentType = dto.File.ContentType
        };

        Directory.SetCurrentDirectory(FolderNameConsts.ContentRootDir);
        using (FileStream fs = File.Create(storedFile.FilePath))
        {
            await dto.File.CopyToAsync(fs);
        }
        Directory.SetCurrentDirectory(FolderNameConsts.ParentFolder);
        return storedFile;
    }
}
