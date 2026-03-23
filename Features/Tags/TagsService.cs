using WriteTogether.Features.Tags;

namespace WriteTogether.Features.Tags
{
    public class TagsService
    {
        private readonly TagsRepository _repo;
        public TagsService(TagsRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TagsModel>> GetTagsBystory(int storyId)
        {
            var result = await _repo.GetTagsByStory(storyId);

            return result;
        }
    }
}
