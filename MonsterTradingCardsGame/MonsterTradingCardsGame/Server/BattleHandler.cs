using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.DataLayer.DTO;
using MonsterTradingCardsGame.Models;
using Serilog;

namespace MonsterTradingCardsGame.Server
{
    public static class BattleHandler
    {
        private static object queueLock = new();
        private static object hardlock = new();
        static List<Guid?> queue = new List<Guid?>();

        public static async Task<BattleLog> Battle(Guid? guid)
        { 
            lock(queueLock)
            {
                queue.Add(guid);
            }
            var res = await Calculate(guid);
            return res;
        }

        public static async Task<BattleLog> Calculate(Guid? guid)
        {
            while(queue.Count < 2)
            {
                Thread.Sleep(300);
            }
            lock (queueLock)
            {

                queue.Remove(guid);
            }
            return new BattleLog(new List<string>(), new Guid(), new Guid(), null);
        }

        private static Tuple<Guid?,List<string>> Calculate(Guid user1, Guid user2)
        {
            /*List<Card> user1Deck = new List<Card>();
            List<Card> user2Deck = new List<Card>();
            List<string> roundLog = new List<string>();
            using (var uow = new UnitOfWork())
            {
                var deckCards = uow.DeckCardRepository().GetByDeckId(uow.UserRepository().GetById(user1).DeckId.Value);
                user1Deck = new List<Card>();
                foreach (var deckCard in deckCards)
                {
                    user1Deck.Add(uow.CardRepository().GetById(deckCard.CardId));
                }

                var deckCards2 = uow.DeckCardRepository().GetByDeckId(uow.UserRepository().GetById(user2).DeckId.Value);
                user2Deck = new List<Card>();
                foreach (var deckCard in deckCards2)
                {
                    user2Deck.Add(uow.CardRepository().GetById(deckCard.CardId));
                }
            }

            var random = new Random();
            int round = 1;
            while (user1Deck.Count != 0 && user2Deck.Count != 0 && round != 100)
            {
                round++;
                var user1Card = user1Deck[random.Next(user1Deck.Count)];
                var user2Card = user2Deck[random.Next(user2Deck.Count)];

                //Calc damage -> weaknesses
                if (user1Card.Damage < user2Card.Damage)
                {
                    var res = $"BATTLE between {user1} and {user2} Round: {round}, {user2} won with {user2Card.Damage} Damage against {user1Card.Damage}";
                    Log.Information(res);
                    roundLog.Add(res);
                    user2Deck.Add(user1Card);
                    user1Deck.Remove(user1Card);
                }
                else if (user1Card.Damage == user2Card.Damage)
                {
                    var res = $"BATTLE between {user1} and {user2} Round: {round}, DRAW";
                    Log.Information(res);
                    roundLog.Add(res);
                    continue;
                }
                else
                {
                    var res = $"BATTLE between {user1} and {user2} Round: {round}, {user1} won with {user1Card.Damage} Damage against {user2Card.Damage}";
                    Log.Information(res);
                    roundLog.Add(res);
                    user1Deck.Add(user2Card);
                    user2Deck.Remove(user2Card);
                } 
            }
            Guid? winner = null;
            if(user1Deck.Count == 0)
            {
                winner = user2;
            }
            else if (user2Deck.Count == 0)
            {
                winner = user1;
            }

            return new(winner, roundLog);*/
            throw new NotImplementedException();
        }

    }
}
