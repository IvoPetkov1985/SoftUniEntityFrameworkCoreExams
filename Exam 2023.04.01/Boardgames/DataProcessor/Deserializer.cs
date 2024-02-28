namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            XmlRootAttribute xra = new XmlRootAttribute("Creators");

            XmlSerializer deserializer = new XmlSerializer(typeof(ImportCreatorDTO[]), xra);

            using StringReader reader = new StringReader(xmlString);

            ImportCreatorDTO[] creatorDTOs = (ImportCreatorDTO[])deserializer.Deserialize(reader);

            ICollection<Creator> creators = new List<Creator>();

            StringBuilder builder = new StringBuilder();

            foreach (ImportCreatorDTO creatorDTO in creatorDTOs)
            {
                if (!IsValid(creatorDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new Creator();
                creator.FirstName = creatorDTO.FirstName;
                creator.LastName = creatorDTO.LastName;

                foreach (ImportBoardgameDTO boardgameDTO in creatorDTO.Boardgames)
                {
                    if (!IsValid(boardgameDTO))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (string.IsNullOrEmpty(boardgameDTO.Name))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = new Boardgame();
                    boardgame.Name = boardgameDTO.Name;
                    boardgame.Rating = boardgameDTO.Rating;
                    boardgame.YearPublished = boardgameDTO.YearPublished;
                    boardgame.CategoryType = (CategoryType)boardgameDTO.CategoryType;
                    boardgame.Mechanics = boardgameDTO.Mechanics;

                    creator.Boardgames.Add(boardgame);
                }

                creators.Add(creator);
                builder.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
            }

            context.Creators.AddRange(creators);
            context.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            ImportSellerDTO[] sellerDTOs = JsonConvert.DeserializeObject<ImportSellerDTO[]>(jsonString);

            ICollection<Seller> sellers = new List<Seller>();

            StringBuilder builder = new StringBuilder();

            int[] validBoardgameIds = context.Boardgames.Select(b => b.Id).ToArray();

            foreach (ImportSellerDTO sellerDTO in sellerDTOs)
            {
                if (!IsValid(sellerDTO))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(sellerDTO.Country) ||
                    string.IsNullOrEmpty(sellerDTO.Website) ||
                    string.IsNullOrEmpty(sellerDTO.Address))
                {
                    builder.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller();
                seller.Name = sellerDTO.Name;
                seller.Address = sellerDTO.Address;
                seller.Country = sellerDTO.Country;
                seller.Website = sellerDTO.Website;

                foreach (int boardgameId in sellerDTO.Boardgames.Distinct())
                {
                    if (!validBoardgameIds.Contains(boardgameId))
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    seller.BoardgamesSellers.Add(new BoardgameSeller()
                    {
                        BoardgameId = boardgameId,
                        Seller = seller
                    });
                }

                sellers.Add(seller);
                builder.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }

            context.Sellers.AddRange(sellers);
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
