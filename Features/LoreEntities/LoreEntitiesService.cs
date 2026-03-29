namespace WriteTogether.Features.LoreEntities
{
    public class LoreEntitiesService
    {
        private readonly LoreEntitiesRepository _repo;

        public LoreEntitiesService(LoreEntitiesRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<LoreEntitiesModel>> GetByStory(int storyId)
        {
            if (storyId <= 0)
                throw new ArgumentException("Invalid StoryId");

            return await _repo.GetByStory(storyId);
        }

        public async Task<int> Create(LoreEntitiesModel model)
        {
            if (model.StoryId <= 0)
                throw new ArgumentException("Invalid StoryId");

            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentException("Name is required");

            return await _repo.Create(model);
        }

        public async Task<bool> Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Id");

            return await _repo.Delete(id);
        }
    }
}