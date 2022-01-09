using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Mapper
{
    public class RulesMapper
    {
        public RulesMapper(List<SpecialRule> specialRules, List<Strong> strongs)
        {
            this.SpecialRules = specialRules ?? throw new ArgumentNullException(nameof(specialRules));
            Strongs = strongs ?? throw new ArgumentNullException(nameof(strongs));
        }

        [JsonPropertyName("SpecialRules")]
        public List<SpecialRule> SpecialRules { get; private set; }

        [JsonPropertyName("Strongs")]
        public List<Strong> Strongs { get; private set; }
    }
}
