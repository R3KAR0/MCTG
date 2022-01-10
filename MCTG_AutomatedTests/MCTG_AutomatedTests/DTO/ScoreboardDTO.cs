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
        public ScoreboardDTO(List<UserScore> eloScores)
        {
            EloScores = eloScores;
        }

        [JsonPropertyName("scores")]
        public List<UserScore> EloScores { get; private set; } = new();
    }
}
