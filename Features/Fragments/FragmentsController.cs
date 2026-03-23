using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetFragmentsByUser(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid User");

            IEnumerable<FragmentsModel> fragments;

            try
            {
                fragments = await _fragService.GetFragmentsByUser(userId);
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
    }
}
