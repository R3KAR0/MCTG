using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EndPointAttribute : Attribute 
    {
            public string Path { get; private set; }
            public EHTTPMethod HTTPMethod { get; private set; }
            public EndPointAttribute(string path, string method)
            {
                Path = path;
                Enum.TryParse(method, out EHTTPMethod httpMethod);
                HTTPMethod = httpMethod;
            }
    }
}
