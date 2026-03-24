using Microsoft.AspNetCore.Mvc;
using WriteTogether.Dapper;
using WriteTogether.Features.Fragments;
using WriteTogether.Features.StoriesAll;

namespace WriteTogether.Features.StoriesAll
{
    public class StoriesAllService
    {
        private readonly StoriesAllRepository _repo;
        public StoriesAllService(StoriesAllRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<StoriesAllModel>> GetFullStoriesByStory(int storyId)
        {
            var story = await _repo.GetFullStoriesByStory(storyId);

            return story;
        }

        public async Task<int> CreateFullStoryByStory(int storyId, StoriesAllModelDto story)
        {
            var fullStoryId = await _repo.CreateFullStoryByStory(storyId, story);

            return fullStoryId;
        }

    }
}
