using Newtonsoft.Json;

namespace Invoices.DataProcessor.ExportDto
{
    public class ExportProductDTO
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Price))]
        public decimal Price { get; set; }

        [JsonProperty(nameof(Category))]
        public string Category { get; set; } = null!;

        [JsonProperty(nameof(Clients))]
        public ExportClientDTO[] Clients { get; set; } = null!;
    }
}
