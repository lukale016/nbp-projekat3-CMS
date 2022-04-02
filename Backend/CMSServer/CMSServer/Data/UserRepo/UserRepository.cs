using MongoDB.Driver;

namespace CMSServer.Data.UserRepo;
public class UserRepository : IUserRepository
{
    private IMongoDatabase _mongoDb;
    private IMongoCollection<User> _users;
    private readonly UnitOfWork _unitOfWork;

    public UserRepository(IMongoDatabase db, UnitOfWork unit)
    {
        _mongoDb = db;
        _users = _mongoDb.GetCollection<User>(CollectionConsts.UserCollectionKey);
        _unitOfWork = unit;
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

        await _unitOfWork.FolderRepository.CreateRootFolderForUser(user.RootDir);
    }

    public async Task DeleteUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ResponseException(400, "Parameters not set");

        User user = await GetUser(username);

        await _unitOfWork.FolderRepository.DeleteFolder(user.RootDir);

        var filter = Builders<User>.Filter.Eq(nameof(User.Username), user.Username);
        await _users.DeleteOneAsync(filter);
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

    public async Task<User> UpdateUser(User user)
    {
        if (user is null || string.IsNullOrWhiteSpace(user.Username))
            throw new ResponseException(400, "Parameters not set");

        User existingUser = await GetUser(user.Username);
        
        existingUser.Name = string.IsNullOrWhiteSpace(user.Name) ? existingUser.Name : user.Name;
        existingUser.Surname = string.IsNullOrWhiteSpace(user.Surname) ? existingUser.Surname : user.Surname;
        existingUser.Password = string.IsNullOrWhiteSpace(user.Password) ? existingUser.Password : user.Password;

        var filter = Builders<User>.Filter.Eq(nameof(User.Username), existingUser.Username);
        var updateData = Builders<User>.Update.Set(nameof(User.Name), existingUser.Name)
                                              .Set(nameof(User.Surname), existingUser.Surname)
                                              .Set(nameof(User.Password), existingUser.Password);
        await _users.UpdateOneAsync(filter, updateData);

        return existingUser;
    }
}
