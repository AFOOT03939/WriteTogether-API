using Microsoft.AspNetCore.Mvc;
using WriteTogether.Features.Fragments;

namespace WriteTogether.Features.StoriesAll
{
    [ApiController]
    [Route("api/storiesAll")]
    public class StoriesAllController : ControllerBase
    {
        public readonly StoriesAllService _stAllService;
        public StoriesAllController(StoriesAllService stAllService)
        {
            _stAllService = stAllService;
        }

        [HttpGet("{storyId}")]
        public async Task<IActionResult> GetFullStoryByStory(int storyId)
        {
            if (storyId <= 0)
                return BadRequest("Invalid story");

            IEnumerable<StoriesAllModel> story;

            try
            {
                story = await _stAllService.GetFullStoriesByStory(storyId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(story);
        }

        [HttpPost("{storyId}")]
        public async Task<IActionResult> CreateFullStoryByStory(int storyId, [FromBody] StoriesAllModelDto story)
        {
            if (storyId <= 0)
                return BadRequest("Invalid story");

            if (story == null)
                return BadRequest("Invalid content");

            int fullStory;

            try
            {
                fullStory = await _stAllService.CreateFullStoryByStory(storyId, story);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(story);
        }
    }
}
