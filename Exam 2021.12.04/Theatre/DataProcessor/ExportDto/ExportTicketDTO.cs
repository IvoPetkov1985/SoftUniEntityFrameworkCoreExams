using Newtonsoft.Json;

namespace Theatre.DataProcessor.ExportDto
{
    public class ExportTicketDTO
    {
        [JsonProperty(nameof(Price))]
        public decimal Price { get; set; }

        [JsonProperty(nameof(RowNumber))]
        public sbyte RowNumber { get; set; }
    }

    public class ExportTheatreDTO
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Halls))]
        public sbyte Halls { get; set; }

        [JsonProperty(nameof(TotalIncome))]
        public decimal TotalIncome { get; set; }

        [JsonProperty(nameof(Tickets))]
        public ExportTicketDTO[] Tickets { get; set; } = null!;
    }
}
