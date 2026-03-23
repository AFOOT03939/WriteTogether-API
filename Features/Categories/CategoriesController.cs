using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WriteTogether.Features.Categories;
using WriteTogether.Features.Stories;

namespace WriteTogether.Features.Categories
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {

        public readonly CategoriesService _catService;
        public readonly StoriesService _stService;
        public CategoriesController(CategoriesService catService, StoriesService stService)
        {
            _catService = catService;
            _stService = stService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            //Lista de objetos Categories
            IEnumerable<CategoriesModel> categories;

            try
            {
                categories = await _catService.Categories();

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(categories);
        }

        [HttpGet("stories")]
        public async Task<IActionResult> GetStoriesByCategory(int? categoryId)
        {

            //Lista de objetos Story
            IEnumerable<StoriesModel> stories;

            try
            {
                if (categoryId.HasValue)
                {
                    stories = await _catService.GetStoriesByCategory(categoryId.Value);
                }
                else
                {
                    stories = await _stService.GetAllStories(); 
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(stories);
        }
    }
}
