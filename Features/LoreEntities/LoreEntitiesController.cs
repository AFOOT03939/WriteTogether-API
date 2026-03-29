using Microsoft.AspNetCore.Mvc;

namespace WriteTogether.Features.LoreEntities
{
    [ApiController]
    [Route("api/loreentities")]
    public class LoreEntitiesController : ControllerBase
    {
        private readonly LoreEntitiesService _leService;

        public LoreEntitiesController(LoreEntitiesService leService)
        {
            _leService = leService;
        }

        [HttpGet("story/{storyId}")]
        public async Task<IActionResult> GetByStory(int storyId)
        {
            try
            {
                var result = await _leService.GetByStory(storyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoreEntitiesModel model)
        {
            try
            {
                var id = await _leService.Create(model);

                return Ok(new
                {
                    Id = id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _leService.Delete(id);

                if (!deleted)
                    return NotFound("Entity not found");

                return Ok("Deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}