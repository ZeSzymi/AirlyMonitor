namespace AirlyMonitor.Models.Database
{
    public class Installation
    {
        public int Id { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public double Elevation { get; set; }
        public bool Airly { get; set; }
        public Guid SponsorId { get; set; }
        public Sponsor Sponsor { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location Location { get; set; }

    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Address
    {
        public Guid AddressId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string DisplayAddress1 { get; set; }
        public string DisplayAddress2 { get; set; }
    }

    public class Sponsor
    {
        public Guid SponsorId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
        public string DisplayName { get; set; }
    }
}
