using System.Text;
using System.Text.Json;
using WriteTogether.Features.Fragments;

namespace WriteTogether.Features.AiText
{
    public class AiTextService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;
        private readonly FragmentsService _fragmentsService;

        public AiTextService(
            HttpClient http,
            IConfiguration config,
            FragmentsService fragmentsService)
        {
            _http = http;
            _config = config;
            _fragmentsService = fragmentsService;
        }

        public async Task<string> GenerateAndUpdate(
            int fragmentId,
            string prompt,
            int userId)
        {
            var apiKey = _config["Gemini:ApiKey"];

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var response = await _http.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(content);

            var generatedText = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            await _fragmentsService.UpdateFragment(
                fragmentId,
                generatedText!,
                userId
            );

            return generatedText!;
        }
    }
}