﻿using Newtonsoft.Json;

namespace Invoices.DataProcessor.ExportDto
{
    public class ExportClientDTO
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(NumberVat))]
        public string NumberVat { get; set; } = null!;
    }
}
