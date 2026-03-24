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
    }
}
