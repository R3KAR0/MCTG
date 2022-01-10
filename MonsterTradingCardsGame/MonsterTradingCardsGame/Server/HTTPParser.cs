using System.Text;

namespace MonsterTradingCardsGame.Server
{
    public static class HTTPParser
    {
        public static HTTPMessage ParseHTTP(string HTTPMessage)
        {
            
            List<string> lines = HTTPMessage.Split("\n").ToList();
            string url = lines[0];
            lines.RemoveAt(0);  //remove url line
            var urlParts = url.Split(' ').ToList();
            
            var method = urlParts[0];
            url = urlParts[1];
            var HTTPVersion = urlParts[2];


            bool bodySection = false;
            StringBuilder bodyBuilder = new();
            Dictionary<string,string> headers = new Dictionary<string,string>();

            try
            {
                foreach (var line in lines)
                {
                    if (line == "")
                    {
                        bodySection = true;
                    }

                    if (!bodySection)
                    {
                        string[] headerElement = line.Split(":");
                        headers.Add(headerElement[0], headerElement[1]);
                    }
                    else
                    {
                        bodyBuilder.Append(line);
                    }
                }
                if (headers.Count > 0)
                {
                    EHTTPMethod hmethode;
                    Enum.TryParse(method, out hmethode);
                    return new HTTPMessage(headers,url, bodyBuilder.ToString(), HTTPVersion, hmethode);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
