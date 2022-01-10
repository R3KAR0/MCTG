using MonsterTradingCardsGame.Models;
using System.Text.Json.Serialization;

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
