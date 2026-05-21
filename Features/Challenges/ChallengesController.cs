namespace WriteTogether.Features.Challenges
{
    using Microsoft.AspNetCore.Mvc;

    namespace WriteTogether.Features.Challenges
    {
        [ApiController]
        [Route("api/challenges")]
        public class ChallengesController : ControllerBase
        {
            private readonly ChallengesService _service;

            public ChallengesController(ChallengesService service)
            {
                _service = service;
            }

            [HttpGet]
            public async Task<IActionResult> GetAllChallenges()
            {
                try
                {
                    var challenges = await _service.GetAllChallenges();

                    return Ok(challenges);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpGet("{challengeId}")]
            public async Task<IActionResult> GetChallengeById(int challengeId)
            {
                try
                {
                    var challenge = await _service.GetChallengeById(challengeId);

                    return Ok(challenge);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpPost]
            public async Task<IActionResult> CreateChallenge(
                [FromBody] ChallengesModelRequest challenge
            )
            {
                try
                {
                    var challengeId = await _service.CreateChallenge(challenge);

                    return Ok(new { challengeId });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpPut("{challengeId}")]
            public async Task<IActionResult> UpdateChallenge(
                int challengeId,
                [FromBody] ChallengesModel challenge
            )
            {
                try
                {
                    var success = await _service.UpdateChallenge(
                        challengeId,
                        challenge
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

            [HttpDelete("{challengeId}")]
            public async Task<IActionResult> DeleteChallenge(int challengeId)
            {
                try
                {
                    var success = await _service.DeleteChallenge(challengeId);

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
}
