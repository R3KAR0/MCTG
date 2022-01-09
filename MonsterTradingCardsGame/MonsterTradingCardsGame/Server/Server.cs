using MonsterTradingCardsGame.Server.Controller;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server
{
    public class Server
    {
        private static readonly Lazy<Server> server = new Lazy<Server>(() => new Server());

        public static Server Instance { get { return server.Value; } }

        public static Dictionary<Tuple<string,EHTTPMethod>, Tuple<IController, MethodInfo>> EndPointPaths = new();
        public static List<MethodInfo> AuthentificationMethods = new();
        private Server()
        {
            Initialize();
        }

        private void Initialize()
        {
            Log.Information("SERVER initialization started!");
            var methods = Assembly.GetExecutingAssembly().GetTypes()
                                  .SelectMany(t => t.GetMethods())
                                  .Where(m => m.GetCustomAttributes(typeof(EndPointAttribute), false).Length > 0)
                                  .ToList();
            foreach (var method in methods)
            {
                EndPointAttribute attr = (EndPointAttribute)method.GetCustomAttributes(typeof(EndPointAttribute), true)[0];
                if (attr != null)
                {
                    object? controller = method?.DeclaringType?.GetProperty(name: "GetInstance")?.GetValue(null);
                    if(controller == null)
                    {
                        continue;
                    }
                    EndPointPaths.Add(new Tuple<string,EHTTPMethod>(attr.Path, attr.HTTPMethod), new Tuple<IController, MethodInfo>(method.DeclaringType as IController, method));
                    Log.Information($"Registered Endpoint: {attr.Path} HTTPMethod: {attr.HTTPMethod} Method: {method.Name} in {method.DeclaringType.Name}");
                }
            }

            var authMethods = Assembly.GetExecutingAssembly().GetTypes()
                                  .SelectMany(t => t.GetMethods())
                                  .Where(m => m.GetCustomAttributes(typeof(Authentification), false).Length > 0)
                                  .ToList();
            foreach (var method in authMethods)
            {
                AuthentificationMethods.Add(method);
                Log.Information($"Authentification registered for {method.Name} in {method.DeclaringType.Name}");
            }



            Log.Information("SERVER initialization finished!");
        }

        public void Run()
        {
            TcpListener loginListener = new TcpListener(IPAddress.Any, 8000);
            loginListener.Start(10);
            Log.Information("SERVER TCPListener started!");
            try
            {
                while (true)
                {
                    Log.Information("SERVER is now ready for connections!");
                    TcpClient client = loginListener.AcceptTcpClient();
                    Log.Information("Client connected!");

                    Thread t = new(new ParameterizedThreadStart(ConnectionHandler.HandleClient));
                    t.Start(client);
                }

            }
            catch (Exception e)
            {
                Log.Fatal($"Exception: {e.ToString} was thrown in Server.Run()!");
                Console.WriteLine(e.ToString());
            }

        }
    }
}
