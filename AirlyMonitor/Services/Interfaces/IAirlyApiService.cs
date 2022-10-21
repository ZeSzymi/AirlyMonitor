using AirlyMonitor.Models.Database;
using AirlyMonitor.Models.QueryParams;

namespace AirlyMonitor.Services.Interface
{
    public interface IAirlyApiService
    {
        public Task<List<Installation>> GetNearestInstallationsAsync(GetInstallationsQueryParams queryParams);
        public Task<Installation> GetInstallationByIdAsync(int id);
    }
}
