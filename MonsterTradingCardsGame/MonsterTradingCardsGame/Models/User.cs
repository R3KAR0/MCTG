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
            Id = Guid.NewGuid();
            Password = SecurityHelper.sha256_hash(password) ?? throw new ArgumentNullException(nameof(password));
            Coins = 20; // hardcoded? -> Config
            Deck = new List<Card>();
            Stack = new List<Card>();
            Description = Program.GetConfigMapper().UserDescription;
            Elo = 100;
        }

        public User(string username, Guid iD, string password, int coins, string profileDescription, int elo)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Id = iD;
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Coins = coins;
            Description = profileDescription ?? throw new ArgumentNullException(nameof(profileDescription));
            Elo = elo;
        }

        [JsonConstructor]
        public User(string username, Guid id, string password, int coins, List<Card> deck, List<Card> stack, string description, int elo)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Id = id;
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Coins = coins;
            Deck = deck ?? throw new ArgumentNullException(nameof(deck));
            Stack = stack ?? throw new ArgumentNullException(nameof(stack));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Elo = elo;
        }

        [JsonPropertyName("username")]
        public string Username { get; private set; }

        [JsonPropertyName("id")]
        public Guid Id { get; private set; }

        [JsonPropertyName("password")]
        public string Password { get; private set; }

        [JsonPropertyName("coins")]
        public int Coins { get; private set; }

        [JsonPropertyName("deck")]
        public List<Card> Deck { get; private set; }

        [JsonPropertyName("stack")]
        public List<Card> Stack { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("elo")]
        public int Elo { get; private set; }    

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
            if(newDescription.Length > 128)
            {
                return false;
            }
            Description = newDescription;
            return true;
        }

    }
}
