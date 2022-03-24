using CMSServer.Data.FolderRepo;
using CMSServer.Data.UserRepo;
using CMSServer.Services.FileSystemManager;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CMSServer.Data;
public class UnitOfWork
{
    private readonly MongoClient _mongo;
    private IMongoDatabase _mongoDb;
    private readonly IFileSystemManagerService _fileSystemManager;

    public UnitOfWork(IOptions<DbSettings> config, IFileSystemManagerService fsm)
    {
        _mongo = new MongoClient();
        _mongoDb = _mongo.GetDatabase(config.Value.DefaultDb);
        _fileSystemManager = fsm;
    }

    private IUserRepository _userRepository;
    private IFolderRepository _folderRepository;

    public IUserRepository UserRepository { get => _userRepository ??= new UserRepository(_mongoDb); }

    public IFolderRepository FolderRepository { get => _folderRepository ??= new FolderRepository(_mongoDb, _fileSystemManager); }
}
