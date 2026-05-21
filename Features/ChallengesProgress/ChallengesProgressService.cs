namespace WriteTogether.Features.ChallengesProgress
{
    namespace WriteTogether.Features.ChallengesProgress
    {
        public class ChallengesProgressService
        {
            private readonly ChallengesProgressRepository _repo;

            public ChallengesProgressService(
                ChallengesProgressRepository repo
            )
            {
                _repo = repo;
            }

            public async Task<IEnumerable<ChallengesProgressModel>>
                GetUserChallengesProgress(int userId)
            {
                return await _repo.GetUserChallengesProgress(userId);
            }

            public async Task<ChallengesProgressModel?>
                GetChallengeProgress(int userId, int challengeId)
            {
                return await _repo.GetChallengeProgress(
                    userId,
                    challengeId
                );
            }

            public async Task<int> CreateProgress(
                ChallengesProgressRequest progress
            )
            {
                return await _repo.CreateProgress(progress);
            }

            public async Task<bool> UpdateProgress(
                int userId,
                int challengeId,
                int currentProgress,
                bool completed
            )
            {
                var result = await _repo.UpdateProgress(
                    userId,
                    challengeId,
                    currentProgress,
                    completed
                );

                return result > 0;
            }

            public async Task<bool> DeleteProgress(int progressId)
            {
                var result = await _repo.DeleteProgress(progressId);

                return result > 0;
            }
        }
    }
}
