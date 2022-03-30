using CMSServer.Data.AuthRepo;
using CMSServer.Data.FileRepo;
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

    private IAuthRepository _authRepository;
    private IUserRepository _userRepository;
    private IFolderRepository _folderRepository;
    private IFileRepository _fileRepository;


    public IAuthRepository AuthRepository { get => _authRepository ??= new AuthRepository(this); }

    public IUserRepository UserRepository { get => _userRepository ??= new UserRepository(_mongoDb, this); }

    public IFolderRepository FolderRepository { get => _folderRepository ??= new FolderRepository(_mongoDb, _fileSystemManager); }

    public IFileRepository FileRepository { get => _fileRepository ??= new FileRepository(_mongoDb, this, _fileSystemManager); }
}
