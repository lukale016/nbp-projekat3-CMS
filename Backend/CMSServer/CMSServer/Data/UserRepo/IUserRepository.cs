namespace CMSServer.Data.UserRepo;
public interface IUserRepository
{
    Task<User> GetUser(string username);
    Task AddUser(User user);
    Task<User> UpdateUser(User user);
    Task DeleteUser(string username);
}
