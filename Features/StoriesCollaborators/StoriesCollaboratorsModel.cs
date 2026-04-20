namespace WriteTogether.Features.StoriesCollaborators
{
    public class StoriesCollaboratorsModel
    {
        public int StoryId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
