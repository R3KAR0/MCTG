using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.Models
{
    public class Package
    {
        [JsonPropertyName("id")]
        public Guid Id { get; private set; }
        [JsonPropertyName("buyerid")]
        public Guid BuyerID { get; private set; }
        [JsonPropertyName("description")]
        public string Description { get; private set; }
        [JsonPropertyName("cards")]
        public List<Card> Cards {get; private set;}

        [JsonPropertyName("price")]
        public int Price {get;private set;}  

        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get;private set;}

        public Package(Guid buyerId)
        {
            var mapper = Program.GetPackageCreationMapper();
            if (mapper == null) throw new NullReferenceException();

            Id = Guid.NewGuid();
            BuyerID = buyerId;
            Price = mapper.PackagePrize;
            TimeStamp = DateTime.Now;
            Description = mapper.PackageDescription;
            Cards = CreateCards();
        }

        public Package(Guid id, Guid buyerid, string description, int price, DateTime timestamp)
        {
            Id = id;
            BuyerID = buyerid;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Cards = new();
            Price = price;
            TimeStamp = timestamp;
        }
        [JsonConstructor]
        public Package(Guid id, Guid buyerID, string description, List<Card> cards, int price, DateTime timestamp)
        {
            Id = id;
            BuyerID = buyerID;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Cards = cards ?? throw new ArgumentNullException(nameof(cards));
            Price = price;
            TimeStamp = timestamp;
        }

        private List<Card> CreateCards()
        {
            List<Card> cards = new List<Card>();
            Random kind = new Random();
            Random element = new Random();

            Random rand = new Random();
            var mapper = Program.GetPackageCreationMapper();
            if (mapper == null) throw new NullReferenceException();


            for (int i = 0; i < mapper.PackageSize; i++)
            {
                int kindValue = kind.Next(0, 100);
                int elementValue = element.Next(0, 100);

                EKind selectedKind = mapper.KindChances.OrderBy(pair => pair.Value).First(x => x.Value >= kindValue).Key;
                EElement selectedElement = mapper.ElementChances.OrderBy(pair => pair.Value).First(x => x.Value >= elementValue).Key;
                EType type = EType.MONSTER;
                if (selectedKind == EKind.SPELL)
                {
                    type = EType.SPELL;
                }

                cards.Add(new Card(Guid.NewGuid(), BuyerID, Id, mapper.PackageDescription, type, selectedKind, selectedElement, rand.Next(10,50)));
            } 
            return cards;
        }

        public bool SetDescription(string newDescription)
        {
            if (newDescription.Length > 128)
            {
                return false;
            }
            Description = newDescription;
            return true;
        }

        public void SetCards(List<Card> cards)
        {
            Cards = cards;
        }
    }
}
