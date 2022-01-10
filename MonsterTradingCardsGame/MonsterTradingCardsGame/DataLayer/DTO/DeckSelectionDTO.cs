using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class DeckSelectionDTO
    {
        public DeckSelectionDTO(Guid deckId)
        {
            DeckId = deckId;
        }

        [JsonPropertyName("deckid")]
        public Guid DeckId { get; private set; }
    }
}
