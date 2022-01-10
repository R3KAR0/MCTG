using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class CardIdListDeckDTO
    {
        public CardIdListDeckDTO(List<Guid> cardIds, Guid deckId)
        {
            CardIds = cardIds ?? throw new ArgumentNullException(nameof(cardIds));
            DeckId = deckId;
        }

        [JsonPropertyName("card_ids")]
        public List<Guid> CardIds { get; private set; }

        [JsonPropertyName("deck_id")]
        public Guid DeckId { get; private set; }
    }
}
