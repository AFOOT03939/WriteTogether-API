using WriteTogether.Features.Ratings;

namespace WriteTogether.Features.Stories
{
    public class StoriesService
    {
        private readonly StoriesRepository _repo;
        public StoriesService(StoriesRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<StoriesModel>> GetAllStories()
        {
            var stories = await _repo.GetAllStories();

            return stories;
        }
    }
}
