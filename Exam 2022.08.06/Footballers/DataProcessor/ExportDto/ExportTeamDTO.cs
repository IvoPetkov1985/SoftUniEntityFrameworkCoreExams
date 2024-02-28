using Newtonsoft.Json;

namespace Footballers.DataProcessor.ExportDto
{
    public class ExportTeamDTO
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Footballers))]
        public ExportFootballerDTO[] Footballers { get; set; } = null!;
    }
}
