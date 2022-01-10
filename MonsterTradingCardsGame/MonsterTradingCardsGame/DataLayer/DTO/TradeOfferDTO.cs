using MonsterTradingCardsGame.Models;
using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class TradeOfferDTO
    {
        public TradeOfferDTO(Guid cardId, EType desiredType, int minDamage)
        {
            CardId = cardId;
            DesiredType = desiredType;
            MinDamage = minDamage;
        }

        [JsonPropertyName("cardid")]
        public Guid CardId { get; private set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("desiredType")]
        public EType DesiredType { get; private set; }
        [JsonPropertyName("minDamage")]
        public int MinDamage { get; private set; }
    }
}
