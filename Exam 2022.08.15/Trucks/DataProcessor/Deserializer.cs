namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Despatchers");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportDespatcherDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportDespatcherDTO[] despatcherDTOs = (ImportDespatcherDTO[])deserializer.Deserialize(reader);

            ICollection<Despatcher> despatchers = new List<Despatcher>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportDespatcherDTO despatcherDTO in despatcherDTOs)
            {
                if (!IsValid(despatcherDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(despatcherDTO.Position))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = despatcherDTO.Name,
                    Position = despatcherDTO.Position,
                };

                foreach (ImportTruckDTO truckDTO in despatcherDTO.Trucks)
                {
                    if (!IsValid(truckDTO))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (string.IsNullOrEmpty(truckDTO.RegistrationNumber) ||
                        string.IsNullOrEmpty(truckDTO.VinNumber))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = new Truck()
                    {
                        RegistrationNumber = truckDTO.RegistrationNumber,
                        VinNumber = truckDTO.VinNumber,
                        TankCapacity = truckDTO.TankCapacity,
                        CargoCapacity = truckDTO.CargoCapacity,
                        CategoryType = (CategoryType)truckDTO.CategoryType,
                        MakeType = (MakeType)truckDTO.MakeType
                    };

                    despatcher.Trucks.Add(truck);
                }

                despatchers.Add(despatcher);
                builder.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));
            }

            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static string ImportClient(TrucksContext context, string jsonString)
        {
            ImportClientDTO[] clientDTOs = JsonConvert.DeserializeObject<ImportClientDTO[]>(jsonString);

            ICollection<Client> clients = new List<Client>();

            StringBuilder builder = new StringBuilder();

            int[] truckIds = context.Trucks.Select(t => t.Id).ToArray();

            foreach (ImportClientDTO clientDTO in clientDTOs)
            {
                if (!IsValid(clientDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(clientDTO.Nationality))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                if (clientDTO.Type == "usual")
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = clientDTO.Name,
                    Nationality = clientDTO.Nationality,
                    Type = clientDTO.Type
                };

                foreach (int truckId in clientDTO.Trucks.Distinct())
                {
                    if (!truckIds.Contains(truckId))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    client.ClientsTrucks.Add(new ClientTruck()
                    {
                        Client = client,
                        TruckId = truckId
                    });
                }

                clients.Add(client);
                builder.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }

            context.Clients.AddRange(clients);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}