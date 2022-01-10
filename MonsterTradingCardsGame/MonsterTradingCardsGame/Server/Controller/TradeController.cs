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
    public class TradeController : IController
    {
        private static readonly Lazy<TradeController> tradeController = new Lazy<TradeController>(() => new TradeController());

        public static IController GetInstance { get { return tradeController.Value; } }

        [Authentification]
        [EndPointAttribute("/tradingoffers", "GET")]
        public static JsonResponseDTO GetTradeOfferings(string token, string content)
        {
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    var results = unit.TradeOfferRepository().GetAll();
                    return new JsonResponseDTO(JsonSerializer.Serialize(new AllTradeOfferDTO(results)), System.Net.HttpStatusCode.OK);

                }
                catch (Exception)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }
        }

        [Authentification]
        [EndPointAttribute("/tradingoffers", "POST")]
        public static JsonResponseDTO CreateTradeOffer(string token, string content)
        {
            TradeOfferDTO? tradeOffer;
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    tradeOffer = JsonSerializer.Deserialize<TradeOfferDTO>(content);
                    if (tradeOffer == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

                    var card = unit.CardRepository().GetById(tradeOffer.CardId);
                    if (card == null) throw new InvalidDataException();
                    if (card.Owner.ToString() != user.Id.ToString()) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

                    var decksOfCard = unit.DeckCardRepository().GetByCardId(tradeOffer.CardId).ToList();
                    if (decksOfCard.Count > 0)
                    {
                        foreach (var deckEntry in decksOfCard)
                        {
                            unit.DeckCardRepository().Delete(deckEntry);
                        }
                    }

                    var results = unit.TradeOfferRepository().Add(new Models.TradeOffer(tradeOffer.CardId, user.Id, tradeOffer.DesiredType, tradeOffer.MinDamage));
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.Accepted);

                }
                catch (Exception)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }
        }

        [Authentification]
        [EndPointAttribute("/tradingoffers/trade", "POST")]
        public static JsonResponseDTO Trade(string token, string content)
        {
            TradingDTO? trade;
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    trade = JsonSerializer.Deserialize<TradingDTO>(content);
                    if (trade == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

                    var card = unit.CardRepository().GetById(trade.BuyerId);
                    if (card == null) throw new Exception();
                    if (card.Owner.ToString() != user.Id.ToString() || trade.BuyerId == trade.TradeId) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

                    var buyerCard = unit.CardRepository().GetById(trade.BuyerId);
                    var tradeCard = unit.CardRepository().GetById(trade.TradeId);
                    var tradeOffer = unit.TradeOfferRepository().GetById(trade.TradeId);

                    if(buyerCard == null || tradeCard == null || tradeOffer == null)
                    {
                        return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                    }
                    if(buyerCard.Type != tradeOffer.DesiredType || buyerCard.Damage < tradeOffer.MinDamage)
                    {
                        return new JsonResponseDTO("", System.Net.HttpStatusCode.NotAcceptable);
                    }
                    var swap = buyerCard.Owner.ToString();
                    buyerCard.Owner = tradeCard.Owner;
                    tradeCard.Owner = new Guid(swap);
                    
                    var updateOffer = unit.CardRepository().UpdateOwner(buyerCard);
                    var updateTradeCard = unit.CardRepository().UpdateOwner(tradeCard);
                    var delete = unit.TradeOfferRepository().Delete(tradeOffer);

                    if(updateOffer == null || updateTradeCard == null || delete == false)
                    {
                        throw new Exception();
                    }

                    return new JsonResponseDTO("", System.Net.HttpStatusCode.Accepted);

                }
                catch (Exception)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }
        }


        [Authentification]
        [EndPointAttribute("/tradingoffers", "DELETE")]
        public static JsonResponseDTO DeleteTradeOffer(string token, string content)
        {
            DeleteTradeOfferDTO? delete;
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    delete = JsonSerializer.Deserialize<DeleteTradeOfferDTO>(content);
                    if (delete == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);


                    var card = unit.CardRepository().GetById(delete.CardId);
                    if (card == null) throw new InvalidDataException();
                    if (card.Owner != user.Id) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

                    var tradeOffer = unit.TradeOfferRepository().GetById(delete.CardId);
                    if(tradeOffer == null) throw new InvalidDataException();

                    var result = unit.TradeOfferRepository().Delete(tradeOffer);
                    if(result)
                    {
                        return new JsonResponseDTO("", System.Net.HttpStatusCode.NoContent);
                    }
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.NotFound);

                }
                catch (Exception)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }
        }


        [Authentification]
        [EndPointAttribute("/sellingoffers", "GET")]
        public static JsonResponseDTO GetSellOfferings(string token, string content)
        {
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    var results = unit.StatisticRepository().GetBattleResultsByUserId(user.Id);
                    var wins = results.Where(result => result.Winner == user.Id).ToList().Count;
                    var loses = results.Where(result => result.Winner != user.Id).ToList().Count;
                    var draws = results.Where(result => result.Winner == null).ToList().Count;

                    var elo = user.Elo;
                    return new JsonResponseDTO(JsonSerializer.Serialize(new BattleResultsRepresentation(wins, loses, draws, elo)), System.Net.HttpStatusCode.OK);

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
