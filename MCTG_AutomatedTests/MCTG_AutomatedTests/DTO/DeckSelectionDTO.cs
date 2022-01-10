using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
