namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            ExportCreatorDTO[] creators = context.Creators
                .Where(c => c.Boardgames.Any())
                .ToArray()
                .Select(c => new ExportCreatorDTO()
                {
                    CreatorName = c.FirstName + " " + c.LastName,
                    BoardgamesCount = c.Boardgames.Count,
                    Boardgames = c.Boardgames
                    .Select(b => new ExportBoardgameXmlDTO()
                    {
                        BoardgameName = b.Name,
                        BoardgameYearPublished = b.YearPublished,
                    })
                    .OrderBy(b => b.BoardgameName)
                    .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();

            XmlRootAttribute xra = new XmlRootAttribute("Creators");

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCreatorDTO[]), xra);

            XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
            xsn.Add(string.Empty, string.Empty);

            StringBuilder builder = new StringBuilder();

            using (StringWriter writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, creators, xsn);
            }

            return builder.ToString().TrimEnd();
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            ExportSellerDTO[] sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(b => b.Boardgame.YearPublished >= year && b.Boardgame.Rating <= rating))
                .ToArray()
                .Select(s => new ExportSellerDTO()
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                    .Where(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating)
                    .Select(b => new ExportBoardgameJsonDTO()
                    {
                        Name = b.Boardgame.Name,
                        Rating = b.Boardgame.Rating,
                        Mechanics = b.Boardgame.Mechanics,
                        Category = b.Boardgame.CategoryType.ToString()
                    })
                    .OrderByDescending(b => b.Rating)
                    .ThenBy(b => b.Name)
                    .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Length)
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            string result = JsonConvert.SerializeObject(sellers, Formatting.Indented);
            return result;
        }
    }
}
