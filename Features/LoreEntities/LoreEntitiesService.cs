using WriteTogether.Features.AiText;
using WriteTogether.Features.StoriesAll;

namespace WriteTogether.Features.LoreEntities
{
    public class LoreEntitiesService
    {
        private readonly LoreEntitiesRepository _repo;
        private readonly StoriesAllService _storiesAllService;
        private readonly AiTextService _aiService;

        public LoreEntitiesService(
            LoreEntitiesRepository repo,
            StoriesAllService storiesAllService,
            AiTextService aiService)
        {
            _repo = repo;
            _storiesAllService = storiesAllService;
            _aiService = aiService;
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

        public async Task<LoreWikiResponse> GetFullWikiData(int storyId, string userPrompt)
        {
            // Obtener personajes
            var entities = await _repo.GetByStory(storyId);

            // Obtener historia completa 
            var storySegments = await _storiesAllService.GetFullStoriesByStory(storyId);

            // Corregimos el error del screenshot: 
            // Si tu modelo tiene SummaryText, usamos esa. 
            // Si lo que quieres es unir todos los fragmentos, asegúrate de estar llamando al servicio correcto.
            string fullContent = string.Join("\n\n", storySegments.Select(s => s.SummaryText));

            // Generar síntesis usando el nuevo método genérico
            string aiContext = $"{userPrompt}\n\nCONTENIDO DE LA HISTORIA:\n{fullContent}";
            string summary = await _aiService.GenerateRawText(aiContext);

            return new LoreWikiResponse
            {
                StorySummary = summary,
                Entities = entities
            };
        }
    }
}