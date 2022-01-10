using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.Models
{
    public class TradeRecord
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
