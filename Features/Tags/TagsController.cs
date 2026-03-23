using Microsoft.AspNetCore.Mvc;
using WriteTogether.Features.Categories;
using WriteTogether.Features.Stories;

namespace WriteTogether.Features.Tags
{
    [ApiController]
    [Route("api/tags")]
    public class TagsController : ControllerBase
    {
        public readonly TagsService _tagService;
        public TagsController(TagsService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTagsByStory(int storyId)
        {
            IEnumerable<TagsModel> tags;

            try
            {
                tags = await _tagService.GetTagsBystory(storyId);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(tags);
        }
    }
}
