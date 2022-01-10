using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using MonsterTradingCardsGame.Exceptions;
using MonsterTradingCardsGame.Models;
using Npgsql;
using Serilog;
using System.Text.Json;

namespace MonsterTradingCardsGame.Server.Controller
{
    public class DeckController : IController
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
                            var card = unit.CardRepository().GetById(deckcard.CardId);
                            if(card != null)
                            {
                                deck.Cards.Add(card);
                            }                      
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
        [EndPointAttribute("/decks", "POST")]
        public static JsonResponseDTO CreateNewDeck(string token, string content)
        {
            Guid? userID = SecurityHelper.GetUserIdFromToken(token);
            if (userID == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    CreateDeckDTO? dto = JsonSerializer.Deserialize<CreateDeckDTO>(content);
                    if(dto == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                    unit.DeckRepository().Add(new Deck(Guid.NewGuid(),userID.Value,dto.Description,DateTime.Now));
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.Created);

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
            CardIdListDeckDTO? cardIdsDTO;
            Guid? userID = SecurityHelper.GetUserIdFromToken(token);
            if (userID == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    cardIdsDTO = JsonSerializer.Deserialize<CardIdListDeckDTO>(content);
                    if (cardIdsDTO == null) throw new InvalidDataException();
                    if (cardIdsDTO.CardIds.Count != Program.GetConfigMapper()?.DeckSize)
                    {
                        throw new InvalidDataException();
                    }
                    
                    var deck = unit.DeckRepository().GetById(cardIdsDTO.DeckId);
                    if (deck==null) throw new InvalidDataException();
                    if (deck.User != userID)
                    {
                        throw new NotAuthorizedException();
                    }
                    
                    foreach (var cardId in cardIdsDTO.CardIds)
                    {
                        var card = unit.CardRepository().GetById(cardId);
                        if(card == null) throw new InvalidDataException();
                        if(unit.TradeOfferRepository().GetById(cardId) != null)
                        {
                            throw new NotAuthorizedException();
                        }
                        if (card.Owner != userID)
                        {
                            throw new NotAuthorizedException();
                        }
                        unit.DeckCardRepository().Add(new DeckCard(deck.Id, card.Id));
                    }
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.Accepted);
                }
                catch (NotAuthorizedException)
                {
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);
                }
                catch (PostgresException e)
                {
                    var mapper = Program.GetConfigMapper();
                    if (mapper == null) throw new NullReferenceException();

                    if (e.Code == mapper.PostgresDoubleEntry)
                    {
                        Log.Error($"Double entry for Cards in Deck");
                        unit.Rollback();
                        return new JsonResponseDTO("", System.Net.HttpStatusCode.Conflict);
                    }
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
                catch (Exception)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }
        }

        [Authentification]
        [EndPointAttribute("/decks/select", "POST")]
        public static JsonResponseDTO SelectDeck(string token, string content) 
        {
            DeckSelectionDTO? deckSelectionDTO;
            var user = SecurityHelper.GetUserFromToken(token);
            if(user == null) throw new NotAuthorizedException();

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    deckSelectionDTO = JsonSerializer.Deserialize<DeckSelectionDTO>(content);
                    if (deckSelectionDTO == null) throw new InvalidDataException(); 
                    var deck = unit.DeckRepository().GetById(deckSelectionDTO.DeckId);
                    if (deck == null) throw new InvalidDataException();
                    if (deck.User != user.Id)
                    {
                        throw new NotAuthorizedException();
                    }
                    var existingEntry = unit.UserSelectedDeckRepository().GetById(user.Id);
                    
                    if (existingEntry == null)
                    {
                        unit.UserSelectedDeckRepository().Add(new(user.Id, deckSelectionDTO.DeckId));
                    }
                    else
                    {
                        unit.UserSelectedDeckRepository().Update(new(user.Id, deckSelectionDTO.DeckId));
                    }
                    

                    return new JsonResponseDTO("", System.Net.HttpStatusCode.Accepted);
                }
                catch (NotAuthorizedException)
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

        [Authentification]
        [EndPointAttribute("/decks/select", "GET")]
        public static JsonResponseDTO GetSelectDeck(string token, string content)
        {
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) throw new NotAuthorizedException();

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    var deckSelection = unit.UserSelectedDeckRepository().GetById(user.Id);
                    if(deckSelection!=null)
                    {
                        var deck = unit.DeckRepository().GetById(deckSelection.DeckId);
                        if(deck != null)
                        {
                            var deckcards = unit.DeckCardRepository().GetByDeckId(deck.Id);
                            if(deckcards != null)
                            {
                                List<Card> cards = new();
                                foreach(var deckCard in deckcards)
                                {
                                    var c = unit.CardRepository().GetById(deckCard.CardId);
                                    if (c == null) throw new InvalidDataException();
                                    cards.Add(c);
                                }
                                deck.Cards = cards;
                            }
                            return new JsonResponseDTO(JsonSerializer.Serialize(deck), System.Net.HttpStatusCode.OK);
                        }
                    }
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.NoContent);
                }
                catch (NotAuthorizedException)
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
