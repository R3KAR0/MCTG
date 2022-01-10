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
            Owner = owner;
            Package = package;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Type = cardType;
            Kind = kind;
            Element = element;
            Timestamp = DateTime.Now;
            Damage = damage;
        }

        [JsonConstructor]
        public Card(Guid id, Guid owner, Guid package, string description, DateTime timestamp, EType type, EKind kind, EElement element, int damage)
        {
            Id = id;
            Owner = owner;
            Package = package;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Type = type;
            Kind = kind;
            Element = element;
            Timestamp = timestamp;
            Damage = damage;
        }


        [JsonPropertyName("id")]
        public Guid Id { get; private set; }

        [JsonPropertyName("owner")]
        public Guid Owner { get; private set; }

        [JsonPropertyName("package")]
        public Guid Package { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("type")]
        public EType Type { get; private set; }

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
