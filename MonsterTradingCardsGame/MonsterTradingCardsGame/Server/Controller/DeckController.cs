using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using MonsterTradingCardsGame.Exceptions;
using MonsterTradingCardsGame.Models;
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
        public static JsonResponseDTO GetUserDecks(string token, string content)
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

        [Authentification]
        [EndPointAttribute("/decks", "PUT")]
        public static JsonResponseDTO ConfigureUserDeck(string token, string content)
        {
            CardIdListDeckDTO cardIdsDTO;
            Guid? userID = SecurityHelper.GetUserIdFromToken(token);
            if (userID == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    cardIdsDTO = JsonSerializer.Deserialize<CardIdListDeckDTO>(content);
                    if(cardIdsDTO.CardIds.Count != Program.GetConfigMapper().DeckSize)
                    {
                        throw new InvalidDataException();
                    }
                    var deck = unit.DeckRepository().GetById(cardIdsDTO.DeckId);
                    if(deck.UserId != userID)
                    {
                       throw new NotAuthorizedException();
                    }
                    foreach (var cardId in cardIdsDTO.CardIds)
                    {
                        var card = unit.CardRepository().GetById(cardId);
                        if (card.UserId != userID)
                        {
                            throw new NotAuthorizedException();
                        }
                        unit.DeckCardRepository().Add(new DeckCard(deck.Id, card.Id));                  
                    }
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.Accepted);
                }
                catch(NotAuthorizedException)
                {
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);
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
