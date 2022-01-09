﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public  class Deck
    {
        public Deck(Guid id, Guid userId, string description, string creationDate)
        {
            Id = id;
            UserId = userId;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            CreationDate = creationDate ?? throw new ArgumentNullException(nameof(creationDate));
            Cards = new List<Card>();
        }

        public Deck(Guid id, Guid userId, List<Card> cards, string description, string creationDate)
        {
            Id = id;
            UserId = userId;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            CreationDate = creationDate ?? throw new ArgumentNullException(nameof(creationDate));
            Cards = cards;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; private set; }

        [JsonPropertyName("user")]
        public Guid UserId { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("timestamp")]
        public string CreationDate { get; private set; }


        [JsonPropertyName("cards")]
        public List<Card> Cards { get; set; }

    }
}