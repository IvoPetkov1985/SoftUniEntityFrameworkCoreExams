using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Task")]
    public class ExportTaskXmlDTO
    {
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("Label")]
        public string Label { get; set; } = null!;
    }

    [XmlType("Project")]
    public class ExportProjectDTO
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set; } = null!;

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; } = null!;

        [XmlArray("Tasks")]
        [XmlArrayItem("Task")]
        public ExportTaskXmlDTO[] Tasks { get; set; } = null!;
    }
}
