using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class BuySellingOfferDTO
    {
        public BuySellingOfferDTO(Guid cardId)
        {
            CardId = cardId;
        }

        [JsonPropertyName("cardid")]
        public Guid CardId { get; private set; }
    }
}
