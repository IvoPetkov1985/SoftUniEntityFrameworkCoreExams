using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ImportProjectDTO
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; } = null!;

        [XmlElement("DueDate")]
        public string? DueDate { get; set; }

        [XmlArray("Tasks")]
        [XmlArrayItem("Task")]
        public ImportTaskDTO[] Tasks { get; set; } = null!;
    }
}

//•	Name – text with length [2, 40] (required)
//•	OpenDate – date and time (required)
//•	DueDate – date and time (can be null)
//•	Tasks – collection of type Task