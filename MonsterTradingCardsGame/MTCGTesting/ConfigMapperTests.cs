using MonsterTradingCardsGame.Mapper;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace MTCGTesting
{
    public class ConfigMapperTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_ConfigMapper()
        {
            ConfigMapper? mapper;

            using (var sr = new StreamReader("..\\..\\..\\..\\config.json"))
            {
                try
                {
                    mapper = JsonSerializer.Deserialize<ConfigMapper>(sr.ReadToEnd());
                    if (mapper == null)
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public void Test_PackageCreationMapper()
        {
            PackageCreationMapper? mapper;
            using (var sr = new StreamReader("..\\..\\..\\..\\PackageCreationConfig.json"))
            {
                try
                {
                    mapper = JsonSerializer.Deserialize<PackageCreationMapper>(sr.ReadToEnd());
                    if (mapper == null)
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void Test_RulesMapper()
        {
            RulesMapper? mapper;
            using (var sr = new StreamReader("..\\..\\..\\..\\rules.json"))
            {
                try
                {
                    mapper = JsonSerializer.Deserialize<RulesMapper>(sr.ReadToEnd());
                    if (mapper == null)
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception)
                {

                    Assert.Fail();
                }
            }
        }
    }
}
