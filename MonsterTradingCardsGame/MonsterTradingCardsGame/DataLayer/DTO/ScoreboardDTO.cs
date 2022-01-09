using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class ScoreboardDTO 
    {
        public ScoreboardDTO(List<Tuple<string, int>> eloScores)
        {
            EloScores = eloScores ?? throw new ArgumentNullException(nameof(eloScores));
        }

        [JsonPropertyName("scores")]
        public List<Tuple<string, int>> EloScores { get; private set; }
    }
}
