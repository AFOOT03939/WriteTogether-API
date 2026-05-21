using WriteTogether.Features.ChallengesProgress;
using WriteTogether.Features.ChallengesProgress.WriteTogether.Features.ChallengesProgress;
using WriteTogether.Features.Users;

namespace WriteTogether.Features.Challenges
{
    public class ChallengeEngineService
    {
        private readonly ChallengesRepository _challengeRepo;
        private readonly ChallengesProgressRepository _progressRepo;
        private readonly UsersRepository _usersRepo;

        public ChallengeEngineService(
            ChallengesRepository challengeRepo,
            ChallengesProgressRepository progressRepo,
            UsersRepository usersRepo
        )
        {
            _challengeRepo = challengeRepo;
            _progressRepo = progressRepo;
            _usersRepo = usersRepo;
        }

        public async Task UpdateProgress(
            int userId,
            string challengeType
        )
        {
            var challenges =
                await _challengeRepo
                .GetActiveChallengesByType(challengeType);

            foreach (var challenge in challenges)
            {
                var progress =
                    await _progressRepo.GetChallengeProgress(
                        userId,
                        challenge.ChallengeId
                    );

                // Si no existe progreso -> crearlo
                if (progress == null)
                {
                    var firstCompleted =
                        1 >= challenge.TargetValue;

                    await _progressRepo.CreateProgress(
                        new ChallengesProgressRequest
                        {
                            UserId = userId,
                            ChallengeId = challenge.ChallengeId,
                            CurrentProgress = 1
                        }
                    );

                    if (firstCompleted)
                    {
                        await _progressRepo.UpdateProgress(
                            userId,
                            challenge.ChallengeId,
                            1,
                            true
                        );

                        await _usersRepo.AddReputationPoints(
                            userId,
                            challenge.RewardPoints
                        );
                    }

                    continue;
                }

                // Si ya completó -> ignorar
                if (progress.Completed)
                    continue;

                var newProgress =
                    progress.CurrentProgress + 1;

                var completed =
                    newProgress >= challenge.TargetValue;

                await _progressRepo.UpdateProgress(
                    userId,
                    challenge.ChallengeId,
                    newProgress,
                    completed
                );

                if (completed)
                {
                    await _usersRepo.AddReputationPoints(
                        userId,
                        challenge.RewardPoints
                    );
                }
            }
        }
    }
}