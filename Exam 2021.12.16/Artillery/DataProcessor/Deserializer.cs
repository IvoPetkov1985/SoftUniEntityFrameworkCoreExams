namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Countries");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportCountryDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportCountryDTO[]? countryDTOs = (ImportCountryDTO[]?)deserializer.Deserialize(reader);

            ICollection<Country> countries = new List<Country>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportCountryDTO countryDTO in countryDTOs)
            {
                if (!IsValid(countryDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country();
                country.CountryName = countryDTO.CountryName;
                country.ArmySize = countryDTO.ArmySize;

                countries.Add(country);
                builder.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return builder.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Manufacturers");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportManufacturerDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportManufacturerDTO[]? manufacturerDTOs = (ImportManufacturerDTO[]?)deserializer.Deserialize(reader);

            ICollection<Manufacturer> manufacturers = new List<Manufacturer>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportManufacturerDTO manufacturerDTO in manufacturerDTOs)
            {
                Manufacturer uniqueManufacturer = manufacturers.FirstOrDefault(m => m.ManufacturerName ==  manufacturerDTO.ManufacturerName);

                if (!IsValid(manufacturerDTO) || uniqueManufacturer != null)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer();
                manufacturer.ManufacturerName = manufacturerDTO.ManufacturerName;
                manufacturer.Founded = manufacturerDTO.Founded;

                string[] foundedDetails = manufacturer.Founded.Split(", ", StringSplitOptions.RemoveEmptyEntries);
                string townName = foundedDetails[^2];
                string countryName = foundedDetails[^1];
                string placeInfo = $"{townName}, {countryName}";

                manufacturers.Add(manufacturer);
                builder.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, placeInfo));
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return builder.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Shells");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportShellDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportShellDTO[]? shellDTOs = (ImportShellDTO[]?)deserializer.Deserialize(reader);

            ICollection<Shell> shells = new List<Shell>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportShellDTO shellDTO in shellDTOs)
            {
                if (!IsValid(shellDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell();
                shell.ShellWeight = shellDTO.ShellWeight;
                shell.Caliber = shellDTO.Caliber;

                shells.Add(shell);
                builder.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return builder.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            ImportGunDTO[]? gunDTOs = JsonConvert.DeserializeObject<ImportGunDTO[]>(jsonString);

            ICollection<Gun> guns = new List<Gun>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportGunDTO gunDTO in gunDTOs)
            {
                if (!IsValid(gunDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                bool isGunTypeValid = Enum.TryParse(gunDTO.GunType, out GunType gunType);

                if (!isGunTypeValid)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun();

                gun.ManufacturerId = gunDTO.ManufacturerId;
                gun.GunWeight = gunDTO.GunWeight;
                gun.BarrelLength = gunDTO.BarrelLength;
                gun.NumberBuild = gunDTO.NumberBuild;
                gun.Range = gunDTO.Range;
                gun.GunType = gunType;
                gun.ShellId = gunDTO.ShellId;

                foreach (ImportCountryIdDTO country in gunDTO.Countries)
                {
                    gun.CountriesGuns.Add(new CountryGun()
                    {
                        Gun = gun,
                        CountryId = country.Id
                    });
                }

                guns.Add(gun);
                builder.AppendLine(string.Format(SuccessfulImportGun, gun.GunType.ToString(), gun.GunWeight, gun.BarrelLength));
            }

            context.Guns.AddRange(guns);
            context.SaveChanges();

            return builder.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}