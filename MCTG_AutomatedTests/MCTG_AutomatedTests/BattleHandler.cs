using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MCTG_AutomatedTests
{
    public static class BattleHandler
    {
        public static void Battle(object requestMessage)
        {
            var message = (HttpRequestMessage)requestMessage;
            HttpClient client = new HttpClient();
            var response = client.Send(message);
            var resContent =  response.Content.ReadAsStringAsync().Result;
        }
    }
}
