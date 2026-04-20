using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WriteTogether.Features.StoriesMessages
{
    [ApiController]
    [Route("api/story-messages")]
    public class StoriesMessagesController : ControllerBase
    {
        private readonly StoriesMessagesService _service;

        public StoriesMessagesController(StoriesMessagesService service)
        {
            _service = service;
        }

        [HttpGet("story/{storyId}")]
        public async Task<IActionResult> GetByStory(int storyId)
        {
            if (storyId <= 0)
                return BadRequest("Invalid story");

            var result = await _service.GetByStory(storyId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StoriesMessagesModel message)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Invalid token");

            if (message == null || message.StoryId <= 0 || string.IsNullOrWhiteSpace(message.Message))
                return BadRequest("Invalid data");

            message.UserId = int.Parse(userId);

            var id = await _service.Create(message);

            return Ok(new { messageId = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StoriesMessagesModel message)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Invalid token");

            if (id <= 0 || message == null || string.IsNullOrWhiteSpace(message.Message))
                return BadRequest("Invalid data");

            message.MessageId = id;

            var success = await _service.Update(message);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Invalid token");

            if (id <= 0)
                return BadRequest("Invalid id");

            var success = await _service.Delete(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}