using MonsterTradingCardsGame;
using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Server;
using Npgsql;
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
        User user1 = new (Guid.NewGuid().ToString(), "pw1");
        User user2 = new (Guid.NewGuid().ToString(), "pw2");
        List<Card>? user1Cards;
        List<Card>? user2Cards;
        NpgsqlConnection? testDB;

        [OneTimeSetUp]
        public void Setup()
        {

            _ = Program.GetConfigMapper();
            Program.TestSetup("..\\..\\..\\..\\testconfig.json", "..\\..\\..\\..\\TestPackageCreationConfig.json", "..\\..\\..\\..\\testrules.json");

            
            string strText = System.IO.File.ReadAllText("..\\..\\..\\..\\test.sql", Encoding.UTF8);
            string connString = Program.GetConfigMapper().ConnectionString;
            testDB = new(connString);
            testDB.Open();
            NpgsqlCommand cmd = new(strText, testDB);
            cmd.ExecuteNonQuery();

            using (var uow = new UnitOfWork())
            {
                uow.UserRepository().Add(user1);
                uow.UserRepository().Add(user2);
            }
        }

        [Test]
        public void Test_SpecialRulesUser1()
        {
            user1Cards = new();
            user2Cards = new();
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(Guid.NewGuid(), user1.Id, Guid.NewGuid(), "test package", EType.MONSTER, EKind.KNIGHT, EElement.NEUTRAL, 50));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(Guid.NewGuid(), user2.Id, Guid.NewGuid(), "test package", EType.SPELL, EKind.SPELL, EElement.WATER, 10));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.BattleLogs)
            {
                Console.WriteLine(item);
            }
            if (res.Winner == null) Assert.Fail();
            Assert.AreEqual(res.Winner, user2.Id);
        }

        [Test]
        public void Test_SpecialRulesUser2()
        {
            user1Cards = new();
            user2Cards = new();
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(Guid.NewGuid(), user1.Id, Guid.NewGuid(), "test package", EType.MONSTER, EKind.KNIGHT, EElement.NEUTRAL, 50));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(Guid.NewGuid(), user2.Id, Guid.NewGuid(), "test package", EType.SPELL, EKind.SPELL, EElement.WATER, 10));
            }

            var res = BattleHandler.Calculate(user2.Id, user1.Id, user2Cards, user1Cards);

            foreach (var item in res.BattleLogs)
            {
                Console.WriteLine(item);
            }
            if (res.Winner == null) Assert.Fail();
            Assert.AreEqual(res.Winner, user2.Id);
        }


        [Test]
        public void Test_CalculationMonsterFight()
        {
            user1Cards = new();
            user2Cards = new();
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(Guid.NewGuid(), user1.Id, Guid.NewGuid(), "test package", EType.MONSTER, EKind.KNIGHT, EElement.FIRE, 30));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(Guid.NewGuid(), user2.Id, Guid.NewGuid(), "test package", EType.MONSTER, EKind.DRAGON, EElement.WATER, 20));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.BattleLogs)
            {
                Console.WriteLine(item);
            }
            if (res.Winner == null) Assert.Fail();
            Assert.AreEqual(res.Winner, user2.Id);
        }

        [Test]
        public void Test_CalculationSpellMonsterAdvantageFight()
        {
            user1Cards = new();
            user2Cards = new();
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(Guid.NewGuid(), user1.Id, Guid.NewGuid(), "test package", EType.SPELL, EKind.SPELL, EElement.WATER, 20));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(Guid.NewGuid(), user2.Id, Guid.NewGuid(), "test package", EType.MONSTER, EKind.DRAGON, EElement.FIRE, 30));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.BattleLogs)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Winner, user1.Id);
        }


        [Test]
        public void Test_CalculationSpellMonsterNeturalFight()
        {
            user1Cards = new();
            user2Cards = new();
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(Guid.NewGuid(), user1.Id, Guid.NewGuid(), "test package", EType.SPELL, EKind.SPELL, EElement.FIRE, 10));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(Guid.NewGuid(), user2.Id, Guid.NewGuid(), "test package", EType.MONSTER, EKind.DRAGON, EElement.FIRE, 20));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.BattleLogs)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Winner, user2.Id);
        }


        [Test]
        public void Test_CalculationSpellSpellAdvantageFight()
        {
            user1Cards = new();
            user2Cards = new();
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(Guid.NewGuid(), user1.Id, Guid.NewGuid(), "test package", EType.SPELL, EKind.SPELL, EElement.FIRE, 30));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(Guid.NewGuid(), user2.Id, Guid.NewGuid(), "test package", EType.SPELL, EKind.SPELL, EElement.WATER, 20));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.BattleLogs)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Winner, user2.Id);
        }


        [Test]
        public void Test_CalculationSpellSpellNeutralFight()
        {
            user1Cards = new();
            user2Cards = new();
            for (int i = 0; i < 5; i++)
            {
                user1Cards.Add(new Card(Guid.NewGuid(), user1.Id, Guid.NewGuid(), "test package", EType.SPELL, EKind.SPELL, EElement.FIRE, 30));
            }

            for (int i = 0; i < 5; i++)
            {
                user2Cards.Add(new Card(Guid.NewGuid(), user2.Id, Guid.NewGuid(), "test package", EType.SPELL, EKind.SPELL, EElement.FIRE, 20));
            }

            var res = BattleHandler.Calculate(user1.Id, user2.Id, user1Cards, user2Cards);

            foreach (var item in res.BattleLogs)
            {
                Console.WriteLine(item);
            }
            Assert.AreEqual(res.Winner, user1.Id);
        }

    }
}
