using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Message")]
    public class ExportMessageDTO
    {
        [XmlElement("Description")]
        public string Description { get; set; } = null!;
    }

    [XmlType("Prisoner")]
    public class ExportPrisonerXmlDTO
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; } = null!;

        [XmlArray("EncryptedMessages")]
        [XmlArrayItem("Message")]
        public ExportMessageDTO[] EncryptedMessages { get; set; } = null!;
    }
}
