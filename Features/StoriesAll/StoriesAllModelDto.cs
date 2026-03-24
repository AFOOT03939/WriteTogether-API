namespace WriteTogether.Features.StoriesAll
{
    public class StoriesAllModelDto
    {
        public int Id { get; set; }

        public string? SummaryText { get; set; }

        public DateTime GeneratedAt { get; set; }

        public int Version { get; set; }
    }
}
