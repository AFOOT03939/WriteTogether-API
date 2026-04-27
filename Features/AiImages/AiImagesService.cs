using System.Text;
using System.Text.Json;

namespace WriteTogether.Features.AiImages
{
    public class AiImagesService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public AiImagesService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<byte[]> GenerateImage(string prompt)
        {
            var apiKey = _config["Gemini:ApiKey"];

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new object[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    responseModalities = new[] { "IMAGE" },
                    temperature = 0.7
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var response = await _http.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-image:generateContent?key={apiKey}",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            var content = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(content);

            if (doc.RootElement.TryGetProperty("error", out var error))
            {
                var message = error.GetProperty("message").GetString();

                if (error.TryGetProperty("code", out var code) && code.GetInt32() == 429)
                {
                    throw new Exception("Image generation limit reached. Try again later.");
                }

                throw new Exception(message ?? "Unknown Gemini error");
            }

            if (!doc.RootElement.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
            {
                throw new Exception("No candidates returned from Gemini");
            }

            var parts = candidates[0]
                .GetProperty("content")
                .GetProperty("parts");

            foreach (var part in parts.EnumerateArray())
            {
                if (part.TryGetProperty("inlineData", out var inlineData))
                {
                    var base64 = inlineData.GetProperty("data").GetString();

                    if (string.IsNullOrEmpty(base64))
                        throw new Exception("Empty image data");

                    return Convert.FromBase64String(base64);
                }
            }

            throw new Exception("No image returned from Gemini");
        }
    }
}