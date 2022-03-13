using CMSServer.Data.UserRepo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CMSServer.Data;
public class UnitOfWork
{
    private readonly MongoClient _mongo;
    private IMongoDatabase _mongoDb;

    public UnitOfWork(IOptions<DbSettings> config)
    {
        _mongo = new MongoClient();
        _mongoDb = _mongo.GetDatabase(config.Value.DefaultDb);
    }

    private IUserRepository _userRepository;

    public IUserRepository UserRepository { get => _userRepository ??= new UserRepository(_mongoDb); }
}
