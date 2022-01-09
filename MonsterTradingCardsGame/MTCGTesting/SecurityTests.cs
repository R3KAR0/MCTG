using MonsterTradingCardsGame;
using MonsterTradingCardsGame.Server;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGTesting
{
    public class SecurityTests
    {
        ConfigMapper mapper;
        string token;
        string username = "username";
        DateTime dateTime = DateTime.Now;
        [SetUp]
        public void Setup()
        {
            using (var sr = new StreamReader("..\\..\\..\\config.json"))
            {
               mapper = JsonSerializer.Deserialize<ConfigMapper>(sr.ReadToEnd());
            }
            token = $"{username}.{dateTime}";
        }

        [Test]
        public void Test_encryption()
        {
            try
            {
                var encrypted = SecurityHelper.EncryptString(token);
                if (encrypted.Length > 0 && encrypted.Contains(username) == false && encrypted.Contains(dateTime.ToString()) == false)
                {
                    Assert.Pass();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void Test_decryption()
        {
            try
            {
                var encrypted = SecurityHelper.EncryptString(token);
                var decrypted = SecurityHelper.DecryptString(encrypted);

                if(decrypted == token)
                {
                    Assert.Pass();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
