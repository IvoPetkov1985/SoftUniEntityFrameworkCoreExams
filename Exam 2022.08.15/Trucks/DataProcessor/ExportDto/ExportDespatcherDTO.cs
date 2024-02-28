using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class ExportDespatcherDTO
    {
        [XmlAttribute(nameof(TrucksCount))]
        public int TrucksCount { get; set; }

        [XmlElement(nameof(DespatcherName))]
        public string DespatcherName { get; set; } = null!;

        [XmlArray(nameof(Trucks))]
        [XmlArrayItem("Truck")]
        public ExportTruckXmlDTO[] Trucks { get; set; } = null!;
    }
}
