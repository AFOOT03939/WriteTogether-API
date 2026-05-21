namespace WriteTogether.Features.Challenges
{
    public class ChallengesModel
    {
        public int ChallengeId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? ChallengeType { get; set; }

        public int TargetValue { get; set; }

        public int RewardPoints { get; set; }

        public bool Active { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class ChallengesModelRequest
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? ChallengeType { get; set; }

        public int TargetValue { get; set; }

        public int RewardPoints { get; set; }

        public bool Active { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}