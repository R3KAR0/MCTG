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
        [EndPointAttribute("/tradings", "GET")]
        public static JsonResponseDTO GetTradeOfferings(string token, string content)
        {
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

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

        [Authentification]
        [EndPointAttribute("/sales", "GET")]
        public static JsonResponseDTO GetSellOfferings(string token, string content)
        {
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

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
