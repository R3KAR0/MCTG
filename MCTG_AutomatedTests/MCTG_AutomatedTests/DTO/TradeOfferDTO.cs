using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
