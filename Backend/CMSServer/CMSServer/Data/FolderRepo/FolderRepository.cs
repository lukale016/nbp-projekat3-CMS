using CMSServer.Services.FileSystemManager;
using MongoDB.Driver;

namespace CMSServer.Data.FolderRepo;
public class FolderRepository : IFolderRepository
{
    private IMongoDatabase _mongoDb;
    private IMongoCollection<Folder> _folders;
    private IFileSystemManagerService _fileSystemManager;

    public FolderRepository(IMongoDatabase db, IFileSystemManagerService fs)
    {
        _mongoDb = db;
        _folders = db.GetCollection<Folder>(CollectionConsts.FolderCollectionKey);
        _fileSystemManager = fs;
    }

    public async Task CreateFolder(FolderPostDto folder)
    {
        if (string.IsNullOrWhiteSpace(folder.Name) || string.IsNullOrWhiteSpace(folder.Parent))
            throw new ResponseException(400, "Parameters are null or empty");

        Folder dbFolderEntry = new Folder()
        {
            Parent = folder.Parent,
            FolderPath = $"{folder.Parent}\\{folder.Name}"
        };

        var filter = Builders<Folder>.Filter.Eq(nameof(Folder.FolderPath), dbFolderEntry.FolderPath);
        Folder dbFolder = null;
        try
        {
            dbFolder = (await _folders.FindAsync(filter)).SingleOrDefault();
        }
        catch (Exception ex)
        {
            throw new ResponseException(500, ex.Message);
        }

        if (dbFolder != null)
            throw new ResponseException(409, "Folder already exists");

        Folder parent = null;
        var parentFilter = Builders<Folder>.Filter.Eq(nameof(parent.FolderPath), dbFolderEntry.Parent);
        try
        {
            parent = (await _folders.FindAsync(parentFilter)).SingleOrDefault();
        }
        catch (Exception ex)
        {
            throw new ResponseException(500, ex.Message);
        }
        
        if (parent == null)
            throw new ResponseException(400, "Parent does not exist");

        await _folders.InsertOneAsync(dbFolderEntry);

        if (parent.ChildFolders != null)
            parent.ChildFolders.Add(dbFolderEntry.Name);
        else
            parent.ChildFolders = new List<string> { dbFolderEntry.Name };

        _fileSystemManager.CreateDirectory(parent.FolderPath, dbFolderEntry.Name);

        var updateFilter = Builders<Folder>.Filter.Eq(nameof(Folder.FolderPath), parent.FolderPath);
        var updateData = Builders<Folder>.Update.Set(nameof(Folder.ChildFolders), parent.ChildFolders);
        await _folders.UpdateOneAsync(updateFilter, updateData);
    }

    public async Task CreateRootFolderForUser(string root)
    {
        if (string.IsNullOrWhiteSpace(root))
            throw new ResponseException(500, "Something went wrong");

        Folder folder = new Folder() 
        {
             FolderPath = root
        };

        try
        {
            await _folders.InsertOneAsync(folder);
        }
        catch (Exception ex)
        {
            throw new ResponseException(500, ex.Message);
        }

        _fileSystemManager.CreateDirectory(root);
    }

    public Task DeleteFolder(string path)
    {
        throw new NotImplementedException();
    }

    public async Task<Folder> GetFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ResponseException(400, "Path not set");

        Folder folder = null;
        try
        {
            var filter = Builders<Folder>.Filter.Eq(nameof(Folder.FolderPath), path);
            folder = (await _folders.FindAsync(filter)).SingleOrDefault();
        }
        catch(Exception ex)
        {
            throw new ResponseException(500, ex.Message);
        }

        return folder;
    }

    public async Task<FolderGetDto> GetFolderContent(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ResponseException(400, "Path not set");

        var searchFilter = Builders<Folder>.Filter.Eq(nameof(Folder.FolderPath), path);
        Folder dbFolder = null;
        try
        {
            dbFolder = (await _folders.FindAsync(searchFilter)).SingleOrDefault();
        }
        catch(Exception ex)
        {
            throw new ResponseException(500, ex.Message);
        }

        if (dbFolder == null)
            throw new ResponseException(404, "Folder with given path does not exist");

        List<FolderItemDto> result = new List<FolderItemDto>();
        if (dbFolder.ChildFolders != null)
            result.AddRange(dbFolder.ChildFolders.Select(fn => new FolderItemDto(fn, FolderItemTypeConsts.Folder)));
        if (dbFolder.ChildFiles != null)
            result.AddRange(dbFolder.ChildFiles.Select(f => new FolderItemDto(f.Name, f.Type)));

        return new FolderGetDto(path, result);
    }

    public Task<string> UpdateFolder(string oldPath, string newName)
    {
        throw new NotImplementedException();
    }
}
