using CMSServer.Services.FileSystemManager;
using MongoDB.Driver;

namespace CMSServer.Data.FileRepo;
public class FileRepository : IFileRepository
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IFileSystemManagerService _fileSystem;
    private IMongoDatabase _mongodb;
    private IMongoCollection<Folder> _folders;

    public FileRepository(IMongoDatabase db, UnitOfWork unit, IFileSystemManagerService fms)
    {
        _mongodb = db;
        _unitOfWork = unit;
        _fileSystem = fms;
        _folders = db.GetCollection<Folder>(CollectionConsts.FolderCollectionKey);
    }
    public Task DeleteFile(string parent, string name)
    {
        throw new NotImplementedException();
    }

    public async Task<StoredFile> GetFile(FileGetDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Parent) || string.IsNullOrWhiteSpace(dto.FileName))
            throw new ResponseException(400, "Parameters not set");

        Folder parentFolder = await _unitOfWork.FolderRepository.GetFolder(dto.Parent);

        if (parentFolder == null)
            throw new ResponseException(404, "Parent not found");

        StoredFile file = null;
        try
        {
            file = parentFolder.ChildFiles.Where(sf => sf.Name == dto.FileName).SingleOrDefault();
        }
        catch(Exception ex)
        {
            throw new ResponseException(500, ex.Message);
        }

        if (file == null)
            throw new ResponseException(404, "File not found");
        return file;
    }

    public async Task<FileInfoAndData> ReadFile(FileGetDto dto)
    {
        StoredFile file = await GetFile(dto);
        return new FileInfoAndData(file, await ReadFileData(file));
    }

    public async Task<byte[]> ReadFileData(StoredFile file)
    {
        byte[] data = null;
        using (FileStream fs = _fileSystem.GetFile(file.FilePath))
        {
            data = new byte[fs.Length];
            await fs.ReadAsync(data, 0, (int)fs.Length);
        }

        return data;
    }

    public async Task StoreFile(FilePostDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Path) || dto.File == null)
            throw new ResponseException(400, "Parameters not set");

        StoredFile file = await _fileSystem.StoreFile(dto);

        if (file == null)
            throw new ResponseException(500, "Something went wrong");

        Folder folder = await _unitOfWork.FolderRepository.GetFolder(dto.Path);
        if (folder == null)
            throw new ResponseException(404, "Parent folder not found");

        if (folder.ChildFiles == null)
            folder.ChildFiles = new List<StoredFile>() { file };
        else
            folder.ChildFiles.Add(file);

        var updateFilter = Builders<Folder>.Filter.Eq(nameof(Folder.FolderPath), folder.FolderPath);
        var updateData = Builders<Folder>.Update.Set(nameof(Folder.ChildFiles), folder.ChildFiles);

        await _folders.UpdateOneAsync(updateFilter, updateData);
    }

    public Task<StoredFile> UpdateFile(FilePostDto dto)
    {
        throw new NotImplementedException();
    }
}
