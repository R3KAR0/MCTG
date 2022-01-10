using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class BuyRecord
    {
        [JsonPropertyName("sellerid")]
        public Guid SellerId { get; private set; }
        [JsonPropertyName("buyerid")]
        public Guid BuyerId { get; private set; }

        [JsonPropertyName("sellerCard")]
        public Guid sellerCardId { get; private set; }
        [JsonPropertyName("amount")]
        public int Amount { get; private set; }

        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; private set; }

        public BuyRecord(Guid sellerId, Guid buyerId, Guid sellerCardId, int amount)
        {
            SellerId = sellerId;
            BuyerId = buyerId;
            this.sellerCardId = sellerCardId;
            Amount = amount;
            TimeStamp = DateTime.Now;
        }

        [JsonConstructor]
        public BuyRecord(Guid sellerId, Guid buyerId, Guid sellerCardId, int amount, DateTime timestamp)
        {
            SellerId = sellerId;
            BuyerId = buyerId;
            this.sellerCardId = sellerCardId;
            Amount = amount;
            TimeStamp = timestamp;
        }
    }
}
