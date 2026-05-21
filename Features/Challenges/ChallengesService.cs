namespace WriteTogether.Features.Challenges
{
    namespace WriteTogether.Features.Challenges
    {
        public class ChallengesService
        {
            private readonly ChallengesRepository _repo;

            public ChallengesService(ChallengesRepository repo)
            {
                _repo = repo;
            }

            public async Task<IEnumerable<ChallengesModel>> GetAllChallenges()
            {
                return await _repo.GetAllChallenges();
            }

            public async Task<ChallengesModel?> GetChallengeById(int challengeId)
            {
                return await _repo.GetChallengeById(challengeId);
            }

            public async Task<int> CreateChallenge(ChallengesModelRequest challenge)
            {
                return await _repo.CreateChallenge(challenge);
            }

            public async Task<bool> UpdateChallenge(int challengeId, ChallengesModel challenge)
            {
                challenge.ChallengeId = challengeId;

                var result = await _repo.UpdateChallenge(challenge);

                return result > 0;
            }

            public async Task<bool> DeleteChallenge(int challengeId)
            {
                var result = await _repo.DeleteChallenge(challengeId);

                return result > 0;
            }
        }
    }
}
