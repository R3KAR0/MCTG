using MonsterTradingCardsGame.DataLayer.DTO;
using MonsterTradingCardsGame.Exceptions;
using Serilog;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace MonsterTradingCardsGame.Server
{
    public class ConnectionHandler
    {
        public static void HandleClient(object client)
        {
            TcpClient tcpClient;
            try
            {
                tcpClient = (TcpClient)client;
            }
            catch (InvalidCastException)
            {
                Log.Fatal("Could not convert client object to TcpClient in ConnectionHandler.HandleClient!");
                throw;
            }

            var receivedMessage = ReadHTTPMessage(tcpClient);

            Log.Information($"Received message with requested URL:{receivedMessage.Url}, body:{receivedMessage.Body}");
            try
            {
                Tuple<string, EHTTPMethod> pathMethod = new Tuple<string, EHTTPMethod>(receivedMessage.Url, receivedMessage.HTTPMethod);
                Server.EndPointPaths.TryGetValue(pathMethod, out var path);
                if (path == null)
                {
                    throw new InvalidDataException();
                }

                MethodInfo method = Server.EndPointPaths[pathMethod].Item2;
                bool auth = true;
                object? res;
                if(Server.AuthentificationMethods.Contains(method))
                {
                    auth = SecurityHelper.VerifyToken(receivedMessage.Headers["Authorization"]);
                    if (!auth)
                    {
                        throw new NotAuthorizedException();
                    }
                    res = Server.EndPointPaths[pathMethod].Item2.Invoke(Server.EndPointPaths[pathMethod].Item1, new object[] { receivedMessage.Headers["Authorization"], receivedMessage.Body });
                }
                else
                {
                    res = Server.EndPointPaths[pathMethod].Item2.Invoke(Server.EndPointPaths[pathMethod].Item1, new object[] { receivedMessage.Body });
                }

                if (res == null) throw new InvalidDataException();
                var JsonDTO = res as JsonResponseDTO;
                if (JsonDTO != null )
                {
                    HTTPResponse response = new(JsonDTO.StatusCode, JsonDTO.Content);
                    Response(response, tcpClient);
                    Log.Information($"SENT response");
                }
                else
                {
                    throw new InvalidDataException();
                }

            }
            catch (InvalidDataException)
            {
                Response(new HTTPResponse(System.Net.HttpStatusCode.BadRequest, ""), tcpClient);
            }
            catch (NotAuthorizedException)
            {
                Response(new HTTPResponse(System.Net.HttpStatusCode.Forbidden, ""), tcpClient);
            }
            catch (Exception)
            {
                Response(new HTTPResponse(System.Net.HttpStatusCode.NotFound, ""), tcpClient);
            }
        }


        private static HTTPMessage ReadHTTPMessage(TcpClient client)
        {
            string data = "";
            string? line;

            var reader = new StreamReader(client.GetStream(), Encoding.UTF8);

            int counter = 0;
            while(counter != 1)
            {
                line = reader.ReadLine();
                if(line == null)
                {
                    break;
                }
                if(line == "")
                {
                    data += "\n";
                    counter += 1;
                    continue;
                }
                data += (line + "\n");
            }

            if (reader.Peek() > -1 && reader.Peek() == (int)'{')
            {

                int brackets = 0;
                char c;
                do
                {
                    c = (char)reader.Read();
                    if (c == -1)
                    {
                        break;
                    }
                    if (c == '{')
                    {
                        brackets += 1;
                    }
                    if (c == '}')
                    {
                        brackets -= 1;
                    }

                    data += c.ToString();

                } while (brackets != 0);
            }
            return HTTPParser.ParseHTTP(data);
        }

        public static void Response(HTTPResponse res, TcpClient tcpClient)
        {
            try
            {
                StreamWriter writer = new StreamWriter(tcpClient.GetStream(),Encoding.ASCII);
                writer.Write($"{res}");

                writer.Flush();
                writer.Close();
                tcpClient.Close();
            }
            catch (Exception)
            {

                return;
            }
            
        }
    }
}
