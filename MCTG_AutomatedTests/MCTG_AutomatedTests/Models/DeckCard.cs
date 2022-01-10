using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class DeckCard
    {
        public DeckCard(Guid deckId, Guid cardId)
        {
            DeckId = deckId;
            CardId = cardId;
            Timestamp = DateTime.Now;
        }

        [JsonConstructor]
        public DeckCard(Guid deck, Guid card, DateTime timestamp)
        {
            DeckId = deck;
            CardId = card;
            Timestamp = timestamp;
        }

        [JsonPropertyName("deck")]
        public Guid DeckId { get; private set; }
        [JsonPropertyName("card")]
        public Guid CardId { get; private set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; private set; }
    }
}
