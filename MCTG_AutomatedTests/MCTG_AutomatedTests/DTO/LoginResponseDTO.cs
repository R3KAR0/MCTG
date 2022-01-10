using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class LoginResponseDTO 
    {
        [JsonPropertyName("token")]
        public string Token { get; private set; }

        public LoginResponseDTO(string token)
        {
            Token = token;
        }
    }
}
