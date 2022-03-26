using Microsoft.AspNetCore.Mvc;

namespace CMSServer.Controllers;
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly UnitOfWork _unitOfWork;

    public AuthController(UnitOfWork unit)
    {
        _unitOfWork = unit;
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody]LoginDto creds)
    {
        try
        {
            return await _unitOfWork.AuthRepository.Login(creds);
        }
        catch (ResponseException ex)
        {
            return StatusCode(ex.Status, ex.Message);
        }
    }
}