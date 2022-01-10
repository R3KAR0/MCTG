using MonsterTradingCardsGame.Models;
using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class AllTradeOfferDTO
    {
        public AllTradeOfferDTO(List<TradeOffer> tradeOffers)
        {
            TradeOffers = tradeOffers ?? throw new ArgumentNullException(nameof(tradeOffers));
        }

        [JsonPropertyName("tradeOffers")]
        public List<TradeOffer> TradeOffers { get; private set; }
    }
}
