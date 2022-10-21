namespace AirlyMonitor.Models.Constants
{
    public static class AirlyApi
    {
        public const string Installations = "installations";
        public const string Measurements = "measurements";
        public const string Nearest = "nearest";
        public const string Lattitude = "lat";
        public const string Longitude = "lng";
        public const string MaxDistance = "maxDistanceKM";
        public const string MaxResults = "maxResults";

        public static string NearestInstallationsUrl(double lat, double lng, int distance = 5, int results = 10)
            => $"{Installations}/{Nearest}?{Lattitude}={lat}&{Longitude}={lng}&{MaxDistance}={distance}&{MaxResults}={results}";
        public static string InstallationByIdUrl(int id)
            => $"{Installations}/{id}";
    }
}
