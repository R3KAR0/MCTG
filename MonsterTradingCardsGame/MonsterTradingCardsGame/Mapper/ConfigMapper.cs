using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Mapper
{
    public class ConfigMapper
    {
        public ConfigMapper(string connectionString, string dBUser, string dBPassword, string userDescription, string postgresDoubleEntry, string secret, int deckSize)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            DBUser = dBUser ?? throw new ArgumentNullException(nameof(dBUser));
            DBPassword = dBPassword ?? throw new ArgumentNullException(nameof(dBPassword));
            UserDescription = userDescription ?? throw new ArgumentNullException(nameof(userDescription));
            PostgresDoubleEntry = postgresDoubleEntry ?? throw new ArgumentNullException(nameof(postgresDoubleEntry));
            Secret = secret ?? throw new ArgumentNullException(nameof(secret));
            DeckSize = deckSize;
        }

        [JsonPropertyName("connectionString")]
        public string ConnectionString { get; private set; }
        [JsonPropertyName("username")]
        public string DBUser { get; private set; }
        [JsonPropertyName("password")]
        public string DBPassword { get; private set; }

        [JsonPropertyName("standardUserDescription")]
        public string UserDescription { get; private set; }

        [JsonPropertyName("postgresDoubleEntryCode")]
        public string PostgresDoubleEntry { get; private set; }

        [JsonPropertyName("secret")]
        public string Secret { get; private set; }

        [JsonPropertyName("deckSize")]
        public int DeckSize { get; private set; }

    }
}
