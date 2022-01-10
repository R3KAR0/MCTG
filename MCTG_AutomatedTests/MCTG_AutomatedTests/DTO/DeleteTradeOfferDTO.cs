using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    internal class DeleteTradeOfferDTO
    {
        public DeleteTradeOfferDTO(Guid cardId)
        {
            CardId = cardId;
        }

        [JsonPropertyName("cardid")]
        public Guid CardId { get; private set; }
    }
}
