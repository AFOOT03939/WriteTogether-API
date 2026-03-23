using WriteTogether.Features.Categories;
using WriteTogether.Features.Stories;

namespace WriteTogether.Features.Categories
{
    public class CategoriesService
    {
        private readonly CategoriesRepository _repo;
        public CategoriesService(CategoriesRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<CategoriesModel>> Categories()
        {
            var categories = await _repo.Categories();

            return categories;
        }

        public async Task<IEnumerable<StoriesModel>> GetStoriesByCategory(int? categoryId)
        {
            var stories = await _repo.GetStoriesByCategory(categoryId);

            return stories;
        }
    }
}
