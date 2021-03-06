using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.Models
{
    public class BattleResult
    {
        public BattleResult(Guid user1, Guid user2, Guid? winner)
        {
            Id = Guid.NewGuid();
            User1 = user1;
            User2 = user2;
            BattleTime = DateTime.Now;

            if (winner != user1 && winner != user2) throw new InvalidDataException();

            Winner = winner;
        }

        [JsonConstructor]
        public BattleResult(Guid id, Guid user1, Guid user2, Guid? winner, DateTime battletime)
        {
            Id = id;
            User1 = user1;
            User2 = user2;
            BattleTime = battletime;
            if (winner != user1 && winner != user2) throw new InvalidDataException();
            Winner = winner;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; private set; }
        [JsonPropertyName("user1")]
        public Guid User1 { get; private set; }
        [JsonPropertyName("user2")]
        public Guid User2 { get; private set; }
        [JsonPropertyName("winner")]
        public Guid? Winner { get; private set; }
        [JsonPropertyName("battletime")]
        public DateTime BattleTime { get; private set; }


    }
}
