namespace WriteTogether.Features.ChatMessages
{
    public class ChatMessageModel
    {
        public int ChatMessageId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
    }
}
