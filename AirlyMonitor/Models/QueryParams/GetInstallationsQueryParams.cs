namespace AirlyMonitor.Models.QueryParams
{
    public class GetInstallationsQueryParams
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int MaxDistanceKM { get; set; }
        public int MaxResults { get; set; }
    }
}
