using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class BuyRecord
    {
        [JsonPropertyName("sellerid")]
        public Guid SellerId { get; private set; }
        [JsonPropertyName("buyerid")]
        public Guid BuyerId { get; private set; }

        [JsonPropertyName("sellerCard")]
        public Guid SellerCard { get; private set; }
        [JsonPropertyName("amount")]
        public int Amount { get; private set; }

        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; private set; }

        public BuyRecord(Guid sellerId, Guid buyerId, Guid sellerCard, int amount)
        {
            SellerId = sellerId;
            BuyerId = buyerId;
            SellerCard = sellerCard;
            Amount = amount;
            TimeStamp = DateTime.Now;
        }

        [JsonConstructor]
        public BuyRecord(Guid sellerid, Guid buyerid, Guid sellerCard, int amount, DateTime timestamp)
        {
            SellerId = sellerid;
            BuyerId = buyerid;
            SellerCard = sellerCard;
            Amount = amount;
            TimeStamp = timestamp;
        }
    }
}
