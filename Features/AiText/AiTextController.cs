using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WriteTogether.Features.AiText
{
    [ApiController]
    [Route("api/ai-text")]
    public class AiTextController : ControllerBase
    {
        private readonly AiTextService _service;

        public AiTextController(AiTextService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] AiTextRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt is required");

            try
            {
                var result = await _service.GenerateAndUpdate(
                    request.FragmentId,
                    request.Prompt,
                    int.Parse(userId)
                );

                return Ok(new { content = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}