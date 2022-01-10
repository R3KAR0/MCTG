using MonsterTradingCardsGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class BattleResult : IJsonConvertable
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
        public BattleResult(Guid user1, Guid user2, Guid? winner, DateTime date)
        {
            Id = Guid.NewGuid();
            User1 = user1;
            User2 = user2;
            BattleTime = date;
            if (winner != user1 && winner != user2) throw new InvalidDataException();
            Winner = winner;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; private set; }
        [JsonPropertyName("user1")]
        public Guid User1 { get; private set; }
        [JsonPropertyName("user2")]
        public Guid User2 { get; private set; }
        [JsonPropertyName("battletime")]
        public DateTime BattleTime { get; private set; }
        [JsonPropertyName("winner")]
        public Guid? Winner { get; private set; }


    }
}
