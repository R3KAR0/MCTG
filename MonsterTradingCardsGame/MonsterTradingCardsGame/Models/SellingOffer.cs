using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class SellingOffer
    {
        public SellingOffer(Guid cardId, Guid seller, DateTime creationDate, int price)
        {
            CardId = cardId;
            SellerId = seller;
            Timestamp = creationDate;
            Price = price;
        }

        [JsonConstructor]
        public SellingOffer(Guid cardid, Guid sellerid, Card? card, DateTime timestamp, int price)
        {
            CardId = cardid;
            SellerId = sellerid;
            Card = card;
            Timestamp = timestamp;
            Price = price;
        }

        [JsonPropertyName("cardid")]
        public Guid CardId { get; private set; }

        [JsonPropertyName("sellerid")]
        public Guid SellerId { get; private set; }
        [JsonPropertyName("card")]
        public Card? Card { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; private set; }
        [JsonPropertyName("price")]
        public int Price { get; private set; }

        public bool SetPrice(int val)
        {
            if (val < 0) return false;
            Price = val;
            return true;
        }
    }
}
