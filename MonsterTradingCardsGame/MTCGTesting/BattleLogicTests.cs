using MonsterTradingCardsGame;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Server;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGTesting
{
    public class BattleLogicTests
    {

        User user1 = new("user1", "pw1");
        User user2 = new("user2", "pw2");
        List<Card> user1Cards;
        List<Card> user2Cards;
        [SetUp]
        public void Setup()
        {
            user1Cards = new();
            user2Cards = new();
        }

        [Test]
        public void Test_SpecialRules()
        {
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(new Guid(), user1.Id, new Guid(), "test package", EType.MONSTER, EKind.KNIGHT, EElement.NEUTRAL, 50));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(new Guid(), user1.Id, new Guid(), "test package", EType.SPELL, EKind.SPELL, EElement.WATER, 10));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.Item2)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Item1.Value, user2.Id);
        }

        [Test]
        public void Test_InvalidHTTP()
        {

        }

    }
}
