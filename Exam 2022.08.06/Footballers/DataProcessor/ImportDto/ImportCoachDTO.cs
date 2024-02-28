using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachDTO
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; } = null!;

        [XmlArray("Footballers")]
        [XmlArrayItem("Footballer")]
        public ImportFootballerDTO[] Footballers { get; set; } = null!;
    }
}

//•	Name – text with length [2, 40] (required)
//•	Nationality – text (required)
//•	Footballers – collection of type Footballer
