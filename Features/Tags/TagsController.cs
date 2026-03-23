using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
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

        [HttpPost]
        public async Task<IActionResult> CreateTags(string name)
        {
            int tags;

            if (name == null)
            {
                return BadRequest("Invalid Tag");
            }

            try
            {
                tags = await _tagService.CreateTags(name);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok($"Tags succesfully added: id={tags}");
        }
    }
}
