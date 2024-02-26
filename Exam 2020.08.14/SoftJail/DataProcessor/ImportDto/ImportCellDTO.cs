using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportCellDTO
    {
        [JsonProperty(nameof(CellNumber))]
        [Required]
        [Range(1, 1000)]
        public int CellNumber { get; set; }

        [JsonProperty(nameof(HasWindow))]
        [Required]
        public bool HasWindow { get; set; }
    }
}

//•	CellNumber – integer in the range [1, 1000] (required)
//•	HasWindow – bool (required)
