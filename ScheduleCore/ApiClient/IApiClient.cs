namespace ScheduleCore.ApiClient
{
    public interface IApiClient
    {
        Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? parameters = null);
        Task<T?> PostAsync<T>(string endpoint, object body);
        Task<T?> PutAsync<T>(string endpoint, object body);
        Task<bool> DeleteAsync(string endpoint);
    }
}
