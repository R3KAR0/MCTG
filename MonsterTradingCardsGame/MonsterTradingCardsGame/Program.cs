using Autofac;
using MonsterTradingCardsGame.Server.Controller;
using Npgsql;
using Serilog;
using Serilog.Events;
using System;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    public class Program
    {
        public static IContainer? container;
        private static ContainerBuilder containerBuilder = new();
        private static ConfigMapper? configMapper;
        private static PackageCreationMapper? packageCreationMapper;

        public static ConfigMapper? GetConfigMapper()
        {
            if(configMapper == null)
            {
                Setup();
            }
            return configMapper;          
        }

        public static PackageCreationMapper? GetPackageCreationMapper()
        {
            if (packageCreationMapper == null)
            {
                Setup();
            }
            return packageCreationMapper;
        }
        static void Main(string[] args)
        {
            //Logger from config 
            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration)
            //    .CreateLogger();

            Setup();
            //var con = containerBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("..\\Logs\\UserLog-{Date}.txt")
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();

            NpgsqlConnection.GlobalTypeMapper.UseJsonNet();

            Server.Server GameServer = Server.Server.Instance;
            GameServer.Run();
        }

        private static void Setup()
        {
            // Container INJECTION
            using (var sr = new StreamReader("..\\..\\..\\config.json"))
            {
                try
                {
                    configMapper = JsonSerializer.Deserialize<ConfigMapper>(sr.ReadToEnd());                 
                }
                catch (Exception)
                {

                    throw;
                }
            }

            using (var sr = new StreamReader("..\\..\\..\\PackageCreationConfig.json"))
            {
                try
                {
                    packageCreationMapper = JsonSerializer.Deserialize<PackageCreationMapper>(sr.ReadToEnd());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}