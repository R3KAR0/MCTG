using System.Net;

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
