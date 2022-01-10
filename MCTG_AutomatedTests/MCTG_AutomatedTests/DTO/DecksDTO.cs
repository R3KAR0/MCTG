using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
