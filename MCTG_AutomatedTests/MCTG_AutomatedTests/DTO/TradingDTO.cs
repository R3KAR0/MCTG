using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class TradingDTO
    {
        public TradingDTO(Guid tradeid, Guid buyerid)
        {
            TradeId = tradeid;
            BuyerId = buyerid;
        }

        [JsonPropertyName("tradeid")]
        public Guid TradeId { get; private set; }

        [JsonPropertyName("buyerid")]
        public Guid BuyerId { get; private set; }
    }
}
