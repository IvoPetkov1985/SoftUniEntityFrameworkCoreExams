using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Country")]
    public class ExportCountryXmlDTO
    {
        [XmlAttribute(nameof(Country))]
        public string Country { get; set; } = null!;

        [XmlAttribute(nameof(ArmySize))]
        public int ArmySize { get; set; }
    }

    [XmlType("Gun")]
    public class ExportGunXmlDTO
    {
        [XmlAttribute(nameof(Manufacturer))]
        public string Manufacturer { get; set; } = null!;

        [XmlAttribute(nameof(GunType))]
        public string GunType { get; set; } = null!;

        [XmlAttribute(nameof(GunWeight))]
        public int GunWeight { get; set; }

        [XmlAttribute(nameof(BarrelLength))]
        public double BarrelLength { get; set; }

        [XmlAttribute(nameof(Range))]
        public int Range { get; set; }

        [XmlArray(nameof(Countries))]
        [XmlArrayItem("Country")]
        public ExportCountryXmlDTO[] Countries { get; set; } = null!;
    }
}
