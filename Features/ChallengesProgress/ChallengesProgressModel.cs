namespace WriteTogether.Features.ChallengesProgress
{
    namespace WriteTogether.Features.ChallengesProgress
    {
        public class ChallengesProgressModel
        {
            public int ProgressId { get; set; }

            public int UserId { get; set; }

            public int ChallengeId { get; set; }

            public int CurrentProgress { get; set; }

            public bool Completed { get; set; }

            public DateTime? CompletedAt { get; set; }

            public DateTime? CreatedAt { get; set; }

            // Extras útiles para frontend
            public string? ChallengeTitle { get; set; }

            public int TargetValue { get; set; }

            public int RewardPoints { get; set; }

            public string? ChallengeType { get; set; }

            public bool Active { get; set; }
        }

        public class ChallengesProgressRequest
        {
            public int UserId { get; set; }

            public int ChallengeId { get; set; }

            public int CurrentProgress { get; set; }
        }
    }
}
