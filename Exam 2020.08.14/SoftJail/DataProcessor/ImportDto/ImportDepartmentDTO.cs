using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentDTO
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Cells))]
        public ImportCellDTO[] Cells { get; set; } = null!;
    }
}

//•	Name – text with min length 3 and max length 25 (required)
//•	Cells - collection of type Cell
