using MonsterTradingCardsGame;
using MonsterTradingCardsGame.Mapper;
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
            mapper = Program.GetConfigMapper();
            token = $"{username}.{dateTime}";
        }

        [Test]
        public void Test_encryption()
        {
            try
            {
                var encrypted = SecurityHelper.EncryptString(token);
                if (encrypted.Contains(username) == false && encrypted.Contains(dateTime.ToString()) == false)
                {
                    Assert.Greater(encrypted.Length, 0);
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

                Assert.AreEqual(token, decrypted);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        [Test]
        public void Test_sha256Length()
        {
            var hash1 = SecurityHelper.sha256_hash(token);
            var hash2 = SecurityHelper.sha256_hash(username);
            Assert.AreEqual(hash1.Length, 64);
            Assert.AreEqual(hash2.Length, 64);
        }
    }
}
