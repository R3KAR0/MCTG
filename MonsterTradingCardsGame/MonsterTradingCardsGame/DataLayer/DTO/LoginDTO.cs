using MonsterTradingCardsGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class LoginDTO : IJsonConvertable
    {

        [JsonPropertyName("username")]
        public string Username { get; private set; }
        [JsonPropertyName("password")]
        public string Password { get; private set; }

        public LoginDTO(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}
