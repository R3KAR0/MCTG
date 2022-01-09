using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Mapper
{
    public class Strong
    {
        public Strong(EElement strongElement, EElement weakElement)
        {
            StrongElement = strongElement;
            WeakElement = weakElement;
        }

        [JsonPropertyName("StrongElement")]
        public EElement StrongElement { get; private set; }

        [JsonPropertyName("WeakElement")]
        public EElement WeakElement { get; private set; }
    }
}
