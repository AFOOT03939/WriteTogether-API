using WriteTogether.Features.Ratings;

namespace WriteTogether.Features.Fragments
{
    public class FragmentsService
    {
        private readonly FragmentsRepository _repo;
        public FragmentsService(FragmentsRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<FragmentsModel>> GetFragmentsByUser(int userId)
        {
            var result = await _repo.GetFragmentsByUser(userId);

            return result;
        }

        public async Task<IEnumerable<FragmentsModel>> GetFragmentsByStory(int storyId)
        {
            var result = await _repo.GetFragmentsByStory(storyId);

            return result;
        }

        public async Task<int> CreateFragments(FragmentsModel fragment)
        {
            var result = await _repo.CreateFragments(fragment);

            if (result <= 0)
                return 0;

            return result;
        }
    }
}
