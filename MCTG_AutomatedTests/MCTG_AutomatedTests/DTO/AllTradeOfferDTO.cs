using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
