using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespatcherDTO
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        public string? Position { get; set; }

        [XmlArray("Trucks")]
        [XmlArrayItem("Truck")]
        public ImportTruckDTO[] Trucks { get; set; } = null!;
    }
}

//•	Name – text with length [2, 40] (required)
//•	Position – text
//•	Trucks – collection of type Truck