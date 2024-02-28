using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDTO
    {
        [XmlElement("FirstName")]
        [Required]
        [MinLength(2)]
        [MaxLength(7)]
        public string FirstName { get; set; } = null!;

        [XmlElement("LastName")]
        [Required]
        [MinLength(2)]
        [MaxLength(7)]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        [XmlArrayItem("Boardgame")]
        public ImportBoardgameDTO[] Boardgames { get; set; } = null!;
    }
}

//•	FirstName – text with length [2, 7] (required)
//•	LastName – text with length [2, 7] (required)
//•	Boardgames – collection of type Boardgame