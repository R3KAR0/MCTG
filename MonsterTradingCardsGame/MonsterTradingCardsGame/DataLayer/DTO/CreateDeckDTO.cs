using System.Text.Json.Serialization;

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
