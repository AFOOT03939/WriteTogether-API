using Microsoft.AspNetCore.Mvc;

namespace WriteTogether.Features.Audio
{
    [ApiController]
    [Route("api/audio")]
    public class AudioController : ControllerBase
    {
        private readonly AudioService _audioService;

        public AudioController(
            AudioService audioService)
        {
            _audioService = audioService;
        }

        [HttpGet("{storyId}")]
        public async Task<IActionResult> GenerateAudio(
            int storyId)
        {
            if (storyId <= 0)
                return BadRequest("Invalid story");

            try
            {
                var stream =
                    await _audioService
                        .GenerateStoryAudio(storyId);

                return File(
                    stream,
                    "audio/wav"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}