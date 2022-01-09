using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using MonsterTradingCardsGame.Models;
using Npgsql;
using System.Text.Json;

namespace MonsterTradingCardsGame.Server.Controller
{
    public class UserController : IController
    {
        private static readonly Lazy<UserController> userController = new Lazy<UserController>(() => new UserController());

        public static IController GetInstance { get { return userController.Value; } }

        [EndPointAttribute("/users/login","POST")]
        public static JsonResponseDTO Login(string loginContent)
        {
            LoginDTO? loginDTO;
            bool valid;
            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    loginDTO = JsonSerializer.Deserialize<LoginDTO>(loginContent);
                    if (loginDTO == null)
                    {
                        throw new Exception();
                    }

                    valid = unit.UserRepository().CheckCredentials(loginDTO.Username, loginDTO.Password);
                }
                catch (Exception)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }

            if(valid)
            {
                return new JsonResponseDTO(JsonSerializer.Serialize(new LoginResponseDTO(SecurityHelper.CreateToken(loginDTO.Username))), System.Net.HttpStatusCode.OK);
            }
            return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

        }


        [EndPointAttribute("/users", "POST")]
        public static JsonResponseDTO Register(string registerString)
        {
            RegisterDTO? registerDTO;
            User newUser;
            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    registerDTO = JsonSerializer.Deserialize<RegisterDTO>(registerString);
                    if (registerDTO == null)
                    {
                        throw new Exception();
                    }
                    newUser = new User(registerDTO.Username, registerDTO.Password);


                    unit.UserRepository().Add(newUser);       
                }
                catch (PostgresException e)
                {
                    if(e.Code == Program.GetConfigMapper().PostgresDoubleEntry)
                    {
                        return new JsonResponseDTO("", System.Net.HttpStatusCode.Conflict);
                    }
                    Console.WriteLine(e.ErrorCode);
                }
                catch (Exception)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }

            return new JsonResponseDTO("", System.Net.HttpStatusCode.Created);
        }

    }
}
