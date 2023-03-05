namespace AirlyMonitor.Services.Interface
{
    public interface IHttpService
    {
        Task<T> Get<T>(string url, string token = null) where T : class;
        Task<T> Post<T, U>(string url, U body, string token = null) where T : class;
    }
}
