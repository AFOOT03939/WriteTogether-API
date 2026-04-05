namespace WriteTogether.Features.Stories
{
    public class StoriesModel
    {
        public int StoryId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Visibility { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? AuthorName { get; set; }
        public int Rating { get; set; }
        public string? category { get; set; }
    }
}
