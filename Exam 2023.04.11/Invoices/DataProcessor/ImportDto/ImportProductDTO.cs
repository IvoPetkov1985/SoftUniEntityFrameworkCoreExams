using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductDTO
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(9)]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Price))]
        [Required]
        [Range(5.00, 1000.00)]
        public decimal Price { get; set; }

        [JsonProperty(nameof(CategoryType))]
        [Required]
        [Range(0, 4)]
        public int CategoryType { get; set; }

        [JsonProperty(nameof(Clients))]
        public int[] Clients { get; set; } = null!;
    }
}

//•	Name – text with length [9…30] (required)
//•	Price – decimal in range [5.00…1000.00] (required)
//•	CategoryType – enumeration of type CategoryType, with possible values (ADR, Filters, Lights, Others, Tyres) (required)
