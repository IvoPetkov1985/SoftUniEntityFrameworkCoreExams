// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using System.Xml.Serialization;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Data.Models;
    using System.Text;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Projects");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportProjectDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportProjectDTO[] projectDTOs = (ImportProjectDTO[])deserializer.Deserialize(reader);

            ICollection<Project> projects = new List<Project>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportProjectDTO projectDTO in projectDTOs)
            {
                if (!IsValid(projectDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                bool isOpenDateValid = DateTime.TryParseExact(projectDTO.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime projectOpenDate);

                if (!isOpenDateValid)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? projectDueDate = null;

                if (!string.IsNullOrEmpty(projectDTO.DueDate))
                {
                    bool isProjectDueDateValid = DateTime.TryParseExact(projectDTO.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);

                    if (!isProjectDueDateValid)
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    projectDueDate = dueDate;
                }

                Project project = new Project();
                project.Name = projectDTO.Name;
                project.OpenDate = projectOpenDate;
                project.DueDate = projectDueDate;

                foreach (ImportTaskDTO taskDTO in projectDTO.Tasks)
                {
                    if (!IsValid(taskDTO))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isTaskOpenDateValid = DateTime.TryParseExact(taskDTO.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskOpenDate);

                    if (!isTaskOpenDateValid)
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isTaskDueDateValid = DateTime.TryParseExact(taskDTO.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDueDate);

                    if (!isTaskDueDateValid)
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskOpenDate < project.OpenDate ||
                        taskDueDate > project.DueDate)
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task();
                    task.Name = taskDTO.Name;
                    task.OpenDate = taskOpenDate;
                    task.DueDate = taskDueDate;
                    task.ExecutionType = (ExecutionType)taskDTO.ExecutionType;
                    task.LabelType = (LabelType)taskDTO.LabelType;

                    project.Tasks.Add(task);
                }

                projects.Add(project);
                builder.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            ImportEmployeeDTO[] employeeDTOs = JsonConvert.DeserializeObject<ImportEmployeeDTO[]>(jsonString);

            ICollection<Employee> employees = new List<Employee>();

            StringBuilder builder = new StringBuilder();

            int[] taskIds = context.Tasks.Select(t => t.Id).ToArray();

            foreach (ImportEmployeeDTO employeeDTO in employeeDTOs)
            {
                if (!IsValid(employeeDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee();
                employee.Username = employeeDTO.Username;
                employee.Email = employeeDTO.Email;
                employee.Phone = employeeDTO.Phone;

                foreach (int taskId in employeeDTO.Tasks.Distinct())
                {
                    if (!taskIds.Contains(taskId))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    employee.EmployeesTasks.Add(new EmployeeTask()
                    {
                        Employee = employee,
                        TaskId = taskId
                    });
                }

                employees.Add(employee);
                builder.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(employees);
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
