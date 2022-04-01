using Microsoft.AspNetCore.Mvc;

namespace CMSServer.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        public UserController(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        [HttpGet("GetUser/{username}")]
        public async Task<ActionResult<User>> GetUser([FromRoute]string username)
        {
            try
            {
                return await _unitOfWork.UserRepository.GetUser(username);
            }
            catch(ResponseException ex)
            {
                return StatusCode(ex.Status, ex.Message);
            }
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult> AddUser([FromBody]User user)
        {
            try
            {
                await _unitOfWork.UserRepository.AddUser(user);
                return Ok("User added");
            }
            catch (ResponseException ex)
            {
                return StatusCode(ex.Status, ex.Message);
            }
        }
    }
}
