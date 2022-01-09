using MonsterTradingCardsGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class Package : IJsonConvertable
    {
        [JsonPropertyName("id")]
        public Guid ID { get; private set; }
        [JsonPropertyName("buyer")]
        public Guid BuyerID { get; private set; }
        [JsonPropertyName("description")]
        public string Description { get; private set; }
        [JsonPropertyName("cards")]
        public List<Card> Cards {get; private set;}

        [JsonPropertyName("price")]
        public int Price {get;private set;}  

        [JsonPropertyName("timestamp")]
        public DateTime CreationDate { get;private set;}

        public Package(Guid buyerId)
        {
            ID = Guid.NewGuid();
            BuyerID = buyerId;
            Price = Program.GetPackageCreationMapper().PackagePrize;
            CreationDate = DateTime.Now;
            Description = Program.GetPackageCreationMapper().PackageDescription;
            Cards = CreateCards();
        }

        private List<Card> CreateCards()
        {
            List<Card> cards = new List<Card>();
            Random kind = new Random();
            Random element = new Random();

            Random rand = new Random();
            for (int i = 0; i < Program.GetPackageCreationMapper().PackageSize; i++)
            {
                int kindValue = kind.Next(0, 100);
                int elementValue = element.Next(0, 100);

                EKind selectedKind = Program.GetPackageCreationMapper().KindChances.OrderBy(pair => pair.Value).First(x => x.Value >= kindValue).Key;
                EElement selectedElement = Program.GetPackageCreationMapper().ElementChances.OrderBy(pair => pair.Value).First(x => x.Value >= elementValue).Key;
                EType type = EType.MONSTER;
                if (selectedKind == EKind.SPELL)
                {
                    type = EType.SPELL;
                }
                
                cards.Add(new Card(Guid.NewGuid(), BuyerID, ID, Program.GetPackageCreationMapper().PackageDescription, type, selectedKind, selectedElement, rand.Next(10,50)));
            } 
            return cards;
        }
    }
}
