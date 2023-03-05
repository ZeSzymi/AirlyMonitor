using AirlyMonitor.Models.Configuration;
using AirlyMonitor.Services.Interface;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace AirlyMonitor.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpService> _logger;
        private readonly AirlyApiOptions _options;

        public HttpService(IHttpClientFactory httpClientFactory, IOptions<AirlyApiOptions> options, ILogger<HttpService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<T> Get<T>(string url, string token = null) where T : class
        {
            _logger.LogInformation($"sending GET request to {url}");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers =
                {
                    { "apikey", _options.ApiKey },
                    { "Authorization", token }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);


            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var stringStream = await httpResponseMessage.Content.ReadAsStringAsync();
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();


                _logger.LogInformation($"message sent successfully");
                return await JsonSerializer.DeserializeAsync<T>(contentStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            _logger.LogError($"Error while sending message status code: {httpResponseMessage.StatusCode}");
            return null;
        }

        public async Task<T> Post<T, U>(string url, U body, string token = null) where T : class
        {
            _logger.LogInformation($"sending POST request to {url}");
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Headers =
                {
                    { "apikey", _options.ApiKey },
                    { "Authorization", token }
                },
                Content = content
            };


            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);


            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                _logger.LogInformation($"message sent successfully");
                try
                {
                    return await JsonSerializer.DeserializeAsync<T>(contentStream);
                }
                catch
                {
                    return null;
                }
            }

            _logger.LogError($"Error while sending message status code: {httpResponseMessage.StatusCode}");
            return null;
        }
    }
}
