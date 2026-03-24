namespace WriteTogether.Features.StoriesMessages
{
    public class StoriesMessagesService
    {
        private readonly StoriesMessagesRepository _repo;

        public StoriesMessagesService(StoriesMessagesRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<StoriesMessagesModel>> GetByStory(int storyId)
        {
            return await _repo.GetByStory(storyId);
        }

        public async Task<int> Create(StoriesMessagesModel message)
        {
            return await _repo.Create(message);
        }

        public async Task<bool> Update(StoriesMessagesModel message)
        {
            var result = await _repo.Update(message);
            return result > 0;
        }

        public async Task<bool> Delete(int messageId)
        {
            var result = await _repo.Delete(messageId);
            return result > 0;
        }
    }
}