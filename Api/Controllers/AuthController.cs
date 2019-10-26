using Api.Dtos.Auth;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [HttpPost("register")]
        public IActionResult Register(UserToRegister userToRegister)
        {
            userToRegister.Username = userToRegister.Username.ToLower();
            var createdBool = _authRepository.registerUser(userToRegister);
            if(!createdBool){ return BadRequest("Ya existe Usuario"); }
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(UserToLogin userToLogin)
        {
            userToLogin.Username = userToLogin.Username.ToLower();
            var userLogged = _authRepository.loginUser(userToLogin);
            if(userLogged == null){ return Unauthorized(); }
            return Ok(userLogged); /* Returns Token */
        }
    }
}