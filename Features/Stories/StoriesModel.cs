namespace WriteTogether.Features.Stories
{
    public class StoriesModel
    {
        public int StoryId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public string? Status { get; set; }
        public string? Visibility { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? AuthorName { get; set; }
        public int Rating { get; set; }
        public string? Categories { get; set; }     
        public string? CategoryIds { get; set; }     
    }

    public class StoriesModelRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public string? Status { get; set; }
        public string? Visibility { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? AuthorName { get; set; }
        public List<int> CategoryIds { get; set; } = new();
    }
}
