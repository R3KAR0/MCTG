using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class UserCardsDTO
    {
        public UserCardsDTO(List<Card> cards)
        {
            Cards = cards;
        }

        [JsonPropertyName("cards")]
        public List<Card> Cards { get; private set; } 

    }
}
