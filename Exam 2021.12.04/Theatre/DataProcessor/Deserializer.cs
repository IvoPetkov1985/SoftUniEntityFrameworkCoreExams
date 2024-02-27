namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";


        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Plays");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportPlayDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportPlayDTO[] playDTOs = (ImportPlayDTO[])deserializer.Deserialize(reader);

            StringBuilder builder = new StringBuilder();

            ICollection<Play> plays = new List<Play>();

            foreach (ImportPlayDTO playDTO in playDTOs)
            {
                if (!IsValid(playDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                bool isDurationValid = TimeSpan.TryParseExact(playDTO.Duration, "c", CultureInfo.InvariantCulture, out TimeSpan duration);

                if (!isDurationValid)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                if (duration < TimeSpan.FromHours(1))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Genre genre;

                bool isGenreValid = Enum.TryParse<Genre>(playDTO.Genre, out genre);

                if (!isGenreValid)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play();
                play.Title = playDTO.Title;
                play.Duration = duration;
                play.Rating = playDTO.Rating;
                play.Genre = genre;
                play.Description = playDTO.Description;
                play.Screenwriter = playDTO.Screenwriter;

                plays.Add(play);
                builder.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();
            return builder.ToString();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Casts");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportCastDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportCastDTO[] castDTOs = (ImportCastDTO[])deserializer.Deserialize(reader);

            StringBuilder builder = new StringBuilder();

            ICollection<Cast> casts = new List<Cast>();

            foreach (ImportCastDTO castDTO in castDTOs)
            {
                if (!IsValid(castDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                bool isBooleanValid = bool.TryParse(castDTO.IsMainCharacter, out bool isMainCharacter);

                if (!isBooleanValid)
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast();
                cast.FullName = castDTO.FullName;
                cast.IsMainCharacter = isMainCharacter;
                cast.PhoneNumber = castDTO.PhoneNumber;
                cast.PlayId = castDTO.PlayId;

                casts.Add(cast);
                builder.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, cast.IsMainCharacter ? "main" : "lesser"));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            ImportTheatreDTO[] theatreDTOs = JsonConvert.DeserializeObject<ImportTheatreDTO[]>(jsonString);

            ICollection<Theatre> theatres = new List<Theatre>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportTheatreDTO theatreDTO in theatreDTOs)
            {
                if (!IsValid(theatreDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre();
                theatre.Name = theatreDTO.Name;
                theatre.NumberOfHalls = theatreDTO.NumberOfHalls;
                theatre.Director = theatreDTO.Director;

                foreach (ImportTicketDTO ticketDTO in theatreDTO.Tickets)
                {
                    if (!IsValid(ticketDTO))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = new Ticket();
                    ticket.Price = ticketDTO.Price;
                    ticket.RowNumber = ticketDTO.RowNumber;
                    ticket.PlayId = ticketDTO.PlayId;

                    theatre.Tickets.Add(ticket);
                }

                theatres.Add(theatre);
                builder.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }

            context.Theatres.AddRange(theatres);
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
