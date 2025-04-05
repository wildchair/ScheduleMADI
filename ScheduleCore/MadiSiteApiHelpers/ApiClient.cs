namespace ScheduleCore.MadiSiteApiHelpers
{
    public class ApiClient
    {
        private readonly string _baseUrl;

        private readonly Fetcher _groupsFetcher;
        private readonly Fetcher _professorsFetcher;
        private readonly Fetcher _scheduleFetcher;
        private readonly Fetcher _weekFetcher;

        public ApiClient(string baseUrl)
        {
            _groupsFetcher = new(baseUrl + "tasks/task3,7_fastview.php");
            _professorsFetcher = new(baseUrl + "tasks/task8_prepview.php");
            _scheduleFetcher = new(baseUrl + "tasks/tableFiller.php");
            _weekFetcher = new (baseUrl + "calendar.php");
        }

        public async Task<string> FetchGroupsAsync(FormUrlEncodedContent httpContent, CancellationToken cancellationToken)
        {
            return await _groupsFetcher.FetchAsync(httpContent, cancellationToken);
        }

        public async Task<string> FetchProfessorsAsync(FormUrlEncodedContent httpContent, CancellationToken cancellationToken)
        {
            return await _professorsFetcher.FetchAsync(httpContent, cancellationToken);
        }

        public async Task<string> FetchScheduleAsync(FormUrlEncodedContent httpContent, CancellationToken cancellationToken)
        {
            return await _scheduleFetcher.FetchAsync(httpContent, cancellationToken);
        }

        public async Task<string> FetchWeekAsync(FormUrlEncodedContent httpContent, CancellationToken cancellationToken)
        {
            return await _weekFetcher.FetchAsync(httpContent, cancellationToken);
        }
    }
}