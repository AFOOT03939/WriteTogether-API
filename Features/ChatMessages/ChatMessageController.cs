using Microsoft.AspNetCore.Mvc;
using WriteTogether.Features.Fragments;

namespace WriteTogether.Features.ChatMessages
{
    [ApiController]
    [Route("api/chatmessage")]
    public class ChatMessageController : ControllerBase
    {
        public readonly ChatMessageService _cmService;
        public ChatMessageController(ChatMessageService cmService)
        {
            _cmService = cmService;
        }
    

        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetMessagesByRoom(int roomId)
        {
            try
            {
                var messages = await _cmService.GetMessagesByRoom(roomId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            try
            {
                var message = await _cmService.GetMessageById(id);

                if (message == null)
                    return NotFound("Mensaje no encontrado");

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] ChatMessageModel model)
        {
            try
            {
                var id = await _cmService.CreateMessage(model);

                return Ok(new
                {
                    ChatMessageId = id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMessage([FromBody] ChatMessageModel model)
        {
            try
            {
                var updated = await _cmService.UpdateMessage(model);

                if (!updated)
                    return NotFound("Mensaje no encontrado");

                return Ok("Actualizado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                var deleted = await _cmService.DeleteMessage(id);

                if (!deleted)
                    return NotFound("Mensaje no encontrado");

                return Ok("Eliminado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("room/{roomId}")]
        public async Task<IActionResult> DeleteMessagesByRoom(int roomId)
        {
            try
            {
                var rows = await _cmService.DeleteMessagesByRoom(roomId);

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
