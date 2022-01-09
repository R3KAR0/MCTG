using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Controller
{
    public class CardController : IController
    {
        private static readonly Lazy<UserController> userController = new Lazy<UserController>(() => new UserController());

        public static IController GetInstance { get { return userController.Value; } }

        [Authentification]
        [EndPointAttribute("/cards", "GET")]
        public static JsonResponseDTO GetUserCards(string token, string loginContent)
        {
            Guid? userID = SecurityHelper.GetUserIdFromToken(token); 
            if(userID == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    var cards = unit.CardRepository().GetByUserId(userID.Value);
                    return new JsonResponseDTO(JsonSerializer.Serialize(new UserCardsDTO(cards)), System.Net.HttpStatusCode.OK);

                }
                catch (Exception)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }

        }
    }
}
