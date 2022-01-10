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
        [JsonPropertyName("sellerId")]
        public Guid SellerId { get; private set; }
        [JsonPropertyName("buyerId")]
        public Guid BuyerId { get; private set; }

        [JsonPropertyName("sellerCardId")]
        public Guid SellerCardId { get; private set; }
        [JsonPropertyName("buyerCardId")]
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

        [JsonConstructor]
        public TradeRecord(Guid sellerId, Guid buyerId, Guid sellerCardId, Guid buyerCardId, DateTime timeStamp)
        {
            SellerId = sellerId;
            BuyerId = buyerId;
            SellerCardId = sellerCardId;
            BuyerCardId = buyerCardId;
            TimeStamp = timeStamp;
        }
    }
}
