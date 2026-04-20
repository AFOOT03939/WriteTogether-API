namespace WriteTogether.Features.Fragments
{
    public class FragmentsModel
    {
        public int FragmentId { get; set; }
        public int? StoryId { get; set; }
        public int? UserId { get; set; }
        public string? Content { get; set; }
        public string? OrderIndex { get; set; }
        public string? ImageUrl { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
