using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Mapper
{
    public class SpecialRule
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("StrongElement")]
        public EElement element { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("StrongType")]
        public EType type { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("StrongKind")]
        public EKind kind { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("KillElement")]
        public EElement killelement { get; private set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("KillType")]
        public EType killtype { get; private set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("KillKind")]
        public EKind killkind { get; private set; }

        [JsonPropertyName("KillText")]
        public string KillText { get; private set; }

        public SpecialRule(EElement element, EType type, EKind kind, EElement killelement, EType killtype, EKind killkind, string killText)
        {
            this.element = element;
            this.type = type;
            this.kind = kind;
            this.killelement = killelement;
            this.killtype = killtype;
            this.killkind = killkind;
            this.KillText = killText ?? throw new ArgumentNullException(nameof(killText));
        }
    }
}
