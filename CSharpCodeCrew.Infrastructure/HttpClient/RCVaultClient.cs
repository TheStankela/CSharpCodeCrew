using CSharpCodeCrew.Domain.Models;
using CSharpCodeCrew.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace CSharpCodeCrew.Application
{
    public interface IRCVaultClient
    {
        Task<IEnumerable<TimeEntry>> GetTimeEntries();
    }
    public class RCVaultClient : IRCVaultClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<RemoteApiSettings> _options;
        public RCVaultClient(HttpClient httpClient, IOptions<RemoteApiSettings> options)
        {
            _httpClient = httpClient;
            _options = options;
            _httpClient.BaseAddress = new Uri(_options.Value.ApiUrl);
        }
        public Task<IEnumerable<TimeEntry>> GetTimeEntries() =>
            _httpClient.GetFromJsonAsync<IEnumerable<TimeEntry>>($"gettimeentries?code={_options.Value.AppKey}")!;
        public void Dispose() =>
            _httpClient.Dispose();
    }
}
