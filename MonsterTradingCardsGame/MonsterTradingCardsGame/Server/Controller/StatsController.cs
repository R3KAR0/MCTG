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
    public class StatsController : IController
    {
        private static readonly Lazy<PackageController> packageController = new Lazy<PackageController>(() => new PackageController());

        public static IController GetInstance { get { return packageController.Value; } }

        [Authentification]
        [EndPointAttribute("/stats", "GET")]
        public static JsonResponseDTO GetUserStats(string token, string content)
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
                    return new JsonResponseDTO(JsonSerializer.Serialize(new BattleResultsRepresentation(wins,loses,draws, elo)), System.Net.HttpStatusCode.OK);

                }
                catch (Exception)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);
                }
            }
        }

        [Authentification]
        [EndPointAttribute("/stats/scoreboard", "GET")]
        public static JsonResponseDTO GetScoreBoard(string token, string content)
        {
            Guid? userID = SecurityHelper.GetUserIdFromToken(token);
            if (userID == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.BadRequest);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    var users = unit.UserRepository().GetAll().OrderByDescending(user => user.Elo);
                    List<Tuple<string, int>> scoreboard = new List<Tuple<string, int>>();
                    foreach (var user in users)
                    {
                        scoreboard.Add(new(user.Username, user.Elo));
                    }
                    return new JsonResponseDTO(JsonSerializer.Serialize(new ScoreboardDTO(scoreboard)), System.Net.HttpStatusCode.OK);

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
