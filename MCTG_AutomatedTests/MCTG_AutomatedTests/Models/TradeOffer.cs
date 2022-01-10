using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class TradeOffer
    {

        public TradeOffer(Guid cardId, Guid seller, EType desiredType, int minDamage)
        {
            CardId = cardId;
            SellerId = seller;
            Timestamp = DateTime.Now;
            DesiredType = desiredType;
            MinDamage = minDamage;
        }

        public TradeOffer(Guid cardId, Guid seller, DateTime creationDate, EType desiredType, int minDamage)
        {
            CardId = cardId;
            SellerId = seller;
            Timestamp = creationDate;
            DesiredType = desiredType;
            MinDamage = minDamage;
        }

        [JsonConstructor]
        public TradeOffer(Guid cardid, Guid sellerid, Card? card, DateTime timestamp, EType desiredType, int minDamage)
        {
            CardId = cardid;
            SellerId = sellerid;
            Card = card;
            Timestamp = timestamp;
            DesiredType = desiredType;
            MinDamage = minDamage;
        }
        [JsonPropertyName("cardid")]
        public Guid CardId { get; private set; }
        [JsonPropertyName("sellerid")]
        public Guid SellerId { get; private set; }
        [JsonPropertyName("card")]
        public Card? Card { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; private set; }
        [JsonPropertyName("desiredType")]
        public EType DesiredType { get; set; }
        [JsonPropertyName("minDamage")]
        public int MinDamage { get; private set; }

        public bool SetMinDamage(int val)
        {
            if(val < 0) return false;
            MinDamage = val;
            return true;
        }
    }
}
