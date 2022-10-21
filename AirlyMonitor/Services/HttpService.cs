using AirlyMonitor.Models.Configuration;
using AirlyMonitor.Services.Interface;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace AirlyMonitor.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AirlyApiOptions _options;

        public HttpService(IHttpClientFactory httpClientFactory, IOptions<AirlyApiOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public async Task<T> Get<T>(string url) where T : class
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers =
                {
                    { "apikey", _options.ApiKey },
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);


            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var stringStream = await httpResponseMessage.Content.ReadAsStringAsync();
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<T>(contentStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return null;
        }

        public async Task<T> Post<T, U>(string url, U body) where T : class
        {
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Headers =
                {
                    { "apikey", _options.ApiKey },
                },
                Content = content
            };

           
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);


            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<T>(contentStream);
            }

            return null;
        }
    }
}
