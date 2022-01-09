using MonsterTradingCardsGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{

    public enum EKind {
        [EnumMember(Value="SPELL")]
        SPELL,
        [EnumMember(Value = "GOBLIN")]
        GOBLIN,
        [EnumMember(Value = "DRAGON")]
        DRAGON,
        [EnumMember(Value = "ORC")]
        ORC,
        [EnumMember(Value = "KNIGHT")]
        KNIGHT,
        [EnumMember(Value = "KRAKEN")]
        KRAKEN,
        [EnumMember(Value = "ELVES")]
        ELVES,
        [EnumMember(Value = "WIZZARD")]
        WIZZARD
    }

    public enum EType {
        [EnumMember(Value = "SPELL")]
        SPELL,
        [EnumMember(Value = "MONSTER")]
        MONSTER
    }
    public enum EElement {
        [EnumMember(Value = "FIRE")]
        FIRE,
        [EnumMember(Value = "WATER")]
        WATER,
        [EnumMember(Value = "NEUTRAL")]
        NEUTRAL,
        [EnumMember(Value = "ALL")]
        ALL
    }

    public class Card : IJsonConvertable
    {
        public Card(Guid id,Guid owner, Guid package, string description, EType cardType, EKind kind, EElement element, int damage)
        {
            Id = id;
            UserId = owner;
            PackageId = package;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            CardType = cardType;
            Kind = kind;
            Element = element;
            CreationDate = DateTime.Now;
            Damage = damage;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; private set; }

        [JsonPropertyName("owner")]
        public Guid UserId { get; private set; }

        [JsonPropertyName("package")]
        public Guid PackageId { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("timestamp")]
        public DateTime CreationDate { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("type")]
        public EType CardType { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("kind")]
        public EKind Kind { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("element")]
        public EElement Element { get; private set; }

        [JsonPropertyName("damage")]
        public int Damage { get; private set; }
    }
}
