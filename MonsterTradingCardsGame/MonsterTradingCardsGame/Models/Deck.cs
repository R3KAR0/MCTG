using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public  class Deck
    {
        public Deck(Guid id, Guid userId, string description, DateTime timestamp)
        {
            Id = id;
            UserId = userId;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Timestamp = timestamp;
            Cards = new List<Card>();
        }

        [JsonConstructor]
        public Deck(Guid id, Guid userId, string description, DateTime timestamp, List<Card> cards)
        {
            Id = id;
            UserId = userId;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Timestamp = timestamp;
            Cards = cards;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; private set; }

        [JsonPropertyName("user")]
        public Guid UserId { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; private set; }

        [JsonPropertyName("cards")]
        public List<Card> Cards { get; set; }

        public bool SetDescription(string newDescription)
        {
            if (newDescription.Length > 128)
            {
                return false;
            }
            Description = newDescription;
            return true;
        }

    }
}
