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
            this.specialRules = specialRules ?? throw new ArgumentNullException(nameof(specialRules));
            Strongs = strongs ?? throw new ArgumentNullException(nameof(strongs));
        }

        [JsonPropertyName("specialRules")]
        public List<SpecialRule> specialRules { get; private set; }

        [JsonPropertyName("strong")]
        public List<Strong> Strongs { get; private set; }
    }
}
