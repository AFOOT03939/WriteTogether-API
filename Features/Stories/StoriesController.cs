using Microsoft.AspNetCore.Mvc;
using WriteTogether.Features.Categories;
using WriteTogether.Features.Tags;

namespace WriteTogether.Features.Stories
{
    [ApiController]
    [Route("api/stories")]
    public class StoriesController : ControllerBase
    {
        public readonly StoriesService _stService;
        public StoriesController(StoriesService stService)
        {
            _stService = stService;
        }

        [HttpGet]
        public async Task<IActionResult> getAllStories()
        {

            IEnumerable<StoriesModel> stories;

            try
            {
                stories = await _stService.GetAllStories();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Ok(stories);
            
        }

        [HttpPost("{storyId}")]
        public async Task<IActionResult> AddTagsToStory(int storyId, TagsDto content)
        {
            if (storyId <= 0)
                return BadRequest("Invalid story");

            if (content == null)
                return BadRequest("Invalid tag");

            try
            {
                await _stService.AddTagsToStory(storyId, content);

                return Ok("Tags added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStory(int storyId)
        {
            if (storyId <= 0)
                return BadRequest("Invalid story");

            int deletedStory;

            try
            {
                deletedStory = await _stService.DeleteStory(storyId);

                return Ok(deletedStory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{storyId}/tags/{tagId}")]
        public async Task<IActionResult> RemoveTagFromStory(int storyId, int tagId)
        {
            if (storyId <= 0 || tagId <= 0)
                return BadRequest("Invalid data");

            try
            {
                var success = await _stService.RemoveTagFromStory(storyId, tagId);

                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Error removing tag");
            }
        }

        [HttpPost("{storyId}/image")]
        public async Task<IActionResult> UploadStoryImage(int storyId, IFormFile file)
        {
            if (storyId <= 0)
                return BadRequest("Invalid story");

            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            // 🔹 Obtener story actual
            var story = await _stService.GetStoryById(storyId);

            if (story == null)
                return NotFound();

            // 🔥 BORRAR imagen anterior
            if (!string.IsNullOrEmpty(story.ImageUrl))
            {
                var oldPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    story.ImageUrl.TrimStart('/')
                );

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            // 🔹 Validar extensión
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(ext))
                return BadRequest("Invalid file type");

            // 🔹 Generar nombre único
            var fileName = $"{Guid.NewGuid()}{ext}";

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            // 🔹 Guardar archivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/{fileName}";

            // 🔹 Guardar en DB
            await _stService.UpdateStoryImage(storyId, imageUrl);

            return Ok(new { imageUrl });
        }

    }
}
