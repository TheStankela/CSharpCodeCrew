using CSharpCodeCrew.Models;
using CSharpCodeCrew.Settings;
using Microsoft.Extensions.Options;

namespace CSharpCodeCrew.HttpClients
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
        public async Task<IEnumerable<TimeEntry>> GetTimeEntries() {
            var res = await _httpClient.GetFromJsonAsync<IEnumerable<TimeEntry>>($"gettimeentries?code={_options.Value.AppKey}")!;
            return res;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
