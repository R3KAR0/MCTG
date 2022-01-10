using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class UserScore
    {
        public UserScore(string username, int score)
        {
            Score = score;
            Username = username ?? throw new ArgumentNullException(nameof(username));
        }

        [JsonPropertyName("username")]
        public string Username { get; private set; }
        [JsonPropertyName("score")]
        public int Score { get; private set; }

    }
}
