using Microsoft.AspNetCore.Mvc;
using WriteTogether.Features.Categories;
using WriteTogether.Features.Ratings;

namespace WriteTogether.Features.Ratings
{
    [ApiController]
    [Route("api/ratings")]
    public class RatingsController : ControllerBase
    {
        public readonly RatingsService _ratService;
        public RatingsController(RatingsService ratService)
        {
            _ratService = ratService;
        }

        [HttpGet("stories/{storyId}")]
        public async Task<IActionResult> getRatingByStory(int storyId)
        {

            if(storyId <= 0)
            {
                return BadRequest("Invalid story");
            }

            double? ratings;

            try
            {
                ratings = await _ratService.GetRatingsByStory(storyId);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }

            return Ok(ratings);
        }

        [HttpGet("stories/{storyId}/users/{userId}")]
        public async Task<IActionResult> getRatingByStory(int userId, int storyId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user");
            }

            if (storyId <= 0)
            {
                return BadRequest("Invalid story");
            }

            int? rating;

            try
            {
                rating = await _ratService.GetRatingsByAuthor(userId, storyId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(rating);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRatingByStory(RatingsModel rating)
        {
            if (rating == null)
            {
                return BadRequest("Invalid data");
            }

            int userId;

            try
            {
                userId = await _ratService.CreateRatingsByAuthor(rating);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok($"Rating added succesfully: id={userId}");
        }
    }
}
