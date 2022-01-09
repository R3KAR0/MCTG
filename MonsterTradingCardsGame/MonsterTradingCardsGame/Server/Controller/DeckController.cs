using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Controller
{
    public  class DeckController : IController
    {
        private static readonly Lazy<UserController> userController = new Lazy<UserController>(() => new UserController());

        public static IController GetInstance { get { return userController.Value; } }

        [Authentification]
        [EndPointAttribute("/decks", "GET")]
        public static JsonResponseDTO GetUserCards(string token, string loginContent)
        {
            Guid? userID = SecurityHelper.GetUserIdFromToken(token);
            if (userID == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    var decks = unit.DeckRepository().GetByUserId(userID.Value);
                    foreach (var deck in decks)
                    {
                        var deckCards = unit.DeckCardRepository().GetByDeckId(deck.Id);
                        foreach (var deckcard in deckCards)
                        {
                            deck.Cards.Add(unit.CardRepository().GetById(deckcard.CardId));
                        }
                    }
                    return new JsonResponseDTO(JsonSerializer.Serialize(new DecksDTO(decks)), System.Net.HttpStatusCode.OK);

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
