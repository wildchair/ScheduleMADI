namespace ScheduleCore.MadiSiteApiHelpers
{
    public class Fetcher
    {
        public readonly string Endpoint;
        private readonly HttpClient _httpClient;

        public Fetcher(string endpoint)
        {
            Endpoint = endpoint;
            _httpClient = new();
        }

        public async Task<string> FetchAsync(FormUrlEncodedContent httpContent, CancellationToken cancellationToken)
        {
            var httpResponse = await SendAsync(httpContent, cancellationToken);
            return await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        }

        protected async Task<HttpResponseMessage> SendAsync(HttpContent content, CancellationToken cancellationToken)
        {
            return await _httpClient.PostAsync(Endpoint, content, cancellationToken);
        }
    }
}