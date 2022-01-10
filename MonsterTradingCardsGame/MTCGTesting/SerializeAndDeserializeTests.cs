using MonsterTradingCardsGame.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGTesting
{
    public class SerializeAndDeserializeTests
    {

        [Test]
        public void SerializeTestBattleResult()
        {
            var u1 = Guid.NewGuid();
            var u2 = Guid.NewGuid();
            var br = new BattleResult(Guid.NewGuid(), u1, u2, u2, DateTime.Now);
            try
            {
                var json = JsonSerializer.Serialize(br);
                var obj = JsonSerializer.Deserialize<BattleResult>(json);
                if (obj == null) Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SerializeTestBuyRecord()
        {
            var u1 = Guid.NewGuid();
            var u2 = Guid.NewGuid();
            var c = Guid.NewGuid();
            var br = new BuyRecord(u1,u2,c,10,DateTime.Now);
            try
            {
                var json = JsonSerializer.Serialize(br);
                var obj = JsonSerializer.Deserialize<BuyRecord>(json);
                if (obj == null) Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SerializeTestCard()
        {
            var u1 = Guid.NewGuid();
            var p = Guid.NewGuid();
            var card = new Card(Guid.NewGuid(),u1,p,"test",DateTime.Now,EType.MONSTER,EKind.DRAGON, EElement.FIRE,100);
            try
            {
                var json = JsonSerializer.Serialize(card);
                var obj = JsonSerializer.Deserialize<Card>(json);
                if (obj == null) Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SerializeTestDeck()
        {
            var u1 = Guid.NewGuid();
            var deck = new Deck(Guid.NewGuid(),u1, "test", DateTime.Now, new List<Card>());
            try
            {
                var json = JsonSerializer.Serialize(deck);
                var obj = JsonSerializer.Deserialize<Deck>(json);
                if (obj == null) Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SerializeTestBattlePackage()
        {
            var u1 = Guid.NewGuid();
            var package = new Package(Guid.NewGuid(), u1, "test", 5, DateTime.Now);
            try
            {
                var json = JsonSerializer.Serialize(package);
                var obj = JsonSerializer.Deserialize<Package>(json);
                if (obj == null) Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SerializeTestSellingOffer()
        {
            var sellingOffer = new SellingOffer(Guid.NewGuid(),Guid.NewGuid(),DateTime.Now, 10);
            try
            {
                var json = JsonSerializer.Serialize(sellingOffer);
                var obj = JsonSerializer.Deserialize<SellingOffer>(json);
                if (obj == null) Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SerializeTestTradingOffer()
        {
            var tradingOffer = new TradeOffer(Guid.NewGuid(), Guid.NewGuid(),EType.MONSTER, 10);
            try
            {
                var json = JsonSerializer.Serialize(tradingOffer);
                var obj = JsonSerializer.Deserialize<TradeOffer>(json);
                if (obj == null) Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        [Test]
        public void SerializeTestTradeRecord()
        {
            var tradeRecord = new TradeRecord(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            try
            {
                var json = JsonSerializer.Serialize(tradeRecord);
                var obj = JsonSerializer.Deserialize<TradeRecord>(json);
                if (obj == null) Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        [Test]
        public void SerializeTestUser()
        {
            var user = new User("test","test");
            try
            {
                var json = JsonSerializer.Serialize(user);
                var obj = JsonSerializer.Deserialize<User>(json);
                if (obj == null) Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

    }
}
