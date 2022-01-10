using System.Text.Json.Serialization;

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
