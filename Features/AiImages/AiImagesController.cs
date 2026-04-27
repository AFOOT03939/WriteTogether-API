using Microsoft.AspNetCore.Mvc;

namespace WriteTogether.Features.AiImages
{
    [ApiController]
    [Route("api/ai")]
    public class AiImagesController : ControllerBase
    {
        private readonly AiImagesService _gemini;

        public AiImagesController(AiImagesService gemini)
        {
            _gemini = gemini;
        }

        [HttpPost("generate-image")]
        public async Task<IActionResult> GenerateImage([FromBody] AiImagesModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt is required");

            try
            {
                var bytes = await _gemini.GenerateImage(request.Prompt);

                var fileName = $"{Guid.NewGuid()}.png";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                await System.IO.File.WriteAllBytesAsync(path, bytes);

                var url = $"/images/{fileName}";

                return Ok(new { imageUrl = url });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
