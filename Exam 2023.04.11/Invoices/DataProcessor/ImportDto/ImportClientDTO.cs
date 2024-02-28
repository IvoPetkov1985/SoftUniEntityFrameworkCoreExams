using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ImportClientDTO
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(10)]
        [MaxLength(25)]
        public string Name { get; set; } = null!;

        [XmlElement("NumberVat")]
        [Required]
        [MinLength(10)]
        [MaxLength(15)]
        public string NumberVat { get; set; } = null!;

        [XmlArray("Addresses")]
        [XmlArrayItem("Address")]
        public ImportAddressDTO[] Addresses { get; set; } = null!;
    }
}

//•	Name – text with length [10…25] (required)
//•	NumberVat – text with length [10…15] (required)
//•	Addresses – collection of type Address
