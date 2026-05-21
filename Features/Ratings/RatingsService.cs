using WriteTogether.Features.Categories;
using WriteTogether.Features.Challenges;

namespace WriteTogether.Features.Ratings
{
    public class RatingsService
    {
        private readonly RatingsRepository _repo;
        private readonly ChallengeEngineService _challengeEngine;
        public RatingsService(
            RatingsRepository repo,
            ChallengeEngineService challengeEngine
        )
        {
            _repo = repo;
            _challengeEngine = challengeEngine;
        }

        public async Task<double?> GetRatingsByStory(int storiesId)
        {
            var ratings = await _repo.GetRatingsByStory(storiesId);

            var avg = ratings.Average(r => r.Rating);

            var average = avg.HasValue ? (int)Math.Ceiling(avg.Value) : 0;

            return average;
        }

        public async Task<int> GetRatingsByAuthor(int userId, int storiesId)
        {
            var rating = await _repo.GetRatingsByAuthor(userId, storiesId);

            if (rating == null)
                return 0;

            return rating.Rating ?? 0;
        }

        public async Task<int> CreateRatingsByAuthor(
            RatingsModel rating
        )
        {
            var result =
                await _repo.CreateRatingsByAuthor(
                    rating
                );

            if (result <= 0)
                return 0;

            if (rating.UserId.HasValue)
            {
                await _challengeEngine.UpdateProgress(
                    rating.UserId.Value,
                    "ratings"
                );
            }

            return result;
        }
    }
}
