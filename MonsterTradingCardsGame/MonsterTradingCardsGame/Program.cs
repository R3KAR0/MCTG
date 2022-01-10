using MonsterTradingCardsGame.Mapper;
using Npgsql;
using Serilog;
using Serilog.Events;
using System.Text.Json;

namespace MonsterTradingCardsGame
{
    public class Program
    {
        private static ConfigMapper? configMapper;
        private static PackageCreationMapper? packageCreationMapper;
        private static RulesMapper? rulesMapper;

        public static ConfigMapper? GetConfigMapper()
        {
            if(configMapper == null)
            {
                SetupMapper();
            }
            return configMapper;          
        }

        public static RulesMapper? GetRulesMapper()
        {
            if (rulesMapper == null)
            {
                SetupMapper();
            }
            return rulesMapper;
        }

        public static PackageCreationMapper? GetPackageCreationMapper()
        {
            if (packageCreationMapper == null)
            {
                SetupMapper();
            }
            return packageCreationMapper;
        }
        static void Main(string[] args)
        {
            //Logger from config 
            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration)
            //    .CreateLogger();

            //Setup();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("..\\Logs\\Log.txt")
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();

            SetupMapper();

            NpgsqlConnection.GlobalTypeMapper.UseJsonNet();

            Server.Server GameServer = Server.Server.Instance;
            GameServer.Run();
        }

        private static void SetupMapper(string configMapperConfig = "..\\..\\..\\..\\config.json", string packageCreationMapperConfig = "..\\..\\..\\..\\PackageCreationConfig.json", string rulesMapperConfig = "..\\..\\..\\..\\rules.json")
        {
            using (var sr = new StreamReader(configMapperConfig))
            {
                try
                {
                    configMapper = JsonSerializer.Deserialize<ConfigMapper>(sr.ReadToEnd());
                    Log.Information("ConfigMapper loaded successfully!");
                }
                catch (Exception)
                {

                    throw;
                }
            }

            using (var sr = new StreamReader(packageCreationMapperConfig))
            {
                try
                {
                    packageCreationMapper = JsonSerializer.Deserialize<PackageCreationMapper>(sr.ReadToEnd());
                    Log.Information("PackageCreationMapper loaded successfully!");
                }
                catch (Exception)
                {
                    throw;
                }
            }

            using (var sr = new StreamReader(rulesMapperConfig))
            {
                try
                {
                    rulesMapper = JsonSerializer.Deserialize<RulesMapper>(sr.ReadToEnd());
                    Log.Information("RulesMapper loaded successfully!");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


        public static void TestSetup(string configMapperConfig, string packageCreationMapperConfig, string rulesMapperConfig)
        {
            using (var sr = new StreamReader(configMapperConfig))
            {
                try
                {
                    configMapper = JsonSerializer.Deserialize<ConfigMapper>(sr.ReadToEnd());
                    Log.Information("ConfigMapper loaded successfully!");
                }
                catch (Exception)
                {

                    throw;
                }
            }

            using (var sr = new StreamReader(packageCreationMapperConfig))
            {
                try
                {
                    packageCreationMapper = JsonSerializer.Deserialize<PackageCreationMapper>(sr.ReadToEnd());
                    Log.Information("PackageCreationMapper loaded successfully!");
                }
                catch (Exception)
                {
                    throw;
                }
            }

            using (var sr = new StreamReader(rulesMapperConfig))
            {
                try
                {
                    rulesMapper = JsonSerializer.Deserialize<RulesMapper>(sr.ReadToEnd());
                    Log.Information("RulesMapper loaded successfully!");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}