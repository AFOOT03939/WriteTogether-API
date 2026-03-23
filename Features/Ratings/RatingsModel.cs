namespace WriteTogether.Features.Ratings
{
    public class RatingsModel
    {
        public int RatingId { get; set; }
        public int? StoryId { get; set; }
        public int? UserId { get; set; }
        public int? Rating{ get; set; }
    }
}
