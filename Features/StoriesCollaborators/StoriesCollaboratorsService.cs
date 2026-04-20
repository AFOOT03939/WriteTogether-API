using WriteTogether.Features.Stories;

namespace WriteTogether.Features.StoriesCollaborators
{
    public class StoriesCollaboratorsService
    {
        private readonly StoriesCollaboratorsRepository _collabRepo;
        private readonly StoriesRepository _storiesRepo;

        public StoriesCollaboratorsService(
            StoriesCollaboratorsRepository collabRepo,
            StoriesRepository storiesRepo)
        {
            _collabRepo = collabRepo;
            _storiesRepo = storiesRepo;
        }

        public async Task<bool> JoinStory(int storyId, int currentUserId)
        {
            var story = await _storiesRepo.GetStoryById(storyId);

            if (story == null)
                throw new Exception("Story not found");

            if (story.UserId == currentUserId)
                throw new Exception("You are the creator");

            var alreadyCollaborator = await _collabRepo.IsCollaborator(storyId, currentUserId);

            if (alreadyCollaborator)
                throw new Exception("Already a collaborator");

            var result = await _collabRepo.AddCollaborator(storyId, currentUserId);

            return result > 0;
        }

        public async Task<bool> LeaveStory(int storyId, int currentUserId)
        {
            var story = await _storiesRepo.GetStoryById(storyId);

            if (story == null)
                throw new Exception("Story not found");

            if (story.UserId == currentUserId)
                throw new Exception("Creator cannot leave the story");

            var result = await _collabRepo.RemoveCollaborator(storyId, currentUserId);

            return result > 0;
        }

        public async Task<IEnumerable<StoriesCollaboratorsModel>> GetCollaborators(int storyId)
        {
            return await _collabRepo.GetCollaborators(storyId);
        }

        public async Task<string> GetUserRole(int storyId, int currentUserId)
        {
            var story = await _storiesRepo.GetStoryById(storyId);

            if (story == null)
                return "none";

            if (story.UserId == currentUserId)
                return "creator";

            var collaborator = await _collabRepo.GetCollaborator(storyId, currentUserId);

            if (collaborator != null)
                return "editor";

            return "viewer";
        }
    }
}