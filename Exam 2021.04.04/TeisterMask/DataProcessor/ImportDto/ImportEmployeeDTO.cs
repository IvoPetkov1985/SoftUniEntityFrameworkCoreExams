using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeeDTO
    {
        [JsonProperty(nameof(Username))]
        [Required]
        [RegularExpression(@"^[A-Za-z\d]{3,}$")]
        [MaxLength(40)]
        public string Username { get; set; } = null!;

        [JsonProperty(nameof(Email))]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [JsonProperty(nameof(Phone))]
        [Required]
        [RegularExpression(@"^\d{3}\-\d{3}\-\d{4}$")]
        public string Phone { get; set; } = null!;

        [JsonProperty(nameof(Tasks))]
        public int[] Tasks { get; set; } = null!;
    }
}

//•	Username – text with length [3, 40].Should contain only lower or upper case letters and/or digits. (required)
//•	Email – text (required). Validate it! There is attribute for this job.
//•	Phone – text. Consists only of three groups (separated by '-'), the first two consist of three digits and the last one – of 4 digits. (required)
//•	EmployeesTasks – collection of type EmployeeTask
