using MonsterTradingCardsGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class LoginResponseDTO : IJsonConvertable
    {
        [JsonPropertyName("token")]
        public string Token { get; private set; }

        public LoginResponseDTO(string token)
        {
            Token = token;
        }
    }
}
