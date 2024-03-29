﻿using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Footballer")]
    public class ExportFootballerXmlDTO
    {
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        public string Position { get; set; } = null!;
    }
}
