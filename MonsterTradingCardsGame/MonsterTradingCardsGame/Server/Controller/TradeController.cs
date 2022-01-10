using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using System.Text.Json;

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

                    var buyerCard = unit.CardRepository().GetById(trade.BuyerId);
                    if (buyerCard == null) throw new Exception();
                    var tradeCard = unit.CardRepository().GetById(trade.TradeId);
                    if (tradeCard == null) throw new Exception();

                    if (buyerCard.Owner.ToString() != user.Id.ToString() || trade.BuyerId == trade.TradeId || user.Id.ToString() == tradeCard.Owner.ToString()) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

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
                    var results = unit.SellingOfferRepository().GetAll();
                    if (results == null) throw new InvalidDataException();
                    return new JsonResponseDTO(JsonSerializer.Serialize(new SellingOffersDTO(results)), System.Net.HttpStatusCode.OK);

                }
                catch (Exception)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }
        }



        [Authentification]
        [EndPointAttribute("/sellingoffers", "POST")]
        public static JsonResponseDTO CreateSellingOffer(string token, string content)
        {
            CreateSellingOfferDTO? sellingOffer;
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    sellingOffer = JsonSerializer.Deserialize<CreateSellingOfferDTO>(content);
                    if (sellingOffer == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

                    var card = unit.CardRepository().GetById(sellingOffer.CardId);
                    if (card == null) throw new InvalidDataException();
                    if (card.Owner.ToString() != user.Id.ToString()) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

                    var decksOfCard = unit.DeckCardRepository().GetByCardId(sellingOffer.CardId).ToList();
                    if (decksOfCard.Count > 0)
                    {
                        foreach (var deckEntry in decksOfCard)
                        {
                            unit.DeckCardRepository().Delete(deckEntry);
                        }
                    }
                    var results = unit.SellingOfferRepository().Add(new Models.SellingOffer(sellingOffer.CardId, user.Id, sellingOffer.Price));
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
        [EndPointAttribute("/sellingoffers/buy", "POST")]
        public static JsonResponseDTO BuySellingOffer(string token, string content)
        {
            BuySellingOfferDTO? buyDTO;
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    buyDTO = JsonSerializer.Deserialize<BuySellingOfferDTO>(content);
                    if (buyDTO == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

                    var card = unit.CardRepository().GetById(buyDTO.CardId);
                    if (card == null) throw new InvalidDataException();
                    if (card.Owner.ToString() == user.Id.ToString()) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);
                    var test = unit.SellingOfferRepository().GetAll().ToList();
                    var selling = unit.SellingOfferRepository().GetById(buyDTO.CardId);
                    if (selling == null) throw new InvalidDataException();

                    if (user.Coins-selling.Price < 0)
                    {
                        return new JsonResponseDTO("", System.Net.HttpStatusCode.PaymentRequired);
                    }

                    var resUser = unit.UserRepository().UpdateCoins(user, selling.Price * -1);
                    var resSeller = unit.UserRepository().UpdateCoins(user, selling.Price);
                    card.Owner = user.Id;
                    var resCard = unit.CardRepository().UpdateOwner(card);

                    if (resUser == null || resCard == null || resSeller == null) throw new Exception();
      
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.Accepted);
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
