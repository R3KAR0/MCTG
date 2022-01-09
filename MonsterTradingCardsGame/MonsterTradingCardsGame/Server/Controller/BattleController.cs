using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Controller
{
    public class BattleController : IController
    {
        private static readonly Lazy<PackageController> packageController = new Lazy<PackageController>(() => new PackageController());

        public static IController GetInstance { get { return packageController.Value; } }

        [Authentification]
        [EndPointAttribute("/battle", "POST")]
        public static JsonResponseDTO QueueForBattle(string token, string content)
        {
            var userId = SecurityHelper.GetUserIdFromToken(token);
            if (userId == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);



            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    var res = BattleHandler.Battle(userId);
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.OK);

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
