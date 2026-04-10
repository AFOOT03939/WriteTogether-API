using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WriteTogether.Features.Fragments;

namespace WriteTogether.Features.Users
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        public readonly UsersService _userService;
        public UsersController(UsersService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Token inválido o sin ID");

            var user = await _userService.GetUserById(int.Parse(userId));

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Token inválido o sin ID");

            if (dto == null || string.IsNullOrWhiteSpace(dto.Username))
                return BadRequest("Invalid data");

            var success = await _userService.UpdateUser(int.Parse(userId), dto);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadUserImage(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Token inválido o sin ID");

            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            // Obtener usuario actual
            var user = await _userService.GetUserById(int.Parse(userId));

            if (user == null)
                return NotFound();

            // BORRAR imagen anterior si existe
            if (!string.IsNullOrEmpty(user.ImageUrl))
            {
                var oldPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    user.ImageUrl.TrimStart('/')
                );

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            // Generar nombre único
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            // Guardar nueva imagen
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/{fileName}";

            await _userService.UpdateUserImage(int.Parse(userId), imageUrl);

            return Ok(new { imageUrl });
        }
    }

}
