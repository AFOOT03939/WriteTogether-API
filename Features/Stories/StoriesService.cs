using WriteTogether.Features.Ratings;
using WriteTogether.Features.Tags;

namespace WriteTogether.Features.Stories
{
    public class StoriesService
    {
        private readonly StoriesRepository _repo;
        private readonly TagsRepository _tagsrepo;
        public StoriesService(StoriesRepository repo, TagsRepository tagsrepo)
        {
            _repo = repo;
            _tagsrepo = tagsrepo;
        }

        public async Task<IEnumerable<StoriesModel>> GetAllStories()
        {
            var stories = await _repo.GetAllStories();

            return stories;
        }

        public async Task<IEnumerable<StoriesModel>> GetStories(string? status, int? categoryId)
        {
            var stories = await _repo.GetStories(status, categoryId);

            return stories;
        }

        public async Task<bool> AddTagsToStory(int storyId, TagsDto content)
        {
            var tagId = await _tagsrepo.GetTagsByName(content.Content);

            bool success;

            //valida si ya existe el nombre del tag
            if(tagId == 0)
            {
                var tagIdCreated = await _tagsrepo.CreateTags(content.Content);

                if (tagIdCreated <= 0)
                    return false;

                success = await _repo.CreateStoryTag(storyId, tagIdCreated);
            }
            else
            {
                success = await _repo.CreateStoryTag(storyId, tagId);
            }

            return success;
        }

        public async Task<int> DeleteStory(int storyId)
        {
            var stories = await _repo.DeleteStory(storyId);

            return stories;
        }

        public async Task<bool> RemoveTagFromStory(int storyId, int tagId)
        {
            var result = await _repo.DeleteTagFromStory(storyId, tagId);

            return result > 0;
        }

        public async Task<StoriesModel?> GetStoryById(int storyId)
        {
            return await _repo.GetStoryById(storyId);
        }

        public async Task<StoriesModel?> GetStoryByUser(int userId)
        {
            return await _repo.GetStoryByUser(userId);
        }

        public async Task<bool> UpdateStoryImage(int storyId, string imageUrl)
        {
            var result = await _repo.UpdateStoryImage(storyId, imageUrl);
            return result > 0;
        }
    }
}
