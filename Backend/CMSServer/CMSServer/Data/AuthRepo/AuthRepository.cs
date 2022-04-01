namespace CMSServer.Data.AuthRepo;
public class AuthRepository : IAuthRepository
{
    private readonly UnitOfWork _unitOfWork;

    public AuthRepository(UnitOfWork unit)
    {
        _unitOfWork = unit;
    }

    public async Task<User> Login(LoginDto creds)
    {
        if (creds == null || string.IsNullOrWhiteSpace(creds.Username) || string.IsNullOrWhiteSpace(creds.Password))
            throw new ResponseException(400, "Credentials missing");

        User user = await _unitOfWork.UserRepository.GetUser(creds.Username);

        if (user.Password != creds.Password)
            throw new ResponseException(401, "Passwords do not match");

        return user;
    }
}