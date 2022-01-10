using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using MonsterTradingCardsGame.Models;
using Npgsql;
using Serilog;
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


                    var user = unit.UserRepository().Add(newUser);
                   if(user == null)
                    {
                        throw new Exception();
                    }
                    Log.Information($"Created User (username: {user.Username}) successfully");
                }
                catch (PostgresException e)
                {
                    var mapper = Program.GetConfigMapper();
                    if (mapper == null) throw new NullReferenceException();

                    if (e.Code == mapper.PostgresDoubleEntry)
                    {
                        Log.Error($"Double entry for {registerString}");
                        return new JsonResponseDTO("", System.Net.HttpStatusCode.Conflict);
                    }
                    Console.WriteLine(e.ErrorCode);
                }
                catch (Exception)
                {
                    unit.Rollback();
                    Log.Error($"Exception encountered at UserController.Register");
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }

            return new JsonResponseDTO("", System.Net.HttpStatusCode.Created);
        }

        [Authentification]
        [EndPointAttribute("/users", "GET")]
        public static JsonResponseDTO GetUserInformation(string token, string content)
        {
            User? user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);
            return new JsonResponseDTO(JsonSerializer.Serialize(new UserRepresentation(user.Username,user.Coins,user.Description, user.Elo)), System.Net.HttpStatusCode.OK);
        }

        [Authentification]
        [EndPointAttribute("/users/coins", "POST")]
        public static JsonResponseDTO GetCoins(string token, string content)
        {
            GetCoinsDTO? dto;
            try
            {
                dto = JsonSerializer.Deserialize<GetCoinsDTO>(content);
                if (dto == null || dto.Amount<0) throw new InvalidDataException();

                User? user = SecurityHelper.GetUserFromToken(token);
                if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

                User? res;
                using (var ouw = new UnitOfWork())
                {
                     res = ouw.UserRepository().UpdateCoins(user, dto.Amount);
                }
                if(res != null)
                {
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.Accepted);
                }
                return new JsonResponseDTO("", System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception)
            {
                return new JsonResponseDTO("", System.Net.HttpStatusCode.InternalServerError);
            }

        }

        [Authentification]
        [EndPointAttribute("/users", "PUT")]
        public static JsonResponseDTO UpdateUserInformation(string token, string content)
        {
            UserUpdateDTO? userUpdate;
            User? user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

            try
            {
                userUpdate = JsonSerializer.Deserialize<UserUpdateDTO>(content);
                if (userUpdate == null) throw new InvalidDataException();

                if(userUpdate.NewPassword != null)
                {
                    userUpdate.NewPassword = SecurityHelper.sha256_hash(userUpdate.NewPassword);
                }
                else
                {
                    userUpdate.NewPassword = user.Password;
                }

                using (var uow = new UnitOfWork())
                {
                    user = uow.UserRepository().Update(new User(user.Username,user.Id, userUpdate.NewPassword,user.Coins, userUpdate.NewProfileDescription, user.Elo));
                }
                if(user == null) throw new InvalidDataException();
                return new JsonResponseDTO(JsonSerializer.Serialize(new UserRepresentation(user.Username,user.Coins,user.Description, user.Elo)), System.Net.HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
            }
        }

    }
}
