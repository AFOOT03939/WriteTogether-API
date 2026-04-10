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

        [HttpPost]
        public async Task<IActionResult> CreateFragments(FragmentsModel fragment)
        {

            if (fragment == null)
                return BadRequest("Invalid fragment");

            int fragmentId;

            try
            {
                fragmentId = await _fragService.CreateFragments(fragment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(fragmentId);
        }

        [HttpDelete("{fragmentId}")]
        public async Task<IActionResult> DeleteFragments(int fragmentId)
        {

            if (fragmentId <= 0)
                return BadRequest("Invalid fragment");

            int rowFragment;

            try
            {
                rowFragment = await _fragService.DeleteFragments(fragmentId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(fragmentId);
        }
    }
}
