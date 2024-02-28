using Newtonsoft.Json;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportClientDTO
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Trucks))]
        public ExportTruckDTO[] Trucks { get; set; } = null!;
    }
}
