using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportFootballerDTO
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [XmlElement("ContractStartDate")]
        [Required]
        public string ContractStartDate { get; set; } = null!;

        [XmlElement("ContractEndDate")]
        [Required]
        public string ContractEndDate { get; set; } = null!;

        [XmlElement("PositionType")]
        [Required]
        [Range(0, 3)]
        public int PositionType { get; set; }

        [XmlElement("BestSkillType")]
        [Required]
        [Range(0, 4)]
        public int BestSkillType { get; set; }
    }
}

//•	Name – text with length [2, 40] (required)
//•	ContractStartDate – date and time (required)
//•	ContractEndDate – date and time (required)
//•	PositionType - enumeration of type PositionType, with possible values (Goalkeeper, Defender, Midfielder, Forward) (required)
//•	BestSkillType – enumeration of type BestSkillType, with possible values (Defence, Dribble, Pass, Shoot, Speed) (required)
