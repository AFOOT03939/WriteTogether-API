using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WriteTogether.Features.Categories;

namespace WriteTogether.Features.Fragments
{
    [ApiController]
    [Route("api/fragments")]
    public class FragmentsController : ControllerBase
    {
        public readonly FragmentsService _fragService;
        public FragmentsController(FragmentsService fragService)
        {
            _fragService = fragService;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetFragmentsByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Token inválido o sin ID");

            IEnumerable<FragmentsModel> fragments;

            try
            {
                fragments = await _fragService.GetFragmentsByUser(int.Parse(userId));
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(fragments);
        }

        [HttpGet("stories/{storyId}")]
        public async Task<IActionResult> GetFragmentsByStory(int storyId)
        {

            if (storyId <= 0)
                return BadRequest("Invalid Story");

            IEnumerable<FragmentsModel> fragments;

            try
            {
                fragments = await _fragService.GetFragmentsByStory(storyId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(fragments);
        }

        [HttpPost("stories/{storyId}")]
        public async Task<IActionResult> CreateFragment(int storyId, [FromBody] FragmentsModel fragment)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Invalid token");

            if (fragment == null || string.IsNullOrWhiteSpace(fragment.Content))
                return BadRequest("Invalid fragment");

            try
            {
                fragment.StoryId = storyId;
                fragment.UserId = int.Parse(userId);

                var fragmentId = await _fragService.CreateFragment(fragment);

                return Ok(new { fragmentId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{fragmentId}")]
        public async Task<IActionResult> UpdateFragment(int fragmentId, [FromBody] FragmentsModel fragment)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Invalid token");

            if (fragmentId <= 0)
                return BadRequest("Invalid fragment id");

            if (string.IsNullOrWhiteSpace(fragment.Content))
                return BadRequest("Content is required");

            try
            {
                var success = await _fragService.UpdateFragment(
                    fragmentId,
                    fragment.Content,
                    int.Parse(userId)
                );

                return success ? Ok() : NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{fragmentId}")]
        public async Task<IActionResult> DeleteFragment(int fragmentId)
        {
            if (fragmentId <= 0)
                return BadRequest("Invalid fragment");

            try
            {
                var success = await _fragService.DeleteFragment(fragmentId);

                return success ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{fragmentId}/image")]
        public async Task<IActionResult> UploadFragmentImage(int fragmentId, IFormFile file)
        {
            if (fragmentId <= 0)
                return BadRequest("Invalid fragment");

            try
            {
                var imageUrl = await _fragService.UploadFragmentImage(fragmentId, file);

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
