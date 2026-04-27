using WriteTogether.Features.Ratings;
using WriteTogether.Features.Tags;
using WriteTogether.Helpers.Cloudinary;

namespace WriteTogether.Features.Stories
{
    public class StoriesService
    {
        private readonly StoriesRepository _repo;
        private readonly IImageService _imageService;
        private readonly TagsRepository _tagsrepo;
        private readonly StoriesCollaboratorsRepository _collabRepo;
        public StoriesService(StoriesRepository repo, TagsRepository tagsrepo, StoriesCollaboratorsRepository collabRepo, IImageService imageService)
        {
            _repo = repo;
            _tagsrepo = tagsrepo;
            _collabRepo = collabRepo;
            _imageService = imageService;
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

            // valida si ya existe el nombre del tag
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

        public async Task<int> CreateStory(StoriesModelRequest story, int UserId)
        {
            story.UserId = UserId;

            var storyId = await _repo.CreateStory(story);

            if (storyId <= 0)
                throw new Exception("Error creating story");

            if (story.CategoryIds != null && story.CategoryIds.Any())
            {
                foreach (var categoryId in story.CategoryIds)
                {
                    await _repo.InsertStoryCategory(storyId, categoryId);
                }
            }

            return storyId;
        }

        public async Task<bool> UpdateStory(int storyId, StoriesModel updatedStory, int currentUserId)
        {
            var existingStory = await _repo.GetStoryById(storyId);

            if (existingStory == null)
                throw new Exception("Story not found");

            // Si no es owner, verificar si es colaborador
            if (existingStory.UserId != currentUserId)
            {
                var isCollaborator = await _collabRepo.IsCollaborator(storyId, currentUserId);

                if (!isCollaborator)
                    throw new UnauthorizedAccessException("You don't have permission to edit this story");
            }

            updatedStory.StoryId = storyId;

            var result = await _repo.UpdateStory(updatedStory);

            return result > 0;
        }

        public async Task<bool> UpdateStoryStatus(int storyId, string status, int currentUserId)
        {
            var existingStory = await _repo.GetStoryById(storyId);

            if (existingStory == null)
                throw new Exception("Story not found");

            // Si no es owner, verificar si es colaborador
            if (existingStory.UserId != currentUserId)
            {
                var isCollaborator = await _collabRepo.IsCollaborator(storyId, currentUserId);

                if (!isCollaborator)
                    throw new UnauthorizedAccessException("You don't have permission to edit this story");
            }

            var result = await _repo.UpdateStoryStatus(storyId, status);

            return result > 0;
        }

        public async Task<string> UploadStoryImage(int storyId, IFormFile file)
        {
            var story = await _repo.GetStoryById(storyId);

            if (story == null)
                throw new Exception("Story not found");

            var imageUrl = await _imageService.UploadImageAsync(file);

            await _repo.UpdateStoryImage(storyId, imageUrl);

            return imageUrl;
        }
    }
}
