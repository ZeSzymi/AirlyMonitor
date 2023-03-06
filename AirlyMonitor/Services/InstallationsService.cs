using AirlyInfrastructure.Models.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Services.Interfaces;
using AirlyMonitor.Models.Database;
using AirlyMonitor.Models.Dtos;
using AirlyMonitor.Models.QueryParams;
using AirlyMonitor.Services.Interface;
using AirlyMonitor.Services.Interfaces;

namespace AirlyMonitor.Services
{
    public class InstallationsService : IInstallationsService
    {
        private readonly IInstallationsRepository _installationsRepository;
        private readonly IAirlyApiService _airlyApiService;
        private readonly IMeasurementRepository _measurementRepository;
        private readonly IMeasurementGenerationService _measurementGenerationService;

        public InstallationsService(
            IInstallationsRepository installationsRepository,
            IAirlyApiService airlyApiService,
            IMeasurementRepository measurementRepository,
            IMeasurementGenerationService measurementGenerationService)
        {
            _installationsRepository = installationsRepository;
            _airlyApiService = airlyApiService;
            _measurementRepository = measurementRepository;
            _measurementGenerationService = measurementGenerationService;
        }

        public async Task<InstallationDto> AddInstallationIfDoesNotExistAsync(int installationId)
        {
            var installation = await _installationsRepository.GetInstallationAsync(installationId);

            if (installation == null)
            {
                installation = await _airlyApiService.GetInstallationByIdAsync(installationId);
                await _installationsRepository.AddInstallationAsync(installation);
            }

            return new InstallationDto(installation);
        }

        public async Task<InstallationDto> MarkInstallationAsync(string userId, int installationId)
        {
            var installation = await AddInstallationIfDoesNotExistAsync(installationId);
            await AddUserInstallationIfDoesNotExistAsync(userId, installationId);

            return installation;
        }

        public async Task<UserInstallation> AddUserInstallationIfDoesNotExistAsync(string userId, int installationId)
        {
            var userInstallation = await _installationsRepository.GetUserInstallationAsync(userId, installationId);

            if (userInstallation == null)
            {
                userInstallation = new UserInstallation { InstallationId = installationId, UserId = userId };
                await _installationsRepository.AddUserInstallationAsync(userInstallation);
            }

            return userInstallation;
        }

        public async Task<List<InstallationDto>> GetNearestInstallationsAsync(GetInstallationsQueryParams queryParams)
        {
            var installations = await _airlyApiService.GetNearestInstallationsAsync(queryParams);
            return await GetInstallationsAsync(installations, queryParams.Lat, queryParams.Lng);
        }

        public async Task<List<InstallationDto>> GetUserInstallations(string userId)
        {
            var installations = await _installationsRepository.GetInstallationsForUserAsync(userId);
            return await GetInstallationsAsync(installations);
        }

        private async Task<List<InstallationDto>> GetInstallationsAsync(List<Installation> installations, double lat = 0.0, double lng = 0.0)
        {
            var installationIds = installations.Select(i => i.Id).ToList();
            var latestMeasurements = await _measurementRepository.GetLatestMeasurementsAsync(installationIds);
            var filteredInstallationIds = installationIds.Where(id => !latestMeasurements.Select(m => m.InstallationId).Contains(id)).ToList();
            var generatedMeasurements = _measurementGenerationService.GenerateMeasurementsForInstrumentsInArea(filteredInstallationIds);
            return installations.Select(installation => new InstallationDto(installation, latestMeasurements.Concat(generatedMeasurements).ToList())
            {
                DistanceToInstallationMeters = CalculateDistance(installation.Location, new Location
                {
                    Latitude = lat,
                    Longitude = lng
                })
            }).ToList();
        }

        public double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
