namespace WriteTogether.Features.LoreEntities
{
    public class LoreEntitiesModel
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? Description { get; set; }
        public int? Importance { get; set; }
        public int? FirstFragmentId { get; set; }
    }
    public class LoreWikiResponse
    {
        public string StorySummary { get; set; } = string.Empty;
        public IEnumerable<LoreEntitiesModel> Entities { get; set; } = new List<LoreEntitiesModel>();
    }
}
