using CSharpCodeCrew.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace CSharpCodeCrew.Application
{
    public interface ILocalApiClient
    {
        Task<Stream> GetPieChart(string jsonData);
    }
    public class LocalApiClient : ILocalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<LocalApiSettings> _options;
        public LocalApiClient(HttpClient httpClient, IOptions<LocalApiSettings> options)
        {
            _httpClient = httpClient;
            _options = options;
            _httpClient.BaseAddress = new Uri(_options.Value.ApiUrl);
        }
        public async Task<Stream> GetPieChart(string jsonData)
        {
            var res = await _httpClient.PostAsJsonAsync($"chart/pie", jsonData);
            return await res.Content.ReadAsStreamAsync();
        }
        public void Dispose() => _httpClient.Dispose();
    }
}
