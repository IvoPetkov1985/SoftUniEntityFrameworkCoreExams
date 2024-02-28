using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDTO
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression(@"^[A-Za-z\d\.\ \-]{3,}$")]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Nationality))]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        [JsonProperty(nameof(Trophies))]
        [Required]
        public int Trophies { get; set; }

        [JsonProperty(nameof(Footballers))]
        public int[] Footballers { get; set; } = null!;
    }
}

//•	Name – text with length [3, 40].Should contain letters(lower and upper case), digits, spaces, a dot sign ('.') and a dash ('-'). (required)
//•	Nationality – text with length [2, 40] (required)
//•	Trophies – integer (required)
//•	TeamsFootballers – collection of type TeamFootballer
