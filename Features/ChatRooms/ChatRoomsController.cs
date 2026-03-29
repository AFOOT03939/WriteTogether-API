using Microsoft.AspNetCore.Mvc;
using WriteTogether.Features.Categories;
using WriteTogether.Features.Fragments;

namespace WriteTogether.Features.ChatRooms
{
    [ApiController]
    [Route("api/chatrooms")]
    public class ChatRoomsController : ControllerBase
    {
        public readonly ChatRoomsService _crService;
        public ChatRoomsController(ChatRoomsService crService)
        {
            _crService = crService;
        }

        [HttpGet("{storyId}")]
        public async Task<IActionResult> GetStoriesMessages(int storyId)
        {
            IEnumerable<ChatRoomsModel> chatRooms;

            try
            {
                chatRooms = await _crService.GetChatRooms(storyId);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(chatRooms);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatRoom([FromBody] int storyId)
        {
            try
            {
                var newId = await _crService.CreateChatRoom(storyId);

                return Ok(new
                {
                    ChatRoomId = newId,
                    StoryId = storyId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{chatRoomId}")]
        public async Task<IActionResult> DeleteChatRoom(int chatRoomId)
        {
            try
            {
                var deleted = await _crService.DeleteChatRoom(chatRoomId);

                if (!deleted)
                    return NotFound("ChatRoom no encontrado");

                return Ok("Eliminado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("story/{storyId}")]
        public async Task<IActionResult> DeleteByStory(int storyId)
        {
            try
            {
                var rows = await _crService.DeleteChatRoomsByStory(storyId);

                return Ok(new
                {
                    Deleted = rows
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
