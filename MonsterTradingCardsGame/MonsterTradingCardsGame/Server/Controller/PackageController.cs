using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var package = new Package();
            User? user = SecurityHelper.GetUserFromToken(auth); 
            if(user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

            if(user.Coins - package.Price < 0) return new JsonResponseDTO("", System.Net.HttpStatusCode.PaymentRequired);


            using (var uow = new UnitOfWork())
            {
                try
                {
                    user.SetCoins(package.Price * -1);
                    uow.UserRepository().Update(user);
                    package.SetBuyerId(user.ID);
                    uow.PackageRepository().Add(package);
                    foreach (var card in package.Cards)
                    {
                        uow.CardRepository().Add(card);
                        uow.UserCardStackRepository().Add(new UserCardsStack(user.ID, card.Id));
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
