using MonsterTradingCardsGame.Models;
using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class DecksDTO
    {
        public DecksDTO()
        {
            Decks = new List<Deck>();
        }

        [JsonConstructor]
        public DecksDTO(List<Deck> decks)
        {
            Decks = decks;
        }

        [JsonPropertyName("decks")]
        public List<Deck> Decks { get; private set; }
    }
}
