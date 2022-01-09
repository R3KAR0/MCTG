using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server
{
    public class HTTPResponse
    {

        public readonly string HTTPVersion="HTTP/1.0";
        public HttpStatusCode Status { get; private set; }
        public int StatusCode { get; private set; }
        public string? Content { get; private set; }

        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>();
        public DateTime date { get; private set; }

        public HTTPResponse(HttpStatusCode statusCode, string content)
        {
            Status = statusCode;
            StatusCode = (int)statusCode;
            date = DateTime.UtcNow;
            Content = content;
            //Headers.Add("Date", DateTime.UtcNow);
            //Headers.Add("Server", "apache 1.2");
            if (content != null && content.Length > 0)
            {
                Headers.Add("Content-Length", content.Length);
                Headers.Add("Content-Type", "application/json"); 
            }
            else
            {
                Headers.Add("Content-Length", 0);
            }
        }

        public override string? ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{HTTPVersion} {StatusCode} {Status}");
            foreach (var head in Headers)
            {
                stringBuilder.AppendLine($"{head.Key}: {head.Value}");
            }

            stringBuilder.AppendLine("");
            if(Content != null && Content.Length > 0)
            {
                stringBuilder.Append(Content);   
            }

            return stringBuilder.ToString();    
        }
    }
}
