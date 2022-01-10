using MonsterTradingCardsGame.Server;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGTesting
{
    public class HTTPResponseTests
    {
        string body = "{'test':'test'}";
        string representation = "HTTP/1.0 200 OK\nContent-Length: 15\nContent-Type: application/json\n\n{'test':'test'}";

        [Test]
        public void Test_HTTPResponseBody()
        {
            HTTPResponse response = new(System.Net.HttpStatusCode.OK, body);

            Assert.AreEqual(response.StatusCode, 200);
            Assert.AreEqual(response.Content, body);
        }

        [Test]
        public void Test_HTTPResponseStringRepresentation()
        {
            HTTPResponse response = new(System.Net.HttpStatusCode.OK, body);

            var representation = response.ToString();
            Assert.AreEqual(representation, response.ToString());
        }

    }
}
