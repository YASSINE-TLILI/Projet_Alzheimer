using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using frontend_razor.Models;
using Microsoft.Extensions.Configuration;

namespace frontend_razor.Services
{
    public class DetectionService : IDetectionService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public DetectionService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            var baseUrl = _config["Backend:BaseUrl"]
                       ?? throw new ApplicationException("Configuration 'Backend:BaseUrl' manquante");

            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<DetectionResponse?> AnalyzeImage(IFormFile file)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream())
            {
                Headers = {
                    ContentType = new MediaTypeHeaderValue(file.ContentType),
                    ContentLength = file.Length
                }
            };

            content.Add(fileContent, "file", file.FileName);

            var response = await _httpClient.PostAsync("/api/detection/analyze", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<DetectionResponse>(jsonResponse, options);
        }
    }
}