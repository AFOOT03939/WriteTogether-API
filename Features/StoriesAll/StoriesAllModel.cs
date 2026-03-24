namespace WriteTogether.Features.StoriesAll
{
    public class StoriesAllModel
    {
        public int Id { get; set; }

        public int StoryId { get; set; }

        public string? SummaryText { get; set; }

        public DateTime GeneratedAt { get; set; }

        public int Version { get; set; }
    }
}
