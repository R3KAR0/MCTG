using MonsterTradingCardsGame.Models;
using System.Text.Json.Serialization;

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
