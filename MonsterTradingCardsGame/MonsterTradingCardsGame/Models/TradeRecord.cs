using MonsterTradingCardsGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class TradeRecord : IJsonConvertable
    {
        [JsonPropertyName("seller")]
        public Guid SellerId { get; private set; }
        [JsonPropertyName("buyer")]
        public Guid BuyerId { get; private set; }

        [JsonPropertyName("sellerCard")]
        public Guid SellerCardId { get; private set; }
        [JsonPropertyName("buyerCard")]
        public Guid BuyerCardId { get; private set; }

        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; private set; }

        public TradeRecord(Guid sellerId, Guid buyerId, Guid sellerCardId, Guid buyerCardId)
        {
            SellerId = sellerId;
            BuyerId = buyerId;
            SellerCardId = sellerCardId;
            BuyerCardId = buyerCardId;
            TimeStamp = DateTime.Now;
        }
    }
}
