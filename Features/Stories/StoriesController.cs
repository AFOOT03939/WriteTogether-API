using Microsoft.AspNetCore.Mvc;
using WriteTogether.Features.Categories;
using WriteTogether.Features.Tags;

namespace WriteTogether.Features.Stories
{
    [ApiController]
    [Route("api/stories")]
    public class StoriesController : ControllerBase
    {
        public readonly StoriesService _stService;
        public StoriesController(StoriesService stService)
        {
            _stService = stService;
        }

        [HttpGet]
        public async Task<IActionResult> getAllStories()
        {

            IEnumerable<StoriesModel> stories;

            try
            {
                stories = await _stService.GetAllStories();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Ok(stories);
            
        }

        [HttpPost("{storyId}")]
        public async Task<IActionResult> AddTagsToStory(int storyId, TagsDto content)
        {
            if (storyId <= 0)
                return BadRequest("Invalid story");

            if (content == null)
                return BadRequest("Invalid tag");

            try
            {
                await _stService.AddTagsToStory(storyId, content);

                return Ok("Tags added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStory(int storyId)
        {
            if (storyId <= 0)
                return BadRequest("Invalid story");

            int deletedStory;

            try
            {
                deletedStory = await _stService.DeleteStory(storyId);

                return Ok(deletedStory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{storyId}/tags/{tagId}")]
        public async Task<IActionResult> RemoveTagFromStory(int storyId, int tagId)
        {
            if (storyId <= 0 || tagId <= 0)
                return BadRequest("Invalid data");

            try
            {
                var success = await _stService.RemoveTagFromStory(storyId, tagId);

                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Error removing tag");
            }
        }

    }
}
