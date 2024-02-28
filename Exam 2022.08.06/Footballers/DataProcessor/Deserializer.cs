namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Coaches");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportCoachDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportCoachDTO[] coachDTOs = (ImportCoachDTO[])deserializer.Deserialize(reader);

            ICollection<Coach> coaches = new List<Coach>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportCoachDTO coachDTO in coachDTOs)
            {
                if (!IsValid(coachDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(coachDTO.Nationality))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = coachDTO.Name,
                    Nationality = coachDTO.Nationality
                };

                foreach (ImportFootballerDTO footballerDTO in coachDTO.Footballers)
                {
                    if (!IsValid(footballerDTO))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isStartDateValid = DateTime.TryParseExact(footballerDTO.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime contractStartDate);

                    if (!isStartDateValid)
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isEndDateValid = DateTime.TryParseExact(footballerDTO.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime contractEndDate);

                    if (!isEndDateValid)
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (contractStartDate > contractEndDate)
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = footballerDTO.Name,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate,
                        BestSkillType = (BestSkillType)footballerDTO.BestSkillType,
                        PositionType = (PositionType)footballerDTO.PositionType
                    };

                    coach.Footballers.Add(footballer);
                }

                coaches.Add(coach);
                builder.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.Coaches.AddRange(coaches);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            ImportTeamDTO[] teamDTOs = JsonConvert.DeserializeObject<ImportTeamDTO[]>(jsonString);

            ICollection<Team> teams = new List<Team>();

            StringBuilder builder = new StringBuilder();

            int[] footballerIds = context.Footballers.Select(f => f.Id).ToArray();

            foreach (ImportTeamDTO teamDTO in teamDTOs)
            {
                if (!IsValid(teamDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(teamDTO.Nationality))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                if (teamDTO.Trophies <= 0)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team()
                {
                    Name = teamDTO.Name,
                    Nationality = teamDTO.Nationality,
                    Trophies = teamDTO.Trophies,
                };

                foreach (int footballerId in teamDTO.Footballers.Distinct())
                {
                    if (!footballerIds.Contains(footballerId))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    team.TeamsFootballers.Add(new TeamFootballer()
                    {
                        Team = team,
                        FootballerId = footballerId
                    });
                }

                teams.Add(team);
                builder.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }

            context.Teams.AddRange(teams);
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
