using System.Text.Json.Serialization;

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
