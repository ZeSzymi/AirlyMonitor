namespace AirlyMonitor.Services.Interface
{
    public interface IHttpService
    {
        Task<T> Get<T>(string url) where T : class;
        Task<T> Post<T, U>(string url, U body) where T : class;
    }
}
