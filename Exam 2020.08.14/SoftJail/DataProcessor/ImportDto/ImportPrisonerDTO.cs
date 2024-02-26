using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerDTO
    {
        [JsonProperty(nameof(FullName))]
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FullName { get; set; } = null!;

        [JsonProperty(nameof(Nickname))]
        [Required]
        [RegularExpression(@"^The\ [A-Z][a-z]+$")]
        public string Nickname { get; set; } = null!;

        [JsonProperty(nameof(Age))]
        [Required]
        [Range(18, 65)]
        public int Age { get; set; }

        [JsonProperty(nameof(IncarcerationDate))]
        [Required]
        public string IncarcerationDate { get; set; } = null!;

        [JsonProperty(nameof(ReleaseDate))]
        public string? ReleaseDate { get; set; }

        [JsonProperty(nameof(Bail))]
        [Range(0, (double)decimal.MaxValue)]
        public decimal? Bail { get; set; }

        [JsonProperty(nameof(CellId))]
        public int? CellId { get; set; }

        [JsonProperty(nameof(Mails))]
        public ImportMailDTO[] Mails { get; set; } = null!;
    }
}

//•	FullName – text with min length 3 and max length 20 (required)
//•	Nickname – text starting with "The " and a single word only of letters with an uppercase letter for beginning(example: The Prisoner) (required)
//•	Age – integer in the range [18, 65] (required)
//•	IncarcerationDate ¬– Date (required)
//•	ReleaseDate – Date
//•	Bail – decimal (non-negative, minimum value: 0)
//•	CellId - integer, foreign key
//•	Cell – the prisoner's cell
//•	Mails – collection of type Mail
