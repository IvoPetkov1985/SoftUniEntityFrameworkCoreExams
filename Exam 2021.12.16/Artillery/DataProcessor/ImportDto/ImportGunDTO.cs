using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Artillery.DataProcessor.ImportDto
{
    public class ImportGunDTO
    {
        [JsonProperty(nameof(ManufacturerId))]
        [Required]
        public int ManufacturerId { get; set; }

        [JsonProperty(nameof(GunWeight))]
        [Required]
        [Range(100, 1_350_000)]
        public int GunWeight { get; set; }

        [JsonProperty(nameof(BarrelLength))]
        [Required]
        [Range(2.00, 35.00)]
        public double BarrelLength { get; set; }

        [JsonProperty(nameof(NumberBuild))]
        public int? NumberBuild { get; set; }

        [JsonProperty(nameof(Range))]
        [Required]
        [Range(1, 100_000)]
        public int Range { get; set; }

        [JsonProperty(nameof(GunType))]
        [Required]
        public string GunType { get; set; } = null!;

        [JsonProperty(nameof(ShellId))]
        [Required]
        public int ShellId { get; set; }

        [JsonProperty(nameof(Countries))]
        public ImportCountryIdDTO[] Countries { get; set; } = null!;
    }

    public class ImportCountryIdDTO
    {
        [JsonProperty(nameof(Id))]
        public int Id { get; set; }
    }
}

//•	ManufacturerId – integer, foreign key (required)
//•	GunWeight– integer in range [100…1_350_000] (required)
//•	BarrelLength – double in range [2.00….35.00] (required)
//•	NumberBuild – integer
//•	Range – integer in range [1….100_000] (required)
//•	GunType – enumeration of GunType, with possible values (Howitzer, Mortar, FieldGun, AntiAircraftGun, MountainGun, AntiTankGun) (required)
//•	ShellId – integer, foreign key (required)
