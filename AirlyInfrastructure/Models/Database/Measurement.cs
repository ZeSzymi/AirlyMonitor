using Newtonsoft.Json;

namespace AirlyInfrastructure.Database
{
    public class Measurement
    {
        public Guid Id { get; set; }
        public int InstallationId { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public string Values { get; set; }

        public List<MeasurementValue> MeasurementValues 
        { 
            get => JsonConvert.DeserializeObject<List<MeasurementValue>>(Values); 
            set => Values = JsonConvert.SerializeObject(value); 
        }
    }

    public class MeasurementValue
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
