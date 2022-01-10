using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class SellingOffersDTO
    {
        [JsonPropertyName("sellings")]
        public List<SellingOffer> sellingOffers = new();

        public SellingOffersDTO(List<SellingOffer> sellingOffers)
        {
            this.sellingOffers = sellingOffers ?? throw new ArgumentNullException(nameof(sellingOffers));
        }
    }
}
