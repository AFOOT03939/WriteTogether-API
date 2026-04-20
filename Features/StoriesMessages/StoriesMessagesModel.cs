namespace WriteTogether.Features.StoriesMessages
{
    public class StoriesMessagesModel
    {
        public int MessageId { get; set; }

        public int StoryId { get; set; }

        public int UserId { get; set; }

        public string? Message { get; set; }

        public string? ImageUrl { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
