using MongoDB.Driver;

namespace CMSServer.Data.UserRepo;
public class UserRepository : IUserRepository
{
    private IMongoDatabase _mongoDb;
    private IMongoCollection<User> _users;

    public UserRepository(IMongoDatabase db)
    {
        _mongoDb = db;
        _users = _mongoDb.GetCollection<User>(CollectionConsts.UserCollectionKey);
    }

    public async Task AddUser(User user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.Username))
            throw new ResponseException(400, "Passed arguments not valid");

        var filter = Builders<User>.Filter.Eq(nameof(user.Username), user.Username);

        User mongoUser = null;
        try
        {
            mongoUser = (await _users.FindAsync(filter)).SingleOrDefault();
        }
        catch (Exception ex)
        {
            throw new ResponseException(500, ex.Message);
        }

        if (mongoUser != null)
            throw new ResponseException(409, "User already exists");

        await _users.InsertOneAsync(user);
    }

    public Task DeleteUser(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ResponseException(400, "Username not provided");

        var filter = Builders<User>.Filter.Eq(nameof(User.Username), username);
        User user = null;
        try
        {
            user = (await _users.FindAsync<User>(filter)).SingleOrDefault();
        }
        catch (Exception ex)
        {
            throw new ResponseException(500, ex.Message);
        }

        if (user == null)
            throw new ResponseException(404, "User not found");

        return user;
    }

    public Task<User> UpdateUser(User user)
    {
        throw new NotImplementedException();
    }
}
