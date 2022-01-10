using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class UserSelectedDeck
    {
        public UserSelectedDeck(Guid userId, Guid deckId)
        {
            UserId = userId;
            DeckId = deckId;
        }

        [JsonPropertyName("userid")]
        public Guid UserId { get; private set; }
        [JsonPropertyName("deckid")]
        public Guid DeckId { get; private set; }
    }
}
