using System.Text;
using System.Text.Json;
using WriteTogether.Features.Fragments;
using WriteTogether.Features.StoriesAll;

namespace WriteTogether.Features.AiText
{
    public class AiTextService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;
        private readonly FragmentsService _fragmentsService;
        private readonly StoriesAllService _storiesAllService;

        public AiTextService(
        HttpClient http,
        IConfiguration config,
        FragmentsService fragmentsService,
        StoriesAllService storiesAllService)
        {
            _http = http;
            _config = config;
            _fragmentsService = fragmentsService;
            _storiesAllService = storiesAllService;
        }

        public async Task<string> GenerateRawText(string prompt)
        {
            var apiKey = _config["Gemini:ApiKey"];
            string systemInstruction = "Sin introducciones ni saludos, ni conclusiones. Solo el cuerpo del texto, si no hay información suficiente, trata de hacerlo con lo que puedas.";
            string finalPrompt = $"{systemInstruction}\n\nContenido:\n{prompt}";
            var requestBody = new { contents = new[] { new { parts = new[] { new { text = finalPrompt } } } } };
            var json = JsonSerializer.Serialize(requestBody);

            var response = await _http.PostAsync(
                 $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent?key={apiKey}",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);

            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? throw new Exception("AI returned empty text");
        }

        // Ahora tus otros métodos pueden usar el motor genérico
        public async Task<string> GenerateAndUpdate(int fragmentId, string prompt, int userId)
        {
            var generatedText = await GenerateRawText(prompt);
            await _fragmentsService.UpdateFragment(fragmentId, generatedText, userId);
            return generatedText;
        }

        public async Task<string> GenerateFullStoryAndSave(int storyId, string prompt, int userId)
        {
            var generatedText = await GenerateRawText(prompt);
            await _storiesAllService.CreateFullStoryByStory(storyId, new StoriesAllModelDto { SummaryText = generatedText });
            return generatedText;
        }
    }
}