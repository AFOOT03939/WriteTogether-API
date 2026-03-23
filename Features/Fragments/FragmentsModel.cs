namespace WriteTogether.Features.Fragments
{
    public class FragmentsModel
    {
        public int FragmentsId { get; set; }
        public int? StoryId { get; set; }
        public int? AuthorId { get; set; }
        public string? Content { get; set; }
        public string? OrderIndex { get; set; }
        public string? ImageUrl { get; set; }

    }
}
