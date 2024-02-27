using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class ImportTaskDTO
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
        [Required]
        public string DueDate { get; set; } = null!;

        [XmlElement("ExecutionType")]
        [Required]
        [Range(0, 3)]
        public int ExecutionType { get; set; }

        [XmlElement("LabelType")]
        [Required]
        [Range(0, 4)]
        public int LabelType { get; set; }
    }
}

//•	Name – text with length [2, 40] (required)
//•	OpenDate – date and time (required)
//•	DueDate – date and time (required)
//•	ExecutionType – enumeration of type ExecutionType, with possible values (ProductBacklog, SprintBacklog, InProgress, Finished) (required)
//•	LabelType – enumeration of type LabelType, with possible values (Priority, CSharpAdvanced, JavaAdvanced, EntityFramework, Hibernate) (required)
