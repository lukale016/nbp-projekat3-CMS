namespace CMSServer.Data.AuthRepo;
public interface IAuthRepository
{
    Task<User> Login(LoginDto creds);
}
