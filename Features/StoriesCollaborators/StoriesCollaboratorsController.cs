using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WriteTogether.Features.StoriesCollaborators
{
    [ApiController]
    [Route("api/stories/{storyId}/collaborators")]
    public class StoriesCollaboratorsController : ControllerBase
    {
        private readonly StoriesCollaboratorsService _service;

        public StoriesCollaboratorsController(StoriesCollaboratorsService service)
        {
            _service = service;
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinStory(int storyId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Invalid token");

            try
            {
                var result = await _service.JoinStory(storyId, int.Parse(userId));

                return Ok(new { joined = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("leave")]
        public async Task<IActionResult> LeaveStory(int storyId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Invalid token");

            try
            {
                var result = await _service.LeaveStory(storyId, int.Parse(userId));

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCollaborators(int storyId)
        {
            try
            {
                var collaborators = await _service.GetCollaborators(storyId);

                return Ok(collaborators);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("role")]
        public async Task<IActionResult> GetUserRole(int storyId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                var role = await _service.GetUserRole(storyId, int.Parse(userId));

                return Ok(new { role });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}