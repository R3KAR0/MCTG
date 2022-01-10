using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class CreateDeckDTO
    {
        public CreateDeckDTO(string description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        [JsonPropertyName("description")]
        public string Description { get; private set; }
    }
}
