using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class GetCoinsDTO
    {
        public GetCoinsDTO(int amount)
        {
            Amount = amount;
        }

        [JsonPropertyName("amount")]
        public int Amount { get; private set; }
    }
}
