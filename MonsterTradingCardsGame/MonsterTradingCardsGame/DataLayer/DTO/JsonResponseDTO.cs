using MonsterTradingCardsGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class JsonResponseDTO
    {
        public string Content { get; private set; } = "";
        public HttpStatusCode StatusCode { get; private set; }

        public JsonResponseDTO(string? content, HttpStatusCode status)
        {
            if(content != null)
            {
                Content = content;
            }

            StatusCode = status;
        }
    }
}
