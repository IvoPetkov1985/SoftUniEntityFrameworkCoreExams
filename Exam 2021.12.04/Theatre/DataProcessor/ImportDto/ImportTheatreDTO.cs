using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTheatreDTO
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(NumberOfHalls))]
        [Required]
        [Range(1, 10)]
        public sbyte NumberOfHalls { get; set; }

        [JsonProperty(nameof(Director))]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Director { get; set; } = null!;

        [JsonProperty(nameof(Tickets))]
        public ImportTicketDTO[] Tickets { get; set; } = null!;
    }
}

//•	Name – text with length [4, 30] (required)
//•	NumberOfHalls – sbyte between [1…10] (required)
//•	Director – text with length [4, 30] (required)
//•	Tickets – a collection of type Ticket
