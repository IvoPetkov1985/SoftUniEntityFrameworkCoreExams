using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportMailDTO
    {
        [JsonProperty(nameof(Description))]
        [Required]
        public string Description { get; set; } = null!;

        [JsonProperty(nameof(Sender))]
        [Required]
        public string Sender { get; set; } = null!;

        [JsonProperty(nameof(Address))]
        [Required]
        [RegularExpression(@"^[A-Za-z0-9\ ]+str\.$")]
        public string Address { get; set; } = null!;
    }
}

//•	Description – text (required)
//•	Sender – text (required)
//•	Address – text, consisting only of letters, spaces and numbers, which ends with "str." (required) (Example: "62 Muir Hill str.")
