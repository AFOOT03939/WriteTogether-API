using WriteTogether.Features.StoriesAll;

namespace WriteTogether.Features.Audio
{
    public class AudioService
    {
        private readonly StoriesAllRepository _storiesRepo;
        private readonly AudioHelpers _audioHelper;

        public AudioService(
            StoriesAllRepository storiesRepo,
            AudioHelpers audioHelper)
        {
            _storiesRepo = storiesRepo;
            _audioHelper = audioHelper;
        }

        public async Task<MemoryStream> GenerateStoryAudio(
            int storyId)
        {
            var stories =
                await _storiesRepo
                    .GetFullStoriesByStory(storyId);

            var story =
                stories.FirstOrDefault();

            if (story == null)
                throw new Exception("Story not found");

            if (string.IsNullOrWhiteSpace(
                story.SummaryText))
            {
                throw new Exception(
                    "Story summary empty"
                );
            }

            return _audioHelper.GenerateAudio(
                story.SummaryText
            );
        }
    }
}