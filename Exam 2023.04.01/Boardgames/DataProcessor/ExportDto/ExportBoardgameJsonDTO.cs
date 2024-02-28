using Newtonsoft.Json;

namespace Boardgames.DataProcessor.ExportDto
{
    public class ExportBoardgameJsonDTO
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Rating))]
        public double Rating { get; set; }

        [JsonProperty(nameof(Mechanics))]
        public string Mechanics { get; set; } = null!;

        [JsonProperty(nameof(Category))]
        public string Category { get; set; } = null!;
    }

    public class ExportSellerDTO
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Website))]
        public string Website { get; set; } = null!;

        [JsonProperty(nameof(Boardgames))]
        public ExportBoardgameJsonDTO[] Boardgames { get; set; } = null!;
    }
}
