using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ScheduleCore.ApiClient
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient, IOptionsMonitor<ApiSettings> monitor)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(monitor.CurrentValue.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private string BuildUrl(string endpoint, Dictionary<string, string>? parameters)
        {
            if (parameters == null || !parameters.Any())
                return endpoint;

            var query = string.Join("&", parameters.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            return $"{endpoint}?{query}";
        }

        public async Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? parameters = null)
        {
            var url = BuildUrl(endpoint, parameters);
            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<T>(response);
        }

        public async Task<string> GetAsync(string endpoint, Dictionary<string, string>? parameters = null)
        {
            var url = BuildUrl(endpoint, parameters);
            var response = await _httpClient.GetAsync(url);
            return await HandleResponseRaw(response);
        }

        public async Task<T?> PostAsync<T>(string endpoint, object body)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, body);
            return await HandleResponse<T>(response);
        }

        public async Task<string> PostAsync(string endpoint, object body)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, body);
            return await HandleResponseRaw(response);
        }

        public async Task<T?> PutAsync<T>(string endpoint, object body)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, body);
            return await HandleResponse<T>(response);
        }

        public async Task<string> PutAsync(string endpoint, object body)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, body);
            return await HandleResponseRaw(response);
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }

        private async Task<T?> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error: {response.StatusCode}, Content: {content}");
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        private async Task<string> HandleResponseRaw(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Error: {response.StatusCode}, Content: {content}");

            return content;
        }
    }

    public class UniversityApiClient : ApiClient
    {
        public UniversityApiClient(HttpClient httpClient, IOptionsMonitor<UniversityApiSettings> monitor) : base(httpClient, monitor) { }
    }

    public class ScheduleApiClient : ApiClient
    {
        public ScheduleApiClient(HttpClient httpClient, IOptionsMonitor<ScheduleApiSettings> monitor) : base(httpClient, monitor) { }
    }
}
