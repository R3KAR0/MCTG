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
            Seller = seller;
            CreationDate = creationDate;
            Price = price;
        }

        public SellingOffer(Guid cardId, Guid seller, Card? cardToSell, DateTime creationDate, int price)
        {
            CardId = cardId;
            Seller = seller;
            CardToSell = cardToSell;
            CreationDate = creationDate;
            Price = price;
        }

        public Guid CardId { get; private set; }

        [JsonPropertyName("seller")]
        public Guid Seller { get; private set; }
        [JsonPropertyName("card")]
        public Card? CardToSell { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime CreationDate { get; private set; }
        [JsonPropertyName("Price")]
        public int Price { get; private set; }

        public bool SetPrice(int val)
        {
            if (val < 0) return false;
            Price = val;
            return true;
        }
    }
}
