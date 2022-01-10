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
        public void Test_SpecialRulesUser1()
        {
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(new Guid(), user1.Id, new Guid(), "test package", EType.MONSTER, EKind.KNIGHT, EElement.NEUTRAL, 50));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(new Guid(), user2.Id, new Guid(), "test package", EType.SPELL, EKind.SPELL, EElement.WATER, 10));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.Item2)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Item1.Value, user2.Id);
        }

        [Test]
        public void Test_SpecialRulesUser2()
        {
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(new Guid(), user1.Id, new Guid(), "test package", EType.MONSTER, EKind.KNIGHT, EElement.NEUTRAL, 50));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(new Guid(), user2.Id, new Guid(), "test package", EType.SPELL, EKind.SPELL, EElement.WATER, 10));
            }

            var res = BattleHandler.Calculate(user2.Id, user1.Id, user2Cards, user1Cards);

            foreach (var item in res.Item2)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Item1.Value, user2.Id);
        }


        [Test]
        public void Test_CalculationMonsterFight()
        {
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(new Guid(), user1.Id, new Guid(), "test package", EType.MONSTER, EKind.KNIGHT, EElement.FIRE, 30));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(new Guid(), user2.Id, new Guid(), "test package", EType.MONSTER, EKind.DRAGON, EElement.WATER, 20));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.Item2)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Item1.Value, user2.Id);
        }

        [Test]
        public void Test_CalculationSpellMonsterAdvantageFight()
        {
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(new Guid(), user1.Id, new Guid(), "test package", EType.SPELL, EKind.SPELL, EElement.WATER, 20));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(new Guid(), user2.Id, new Guid(), "test package", EType.MONSTER, EKind.DRAGON, EElement.FIRE, 30));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.Item2)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Item1.Value, user1.Id);
        }


        [Test]
        public void Test_CalculationSpellMonsterNeturalFight()
        {
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(new Guid(), user1.Id, new Guid(), "test package", EType.SPELL, EKind.SPELL, EElement.FIRE, 10));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(new Guid(), user2.Id, new Guid(), "test package", EType.MONSTER, EKind.DRAGON, EElement.FIRE, 20));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.Item2)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Item1.Value, user2.Id);
        }


        [Test]
        public void Test_CalculationSpellSpellAdvantageFight()
        {
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(new Guid(), user1.Id, new Guid(), "test package", EType.SPELL, EKind.SPELL, EElement.FIRE, 30));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(new Guid(), user2.Id, new Guid(), "test package", EType.SPELL, EKind.SPELL, EElement.WATER, 20));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.Item2)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Item1.Value, user2.Id);
        }


        [Test]
        public void Test_CalculationSpellSpellNeutralFight()
        {
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(new Guid(), user1.Id, new Guid(), "test package", EType.SPELL, EKind.SPELL, EElement.FIRE, 30));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(new Guid(), user2.Id, new Guid(), "test package", EType.SPELL, EKind.SPELL, EElement.FIRE, 20));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.Item2)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Item1.Value, user1.Id);
        }

    }
}
