using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class CreateSellingOfferDTO
    {
        public CreateSellingOfferDTO(Guid cardid, int price)
        {
            CardId = cardid;
            Price = price;
        }

        [JsonPropertyName("cardid")]
        public Guid CardId { get; private set; }

        [JsonPropertyName("price")]
        public int Price { get; private set; }
    }
}
