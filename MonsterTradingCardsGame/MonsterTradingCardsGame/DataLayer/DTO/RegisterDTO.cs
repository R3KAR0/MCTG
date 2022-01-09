using MonsterTradingCardsGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class RegisterDTO : IJsonConvertable
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
