using AirlyMonitor.Models.Configuration;
using AirlyMonitor.Models.Constants;
using AirlyMonitor.Models.Database;
using AirlyMonitor.Models.QueryParams;
using AirlyMonitor.Services.Interface;
using Microsoft.Extensions.Options;

namespace AirlyMonitor.Services
{
    public class AirlyApiService : IAirlyApiService
    {
        private readonly IHttpService _httpService;
        private readonly AirlyApiOptions _options;

        public AirlyApiService(IHttpService httpService, IOptions<AirlyApiOptions> options)
        {
            _httpService = httpService;
            _options = options.Value;
        }

        public Task<List<Installation>> GetNearestInstallationsAsync(GetInstallationsQueryParams queryParams)
            => _httpService.Get<List<Installation>>($"{_options.Url}{AirlyApi.NearestInstallationsUrl(queryParams.Lat, queryParams.Lng, queryParams.MaxDistanceKM, queryParams.MaxResults)}");

        public Task<Installation> GetInstallationByIdAsync(int id)
            => _httpService.Get<Installation>($"{_options.Url}{AirlyApi.InstallationByIdUrl(id)}");
    }
}
