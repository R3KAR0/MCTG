using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class UserRepresentation
    {
        public UserRepresentation(string username, int coins, string profileDescription, int battlePower)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Coins = coins;
            ProfileDescription = profileDescription ?? throw new ArgumentNullException(nameof(profileDescription));
            BattlePower = battlePower;
        }

        [JsonPropertyName("username")]
        public string Username { get; private set; }

        [JsonPropertyName("coins")]
        public int Coins { get; private set; }

        [JsonPropertyName("description")]
        public string ProfileDescription { get; private set; }

        [JsonPropertyName("elo")]
        public int BattlePower { get; private set; }
    }
}
