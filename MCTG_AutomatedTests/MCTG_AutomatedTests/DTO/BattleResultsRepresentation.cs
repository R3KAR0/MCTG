using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class BattleResultsRepresentation
    {
        public BattleResultsRepresentation(int wins, int loses, int draws, int elo)
        {
            Wins = wins;
            Loses = loses;
            Draws = draws;
            Elo = elo;
        }

        [JsonPropertyName("wins")]
        public int Wins { get; private set; }

        [JsonPropertyName("loses")]
        public int Loses { get; private set; }

        [JsonPropertyName("draws")]
        public int Draws { get; private set; }

        [JsonPropertyName("elo")]
        public int Elo { get; private set; }
    }
}
