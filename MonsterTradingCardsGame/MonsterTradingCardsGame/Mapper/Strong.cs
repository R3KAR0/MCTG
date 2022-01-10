using MonsterTradingCardsGame.Models;
using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.Mapper
{
    public class Strong
    {
        public Strong(EElement strongElement, EElement weakElement)
        {
            StrongElement = strongElement;
            WeakElement = weakElement;
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("StrongElement")]
        public EElement StrongElement { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("WeakElement")]
        public EElement WeakElement { get; private set; }
    }
}
