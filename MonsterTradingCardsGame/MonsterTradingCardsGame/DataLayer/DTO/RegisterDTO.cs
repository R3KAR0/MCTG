using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class RegisterDTO
    {

        [JsonPropertyName("username")]
        public string Username { get; private set; }
        [JsonPropertyName("password")]
        public string Password { get; private set; }

        public RegisterDTO(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }


    }
}
