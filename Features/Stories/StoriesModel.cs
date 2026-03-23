namespace WriteTogether.Features.Stories
{
    public class StoriesModel
    {
        public int StoryId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public string? Visibility { get; set; }
        public string? ImageUrl { get; set; }
    }
}
