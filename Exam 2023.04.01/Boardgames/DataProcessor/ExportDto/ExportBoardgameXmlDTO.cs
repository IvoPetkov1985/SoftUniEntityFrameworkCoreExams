using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Boardgame")]
    public class ExportBoardgameXmlDTO
    {
        [XmlElement(nameof(BoardgameName))]
        public string BoardgameName { get; set; } = null!;

        [XmlElement(nameof(BoardgameYearPublished))]
        public int BoardgameYearPublished { get; set; }
    }

    [XmlType("Creator")]
    public class ExportCreatorDTO
    {
        [XmlAttribute(nameof(BoardgamesCount))]
        public int BoardgamesCount { get; set; }

        [XmlElement(nameof(CreatorName))]
        public string CreatorName { get; set; } = null!;

        [XmlArray(nameof(Boardgames))]
        public ExportBoardgameXmlDTO[] Boardgames { get; set; } = null!;
    }
}
