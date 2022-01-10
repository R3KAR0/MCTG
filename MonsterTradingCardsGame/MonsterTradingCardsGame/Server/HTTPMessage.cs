namespace MonsterTradingCardsGame.Server
{
    public enum EHTTPMethod { GET, POST, PUT, DELETE, PATCH }
    public class HTTPMessage
    {


        public Dictionary<string, string> Headers { get; private set; }
        public string Url { get ; private set; }
        public string Body { get; private set; }
        public string HTTPVersion { get; private set; }
        public EHTTPMethod HTTPMethod { get; private set; }

        public HTTPMessage(Dictionary<string, string> headers, string url, string body, string hTTPVersion, EHTTPMethod hTTPMethod)
        {
            Headers = headers; //?? throw new ArgumentNullException(nameof(headers));
            Url = url ?? throw new ArgumentNullException(nameof(url));
            Body = body ?? throw new ArgumentNullException(nameof(body));
            HTTPVersion = hTTPVersion ?? throw new ArgumentNullException(nameof(hTTPVersion));
            HTTPMethod = hTTPMethod;
        }

    }
}
