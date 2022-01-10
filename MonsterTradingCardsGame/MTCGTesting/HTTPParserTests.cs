using MonsterTradingCardsGame.Server;
using NUnit.Framework;

namespace MTCGTesting
{
    public class HTTPParserTests
    {

        string demoHTTP;
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_ValidHTTP()
        {
            demoHTTP = "PUT /localhost/test HTTP/1.1\nAuthorization: dwadawdwddadw\nContent-Type: application/json\n\n{'test':'test'}";
            var message = HTTPParser.ParseHTTP(demoHTTP);
            Assert.AreEqual(message.Url, "/localhost/test");
            Assert.AreEqual(message.HTTPMethod, EHTTPMethod.PUT);
            Assert.AreEqual(message.HTTPVersion, "HTTP/1.1");
            Assert.AreEqual(message.Body, "{'test':'test'}");
        }

        [Test]
        public void Test_InvalidHTTP()
        {
            demoHTTP = "\nAuthorization: dwadawdwddadw\nContent-Type: application/json\n\n{'test':'test'}";
            try
            {
                var message = HTTPParser.ParseHTTP(demoHTTP);
            }
            catch (System.Exception)
            {
                
            }
        }

        [Test]
        public void Test_ValidHTTPWithBlankLinesInBody()
        {
            demoHTTP = "PUT /localhost/test HTTP/1.1\nAuthorization: dwadawdwddadw\nContent-Type: application/json\n\n{'test'\n:\n'test'}\n";
            var message = HTTPParser.ParseHTTP(demoHTTP);
            Assert.AreEqual(message.Url, "/localhost/test");
            Assert.AreEqual(message.HTTPMethod, EHTTPMethod.PUT);
            Assert.AreEqual(message.HTTPVersion, "HTTP/1.1");
            Assert.AreEqual(message.Body, "{'test':'test'}");
        }
    }
}