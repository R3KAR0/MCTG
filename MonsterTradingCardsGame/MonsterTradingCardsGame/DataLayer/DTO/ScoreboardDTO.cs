using System.Text.Json.Serialization;

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
