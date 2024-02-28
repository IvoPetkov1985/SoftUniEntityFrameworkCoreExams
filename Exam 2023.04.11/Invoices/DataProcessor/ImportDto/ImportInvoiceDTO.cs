using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDTO
    {
        [JsonProperty(nameof(Number))]
        [Required]
        [Range(1_000_000_000, 1_500_000_000)]
        public int Number { get; set; }

        [JsonProperty(nameof(IssueDate))]
        [Required]
        public string IssueDate { get; set; }

        [JsonProperty(nameof(DueDate))]
        [Required]
        public string DueDate { get; set; }

        [JsonProperty(nameof(Amount))]
        [Required]
        public decimal Amount { get; set; }

        [JsonProperty(nameof(CurrencyType))]
        [Required]
        [Range(0, 2)]
        public int CurrencyType { get; set; }

        [JsonProperty(nameof(ClientId))]
        [Required]
        public int ClientId { get; set; }
    }
}

//•	Number – integer in range  [1,000,000,000…1,500,000,000] (required)
//•	IssueDate – DateTime (required)
//•	DueDate – DateTime (required)
//•	Amount – decimal (required)
//•	CurrencyType – enumeration of type CurrencyType, with possible values (BGN, EUR, USD) (required)
//•	ClientId – integer, foreign key (required)
