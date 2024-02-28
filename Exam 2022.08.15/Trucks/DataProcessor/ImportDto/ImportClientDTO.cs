using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Trucks.DataProcessor.ImportDto
{
    public class ImportClientDTO
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Nationality))]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        [JsonProperty(nameof(Type))]
        [Required]
        public string Type { get; set; } = null!;

        [JsonProperty(nameof(Trucks))]
        public int[] Trucks { get; set; } = null!;
    }
}

//•	Name – text with length [3, 40] (required)
//•	Nationality – text with length [2, 40] (required)
//•	Type – text (required)
//•	ClientsTrucks – collection of type ClientTruck
