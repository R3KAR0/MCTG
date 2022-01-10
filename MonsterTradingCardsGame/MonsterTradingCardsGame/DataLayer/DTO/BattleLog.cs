using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class BattleLog
    {
        public BattleLog(List<string> battleLogs, Guid user1, Guid user2, Guid? winner)
        {
            BattleLogs = battleLogs ?? throw new ArgumentNullException(nameof(battleLogs));
            User1 = user1;
            User2 = user2;
            Winner = winner;
        }

        [JsonPropertyName("battlelogs")]
        public List<string> BattleLogs { get; private set; } = new List<string>();
        [JsonPropertyName("user1")]
        public Guid User1 { get; private set; }
        [JsonPropertyName("user2")]
        public Guid User2 { get; private set; }
        [JsonPropertyName("winner")]
        public Guid? Winner { get; private set; }
    }
}
