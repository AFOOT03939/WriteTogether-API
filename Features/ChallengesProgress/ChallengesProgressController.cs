using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WriteTogether.Features.ChallengesProgress.WriteTogether.Features.ChallengesProgress;

namespace WriteTogether.Features.ChallengesProgress
{
    [ApiController]
    [Route("api/challenges-progress")]
    public class ChallengesProgressController : ControllerBase
    {
        private readonly ChallengesProgressService _service;

        public ChallengesProgressController(
            ChallengesProgressService service
        )
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProgress()
        {
            var userId = User.FindFirst(
                ClaimTypes.NameIdentifier
            )?.Value;

            if (userId == null)
                return Unauthorized();

            try
            {
                var progress =
                    await _service.GetUserChallengesProgress(
                        int.Parse(userId)
                    );

                return Ok(progress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{challengeId}")]
        public async Task<IActionResult> GetChallengeProgress(
            int challengeId
        )
        {
            var userId = User.FindFirst(
                ClaimTypes.NameIdentifier
            )?.Value;

            if (userId == null)
                return Unauthorized();

            try
            {
                var progress =
                    await _service.GetChallengeProgress(
                        int.Parse(userId),
                        challengeId
                    );

                return Ok(progress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProgress(
            [FromBody] ChallengesProgressRequest progress
        )
        {
            try
            {
                var progressId =
                    await _service.CreateProgress(progress);

                return Ok(new { progressId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{challengeId}")]
        public async Task<IActionResult> UpdateProgress(
            int challengeId,
            [FromBody] ChallengesProgressRequest progress
        )
        {
            try
            {
                var success = await _service.UpdateProgress(
                    progress.UserId,
                    challengeId,
                    progress.CurrentProgress,
                    false
                );

                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{progressId}")]
        public async Task<IActionResult> DeleteProgress(
            int progressId
        )
        {
            try
            {
                var success =
                    await _service.DeleteProgress(progressId);

                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}