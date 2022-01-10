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

        public static Dictionary<Tuple<string,EHTTPMethod>, Tuple<Type?, MethodInfo>> EndPointPaths = new();
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
                    EndPointPaths.Add(new Tuple<string,EHTTPMethod>(attr.Path, attr.HTTPMethod), new Tuple<Type?, MethodInfo>(method.DeclaringType, method));

                    var declare = method.DeclaringType;
                    if (declare == null) throw new NullReferenceException();
                    Log.Information($"Registered Endpoint: {attr.Path} HTTPMethod: {attr.HTTPMethod} Method: {method.Name} in {declare.Name}");
                }
            }

            var authMethods = Assembly.GetExecutingAssembly().GetTypes()
                                  .SelectMany(t => t.GetMethods())
                                  .Where(m => m.GetCustomAttributes(typeof(Authentification), false).Length > 0)
                                  .ToList();
            foreach (var method in authMethods)
            {
                if (method == null) throw new NullReferenceException();
                var authDeclaringType = method.DeclaringType;
                if (authDeclaringType == null) throw new NullReferenceException();

                AuthentificationMethods.Add(method);
                Log.Information($"Authentification registered for {method.Name} in {authDeclaringType.Name}");
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
