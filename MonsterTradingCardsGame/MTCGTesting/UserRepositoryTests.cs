using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame;
using MonsterTradingCardsGame.DataLayer;

namespace MTCGTesting
{
    internal class UserRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
            UnitOfWork unit = new();
        }

        [Test]
        public void Test_GetAllUsers()
        {

            Assert.Pass();
        }

        [Test]
        public void Test_InvalidHTTP()
        {

        }
    }
}
