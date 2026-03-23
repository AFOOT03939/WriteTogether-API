using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WriteTogether.Features.Register;

namespace WriteTogether.Features.Register
{
    [ApiController]
    [Route("api/register")]
    public class RegisterController : ControllerBase
    {
        public readonly RegisterService _regService;
        public RegisterController(RegisterService regService)
        {
            _regService = regService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel register)
        {

            if (register.UserName == null)
            {
                return BadRequest("Invalid Username");
            }

            if (register.Email == null)
            {
                return BadRequest("Invalid Email");
            }

            if (register.Password == null)
            {
                return BadRequest("Invalid Password");
            }

            RegisterModel user;

            try
            {
                //trae el id del User, si lo trae entonces está autenticado
                user = await _regService.RegisterUser(register);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok($"Succesfully Registered: id {user.UserId}");
        }

    }
}
