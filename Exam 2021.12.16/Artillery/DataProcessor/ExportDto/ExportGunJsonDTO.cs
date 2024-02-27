using Newtonsoft.Json;

namespace Artillery.DataProcessor.ExportDto
{
    public class ExportGunJsonDTO
    {
        [JsonProperty(nameof(GunType))]
        public string GunType { get; set; } = null!;

        [JsonProperty(nameof(GunWeight))]
        public int GunWeight { get; set; }

        [JsonProperty(nameof(BarrelLength))]
        public double BarrelLength { get; set; }

        [JsonProperty(nameof(Range))]
        public string Range { get; set; } = null!;
    }

    public class ExportShellDTO
    {
        [JsonProperty(nameof(ShellWeight))]
        public double ShellWeight { get; set; }

        [JsonProperty(nameof(Caliber))]
        public string Caliber { get; set; } = null!;
        
        public ExportGunJsonDTO[] Guns { get; set; } = null!;
    }
}
