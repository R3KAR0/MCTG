using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using MonsterTradingCardsGame.Models;
using System.Text.Json;

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
            var user = SecurityHelper.GetUserFromToken(token);
            if (user == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.Forbidden);

            using (UnitOfWork unit = new UnitOfWork())
            {
                try
                {
                    var selectedDeck = unit.UserSelectedDeckRepository().GetById(user.Id);
                    if (selectedDeck == null) throw new InvalidDataException();

                    var deck = unit.DeckRepository().GetById(selectedDeck.DeckId);
                    if (deck == null) throw new InvalidDataException();

                    var deckCards = unit.DeckCardRepository().GetByDeckId(selectedDeck.DeckId);
                    if (deckCards == null) throw new InvalidDataException();

                    List<Card> userCards = new();
                    try
                    {
                        foreach (var item in deckCards)
                        {
                            var card = unit.CardRepository().GetById(item.CardId);
                            if(card == null) throw new InvalidDataException();
                            userCards.Add(card);
                        }
                    }
                    catch (Exception)
                    {
                        return new JsonResponseDTO("", System.Net.HttpStatusCode.PreconditionRequired);
                    }

                    var mapper = Program.GetConfigMapper();
                    if (mapper == null) throw new NullReferenceException();
                    if(userCards.Count != mapper.DeckSize)
                    {
                        return new JsonResponseDTO("", System.Net.HttpStatusCode.PreconditionRequired);
                    }

                    var res = BattleHandler.Battle(userCards,user.Id);

                    if(res == null) return new JsonResponseDTO("", System.Net.HttpStatusCode.AlreadyReported);

                    return new JsonResponseDTO(JsonSerializer.Serialize(res), System.Net.HttpStatusCode.OK);

                }
                catch (InvalidDataException)
                {
                    unit.Rollback();
                    return new JsonResponseDTO("", System.Net.HttpStatusCode.PreconditionRequired);
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
