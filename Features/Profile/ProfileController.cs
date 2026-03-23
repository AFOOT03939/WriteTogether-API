using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using WriteTogether.Features.Register;

namespace WriteTogether.Features.Profile
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        public readonly ProfileService _profService;
        public ProfileController(ProfileService profService)
        {
            _profService = profService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }

            return Ok();
        }

    }
}
