using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class ImportPrisonerXmlDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }

    [XmlType("Officer")]
    public class ImportOfficerDTO
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FullName { get; set; } = null!;

        [XmlElement("Money")]
        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal Salary { get; set; }

        [XmlElement("Position")]
        [Required]
        public string Position { get; set; } = null!;

        [XmlElement("Weapon")]
        [Required]
        public string Weapon { get; set; } = null!;

        [XmlElement("DepartmentId")]
        [Required]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        [XmlArrayItem("Prisoner")]
        public ImportPrisonerXmlDTO[] Prisoners { get; set; } = null!;
    }
}

//•	FullName – text with min length 3 and max length 30 (required)
//•	Salary – decimal (non-negative, minimum value: 0) (required)
//•	Position – Position enumeration with possible values: "Overseer, Guard, Watcher, Labour"(required)
//•	Weapon – Weapon enumeration with possible values: "Knife, FlashPulse, ChainRifle, Pistol, Sniper"(required)
//•	DepartmentId – integer, foreign key (required)
