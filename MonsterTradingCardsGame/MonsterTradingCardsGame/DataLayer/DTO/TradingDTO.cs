using System.Text.Json.Serialization;

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
