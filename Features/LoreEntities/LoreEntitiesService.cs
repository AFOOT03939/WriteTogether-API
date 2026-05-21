using System.Text.Json;
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

        // LoreEntitiesService.cs
        public async Task<LoreWikiResponse> GetFullWikiData(int storyId, string userPrompt)
        {
            var storySegments = await _storiesAllService.GetFullStoriesByStory(storyId);
            string fullContent = string.Join("\n\n", storySegments.Select(s => s.SummaryText));

            // Cambiamos "summary" por "StorySummary" en el prompt para que coincida con tu clase C#
            string masterPrompt = $@"Analiza la siguiente historia y genera:
    1. Un resumen formal.
    2. Una lista de entidades.
    Responde ESTRICTAMENTE en este formato JSON:
    {{
      ""StorySummary"": ""texto del resumen"",
      ""Entities"": [
        {{ ""Name"": ""Nombre"", ""Description"": ""Lore"", ""type"": ""character | place | event"" }}
      ]
    }}
    Historia: {fullContent}";

            string rawJson = await _aiService.GenerateRawText(masterPrompt);
            rawJson = rawJson.Replace("```json", "").Replace("```", "").Trim();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var wikiResult = JsonSerializer.Deserialize<LoreWikiResponse>(rawJson, options);

            return new LoreWikiResponse
            {
                StorySummary = wikiResult?.StorySummary ?? "No summary generated.",
                Entities = wikiResult?.Entities ?? new List<LoreEntitiesModel>()
            };
        }
    }
}