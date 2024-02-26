using Newtonsoft.Json;

namespace SoftJail.DataProcessor.ExportDto
{
    public class ExportOfficerDTO
    {
        [JsonProperty(nameof(OfficerName))]
        public string OfficerName { get; set; } = null!;

        [JsonProperty(nameof(Department))]
        public string Department { get; set; } = null!;
    }

    public class ExportPrisonerDTO
    {
        [JsonProperty(nameof(Id))]
        public int Id { get; set; }

        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(CellNumber))]
        public int? CellNumber { get; set; }

        [JsonProperty(nameof(Officers))]
        public ExportOfficerDTO[] Officers { get; set; } = null!;

        [JsonProperty(nameof(TotalOfficerSalary))]
        public decimal TotalOfficerSalary { get; set; }
    }
}
