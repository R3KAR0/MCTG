using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Buy
    {
        [JsonPropertyName("seller")]
        public Guid SellerId { get; private set; }
        [JsonPropertyName("buyer")]
        public Guid BuyerId { get; private set; }

        [JsonPropertyName("sellerCard")]
        public Guid SellerCardId { get; private set; }
        [JsonPropertyName("amount")]
        public int Amount { get; private set; }

        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; private set; }

        public Buy(Guid sellerId, Guid buyerId, Guid sellerCardId, int amount)
        {
            SellerId = sellerId;
            BuyerId = buyerId;
            SellerCardId = sellerCardId;
            Amount = amount;
            TimeStamp = DateTime.Now;
        }
    }
}
