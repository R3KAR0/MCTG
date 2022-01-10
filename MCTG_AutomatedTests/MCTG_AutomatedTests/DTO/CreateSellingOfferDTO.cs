using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class CreateSellingOfferDTO
    {
        public CreateSellingOfferDTO(Guid cardId, int price)
        {
            CardId = cardId;
            Price = price;
        }

        [JsonPropertyName("cardid")]
        public Guid CardId { get; private set; }

        [JsonPropertyName("price")]
        public int Price { get; private set; }
    }
}
