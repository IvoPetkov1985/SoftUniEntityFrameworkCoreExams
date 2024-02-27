using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTicketDTO
    {
        [JsonProperty(nameof(Price))]
        [Required]
        [Range(1.00, 100.00)]
        public decimal Price { get; set; }

        [JsonProperty(nameof(RowNumber))]
        [Required]
        [Range(1, 10)]
        public sbyte RowNumber { get; set; }

        [JsonProperty(nameof(PlayId))]
        [Required]
        public int PlayId { get; set; }
    }
}

//•	Price – decimal in the range [1.00….100.00] (required)
//•	RowNumber – sbyte in range [1….10] (required)
//•	PlayId – integer, foreign key (required)
