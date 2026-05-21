using WriteTogether.Features.Challenges;
using WriteTogether.Features.Ratings;
using WriteTogether.Helpers.Cloudinary;

namespace WriteTogether.Features.Fragments
{
    public class FragmentsService
    {
        private readonly FragmentsRepository _repo;
        private readonly IImageService _imageService;
        private readonly ChallengeEngineService _challengeEngine;

        public FragmentsService(
            FragmentsRepository repo,
            IImageService imageService,
            ChallengeEngineService challengeEngine
        )
        {
            _repo = repo;
            _imageService = imageService;
            _challengeEngine = challengeEngine;
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

        public async Task<int> CreateFragment(
            FragmentsModel fragment
        )
        {
            if (fragment == null)
                throw new ArgumentNullException(
                    nameof(fragment)
                );

            if (string.IsNullOrWhiteSpace(
                fragment.Content
            ))
                throw new Exception(
                    "Content cannot be empty"
                );

            var result =
                await _repo.CreateFragment(fragment);

            if (result <= 0)
                throw new Exception(
                    "Error creating fragment"
                );

            if (fragment.UserId.HasValue)
            {
                await _challengeEngine.UpdateProgress(
                    fragment.UserId.Value,
                    "stories"
                );
            }

            return result;
        }

        public async Task<bool> UpdateFragment(int fragmentId, string content, int currentUserId)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new Exception("Content cannot be empty");

            var fragments = await _repo.GetFragmentsByUser(currentUserId);
            var fragment = fragments.FirstOrDefault(f => f.FragmentId == fragmentId);

            if (fragment == null)
                throw new UnauthorizedAccessException("You can't edit this fragment");

            var result = await _repo.UpdateFragment(fragmentId, content);

            return result > 0;
        }

        public async Task<bool> DeleteFragment(int fragmentId)
        {
            var result = await _repo.DeleteFragments(fragmentId);

            return result > 0;
        }

        public async Task<string> UploadFragmentImage(int fragmentId, IFormFile file)
        {

            var imageUrl = await _imageService.UploadImageAsync(file);

            await _repo.UpdateFragmentImage(fragmentId, imageUrl);

            return imageUrl;
        }
    }
}

