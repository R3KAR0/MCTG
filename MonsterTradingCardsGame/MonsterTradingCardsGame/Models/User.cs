using System.Text.Json.Serialization;
using MonsterTradingCardsGame.Interfaces;
using MonsterTradingCardsGame.Server;

namespace MonsterTradingCardsGame.Models
{
    public class User : IJsonConvertable
    {
        public User(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            ID = Guid.NewGuid();
            Password = SecurityHelper.sha256_hash(password) ?? throw new ArgumentNullException(nameof(password));
            Coins = 20; // hardcoded? -> Config
            UserDeck = new List<Card>();
            Stack = new List<Card>();
            ProfileDescription = Program.GetConfigMapper().UserDescription;
            Picture = null;
            BattlePower = 100;
        }

        public User(string username, Guid iD, string password, int coins, string profileDescription)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            ID = iD;
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Coins = coins;
            ProfileDescription = profileDescription ?? throw new ArgumentNullException(nameof(profileDescription));
        }

        public User(string username, Guid iD, string password, int coins, string profileDescription, byte[]? picture)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            ID = iD;
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Coins = coins;
            ProfileDescription = profileDescription ?? throw new ArgumentNullException(nameof(profileDescription));
            Picture = picture;
        }

        [JsonPropertyName("username")]
        public string Username { get; private set; }

        [JsonPropertyName("id")]
        public Guid ID { get; private set; }

        //[JsonPropertyName("password")] should not be transfered!
        public string Password { get; private set; }

        [JsonPropertyName("coins")]
        public int Coins { get; private set; }

        [JsonPropertyName("deck")]
        public List<Card> UserDeck { get; private set; }

        [JsonPropertyName("stack")]
        public List<Card> Stack { get; private set; }

        [JsonPropertyName("description")]
        public string ProfileDescription { get; private set; }

        [JsonPropertyName("picture")]
        public byte[]? Picture { get; private set; }

        [JsonPropertyName("elo")]
        public int BattlePower { get; private set; }    

        public bool SetCoins(int amount)
        {
            if(Coins+amount < 0)
            {
                return false;
            }
            Coins += amount;
            return true;
        }

        public bool SetProfileDescription(string newDescription)
        {
            if(newDescription.Length > 2048)
            {
                return false;
            }
            ProfileDescription = newDescription;
            return true;
        }

    }
}
