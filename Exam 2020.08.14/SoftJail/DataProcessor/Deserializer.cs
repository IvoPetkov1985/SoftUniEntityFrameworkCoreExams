namespace SoftJail.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

        private const string SuccessfullyImportedPrisoner = "Imported {0} {1} years old";

        private const string SuccessfullyImportedOfficer = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            ImportDepartmentDTO[] departmentDTOs = JsonConvert.DeserializeObject<ImportDepartmentDTO[]>(jsonString);

            ICollection<Department> departments = new List<Department>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportDepartmentDTO departmentDTO in departmentDTOs)
            {
                if (!IsValid(departmentDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Department department = new Department();
                department.Name = departmentDTO.Name;

                bool areAllCellsValid = true;

                foreach (ImportCellDTO cellDTO in departmentDTO.Cells)
                {
                    if (!IsValid(cellDTO))
                    {
                        builder.AppendLine(ErrorMessage);
                        areAllCellsValid = false;
                        break;
                    }

                    Cell cell = new Cell();
                    cell.CellNumber = cellDTO.CellNumber;
                    cell.HasWindow = cellDTO.HasWindow;

                    department.Cells.Add(cell);
                }

                if (!areAllCellsValid)
                {
                    continue;
                }

                if (!department.Cells.Any())
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                departments.Add(department);
                builder.AppendLine(string.Format(SuccessfullyImportedDepartment, department.Name, department.Cells.Count));
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            ImportPrisonerDTO[] prisonerDTOs = JsonConvert.DeserializeObject<ImportPrisonerDTO[]>(jsonString);

            ICollection<Prisoner> prisoners = new List<Prisoner>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportPrisonerDTO prisonerDTO in prisonerDTOs)
            {
                if (!IsValid(prisonerDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                bool isInitialDateValid = DateTime.TryParseExact(prisonerDTO.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime initialDate);

                if (!isInitialDateValid)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? releaseDate = null;

                if (!string.IsNullOrEmpty(prisonerDTO.ReleaseDate))
                {
                    bool isReleaseDateValid = DateTime.TryParseExact(prisonerDTO.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime checkedReleaseDate);

                    if (!isReleaseDateValid)
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    releaseDate = checkedReleaseDate;
                }

                Prisoner prisoner = new Prisoner();
                prisoner.FullName = prisonerDTO.FullName;
                prisoner.Nickname = prisonerDTO.Nickname;
                prisoner.Age = prisonerDTO.Age;
                prisoner.IncarcerationDate = initialDate;
                prisoner.ReleaseDate = releaseDate;
                prisoner.Bail = prisonerDTO.Bail;
                prisoner.CellId = prisonerDTO.CellId;

                bool areAllMailsValid = true;

                foreach (ImportMailDTO mailDTO in prisonerDTO.Mails)
                {
                    if (!IsValid(mailDTO))
                    {
                        builder.AppendLine(ErrorMessage);
                        areAllMailsValid = false;
                        break;
                    }

                    Mail mail = new Mail();
                    mail.Description = mailDTO.Description;
                    mail.Sender = mailDTO.Sender;
                    mail.Address = mailDTO.Address;

                    prisoner.Mails.Add(mail);
                }

                if (!areAllMailsValid)
                {
                    continue;
                }

                prisoners.Add(prisoner);
                builder.AppendLine(string.Format(SuccessfullyImportedPrisoner, prisoner.FullName, prisoner.Age));
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Officers");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportOfficerDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportOfficerDTO[] officerDTOs = (ImportOfficerDTO[])deserializer.Deserialize(reader);

            ICollection<Officer> officers = new List<Officer>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportOfficerDTO officerDTO in officerDTOs)
            {
                if (!IsValid(officerDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                bool isPositionValid = Enum.TryParse<Position>(officerDTO.Position, out Position position);

                bool isWeaponValid = Enum.TryParse<Weapon>(officerDTO.Weapon, out Weapon weapon);

                if (!isPositionValid || !isWeaponValid)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Officer officer = new Officer();
                officer.FullName = officerDTO.FullName;
                officer.Salary = officerDTO.Salary;
                officer.Position = position;
                officer.Weapon = weapon;
                officer.DepartmentId = officerDTO.DepartmentId;

                foreach (ImportPrisonerXmlDTO prisonerXmlDTO in officerDTO.Prisoners)
                {
                    officer.OfficerPrisoners.Add(new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = prisonerXmlDTO.Id
                    });
                }

                officers.Add(officer);
                builder.AppendLine(string.Format(SuccessfullyImportedOfficer, officer.FullName, officer.OfficerPrisoners.Count));
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}
