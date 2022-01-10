using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Server.Controller
{
    public class PackageController : IController
    {
        private static readonly Lazy<PackageController> packageController = new Lazy<PackageController>(() => new PackageController());

        public static IController GetInstance { get { return packageController.Value; } }

        [Authentification]
        [EndPointAttribute("/packages/buy", "POST")]
        public static JsonResponseDTO BuyPackage(string auth,string buyContent)
        {
            
            User? user = SecurityHelper.GetUserFromToken(auth);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

            var package = new Package(user.Id);
            if (user.Coins - package.Price < 0) return new JsonResponseDTO("", System.Net.HttpStatusCode.PaymentRequired);

            using (var uow = new UnitOfWork())
            {
                try
                {
                    user = uow.UserRepository().UpdateCoins(user, package.Price * -1);
                    uow.PackageRepository().Add(package);
                    foreach (var card in package.Cards)
                    {
                        uow.CardRepository().Add(card);
                    }
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.OK);
                }
                catch (Exception)
                {
                    uow.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.InternalServerError);
                }

            }
                
        }

    }
}
