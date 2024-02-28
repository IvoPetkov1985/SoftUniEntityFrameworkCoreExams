using Newtonsoft.Json;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportTruckDTO
    {
        [JsonProperty(nameof(TruckRegistrationNumber))]
        public string TruckRegistrationNumber { get; set; } = null!;

        [JsonProperty(nameof(VinNumber))]
        public string VinNumber { get; set; } = null!;

        [JsonProperty(nameof(TankCapacity))]
        public int TankCapacity { get; set; }

        [JsonProperty(nameof(CargoCapacity))]
        public int CargoCapacity { get; set; }

        [JsonProperty(nameof(CategoryType))]
        public string CategoryType { get; set; } = null!;

        [JsonProperty(nameof(MakeType))]
        public string MakeType { get; set; } = null!;
    }
}
