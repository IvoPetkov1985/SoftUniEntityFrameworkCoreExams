namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";

        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Clients");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportClientDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportClientDTO[] clientDTOs = (ImportClientDTO[])deserializer.Deserialize(reader);

            ICollection<Client> clients = new List<Client>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportClientDTO clientDTO in clientDTOs)
            {
                if (!IsValid(clientDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client();
                client.Name = clientDTO.Name;
                client.NumberVat = clientDTO.NumberVat;

                foreach (ImportAddressDTO addressDTO in clientDTO.Addresses)
                {
                    if (!IsValid(addressDTO))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address address = new Address();
                    address.StreetName = addressDTO.StreetName;
                    address.StreetNumber = addressDTO.StreetNumber;
                    address.PostCode = addressDTO.PostCode;
                    address.City = addressDTO.City;
                    address.Country = addressDTO.Country;

                    client.Addresses.Add(address);
                }

                clients.Add(client);
                builder.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
            }

            context.Clients.AddRange(clients);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            ImportInvoiceDTO[] invoiceDTOs = JsonConvert.DeserializeObject<ImportInvoiceDTO[]>(jsonString);

            ICollection<Invoice> invoices = new List<Invoice>();

            StringBuilder builder = new StringBuilder();

            int[] clientIds = context.Clients.Select(c => c.Id).ToArray();

            foreach (ImportInvoiceDTO invoiceDTO in invoiceDTOs)
            {
                if (!IsValid(invoiceDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                bool isIssueDateValid = DateTime.TryParseExact(invoiceDTO.IssueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime issueDate);

                bool isDueDateValid = DateTime.TryParseExact(invoiceDTO.DueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);

                if (!isIssueDateValid || !isDueDateValid)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                if (dueDate < issueDate)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                if (!clientIds.Contains(invoiceDTO.ClientId))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Invoice invoice = new Invoice();
                invoice.Number = invoiceDTO.Number;
                invoice.IssueDate = issueDate;
                invoice.DueDate = dueDate;
                invoice.Amount = invoiceDTO.Amount;
                invoice.CurrencyType = (CurrencyType)invoiceDTO.CurrencyType;
                invoice.ClientId = invoiceDTO.ClientId;

                invoices.Add(invoice);
                builder.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
            }

            context.Invoices.AddRange(invoices);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            ImportProductDTO[] productDTOs = JsonConvert.DeserializeObject<ImportProductDTO[]>(jsonString);

            ICollection<Product> products = new List<Product>();

            StringBuilder builder = new StringBuilder();

            int[] clientIds = context.Clients.Select(c => c.Id).ToArray();

            foreach (ImportProductDTO productDTO in productDTOs)
            {
                if (!IsValid(productDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Product product = new Product();
                product.Name = productDTO.Name;
                product.Price = productDTO.Price;
                product.CategoryType = (CategoryType)productDTO.CategoryType;

                foreach (int id in productDTO.Clients.Distinct())
                {
                    if (!clientIds.Contains(id))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    product.ProductsClients.Add(new ProductClient()
                    {
                        Product = product,
                        ClientId = id
                    });
                }

                products.Add(product);
                builder.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));
            }

            context.Products.AddRange(products);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
