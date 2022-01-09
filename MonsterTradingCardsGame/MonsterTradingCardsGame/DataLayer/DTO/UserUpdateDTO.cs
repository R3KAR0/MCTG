using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class UserUpdateDTO
    {
        public UserUpdateDTO(string? newPassword, string newProfileDescription, byte[]? newPicture)
        {
            NewPassword = newPassword;
            NewProfileDescription = newProfileDescription ?? throw new ArgumentNullException(nameof(newProfileDescription));
            NewPicture = newPicture;
        }

        [JsonPropertyName("newpassword")]
        public string? NewPassword { get; set; }

        [JsonPropertyName("newdescription")]
        public string NewProfileDescription { get; private set; }

        [JsonPropertyName("newpicture")]
        public byte[]? NewPicture { get; set; }
    }
}
