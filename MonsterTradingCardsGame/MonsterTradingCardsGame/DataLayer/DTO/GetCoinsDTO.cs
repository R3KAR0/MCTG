using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class GetCoinsDTO
    {
        public GetCoinsDTO(int amount)
        {
            Amount = amount;
        }

        [JsonPropertyName("amount")]
        public int Amount { get; private set; }
    }
}
