﻿using System;
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
        static List<Guid> queue = new List<Guid>();

        public static BattleLog Battle(Guid guid)
        { 
            lock(queueLock)
            {
                queue.Add(guid);
            }
            var res = Calculate(guid);
            return res;
        }

        public static BattleLog Calculate(Guid guid)
        {
            while(queue.Count < 2)
            {
                Thread.Sleep(300);
            }
            lock (queueLock)
            {
                var user1 = queue.First();
                queue.Remove(user1);
                var user2 = queue.First();
                queue.Remove(user2);
                var decks = GetDecks(user1, user2);
                var res = Calculate(user1, user2, decks.Item1, decks.Item2);

            }
            return new BattleLog(new List<string>(), new Guid(), new Guid(), null);
        }

        private static Tuple<List<Card>,List<Card>> GetDecks(Guid user1, Guid user2)
        {
           List<Card> user1Deck = new List<Card>();
            List<Card> user2Deck = new List<Card>();

            using (var uow = new UnitOfWork())
            {
                var deckid1 = uow.UserSelectedDeckRepository().GetById(user1)?.DeckId;
                if (deckid1 == null) throw new InvalidDataException();
                var deckCards = uow.DeckCardRepository().GetByDeckId(deckid1.Value);
                user1Deck = new List<Card>();
                foreach (var deckCard in deckCards)
                {
                    var card = uow.CardRepository().GetById(deckCard.CardId);
                    if(card == null) throw new InvalidDataException();
                    user1Deck.Add(card);
                }

                var deckid2 = uow.UserSelectedDeckRepository().GetById(user2)?.DeckId;
                if (deckid2 == null) throw new InvalidDataException();
                var deckCards2 = uow.DeckCardRepository().GetByDeckId(deckid2.Value);
                user2Deck = new List<Card>();
                foreach (var deckCard in deckCards2)
                {
                    var card = uow.CardRepository().GetById(deckCard.CardId);
                    if (card == null) throw new InvalidDataException();
                    user2Deck.Add(card);
                }
            }
            return Tuple.Create(user1Deck, user2Deck);
        }

        public static Tuple<Guid?, List<string>> Calculate(Guid user1, Guid user2, List<Card> user1Deck, List<Card> user2Deck)
        {
            List<string> roundLog = new List<string>();
            var random = new Random();
            int round = 1;
            var mapper = Program.GetRulesMapper();
            while (user1Deck.Count != 0 && user2Deck.Count != 0 && round != 100)
            {
                double user1multiplier = 1;
                double user2multiplier = 1;
                round++;
                var user1Card = user1Deck[random.Next(user1Deck.Count)];
                var user2Card = user2Deck[random.Next(user2Deck.Count)];


                var specialRulesUser1 = mapper.SpecialRules.Where(rule => rule.element == user1Card.Element && rule.type == user1Card.CardType && rule.kind == user1Card.Kind).ToList();
                var specialRulesUser2 = mapper.SpecialRules.Where(rule => rule.element == user2Card.Element && rule.type == user2Card.CardType && rule.kind == user2Card.Kind).ToList();

                var applicable1 = specialRulesUser1.Where(rule => rule.killkind == user2Card.Kind && rule.killtype == user2Card.CardType).ToList();
                if (applicable1.Count > 0)
                {
                    if (applicable1.Any(rule => rule.killelement == EElement.ALL || rule.killelement == user2Card.Element))
                    {
                        var res = $"BATTLE between {user1} and {user2} Round: {round}, {user1} won with special rule: {applicable1.Where(rule => rule.killelement == EElement.ALL || rule.killelement == user2Card.Element).First().KillText}";
                        Log.Information(res);
                        roundLog.Add(res);
                        user1Deck.Add(user2Card);
                        user2Deck.Remove(user2Card);
                        continue;
                    }
                }

                var applicable2 = specialRulesUser2.Where(rule => rule.killkind == user1Card.Kind && rule.killtype == user1Card.CardType).ToList();
                if (applicable2.Count > 0)
                {
                    if (applicable2.Any(rule => rule.killelement == EElement.ALL || rule.killelement == user1Card.Element))
                    {
                        var res = $"BATTLE between {user1} and {user2} Round: {round}, {user2} won with special rule: {applicable2.Where(rule => rule.killelement == EElement.ALL || rule.killelement == user1Card.Element).First().KillText}";
                        Log.Information(res);
                        roundLog.Add(res);
                        user2Deck.Add(user1Card);
                        user1Deck.Remove(user1Card);
                        continue;
                    }
                }

                if (user1Card.CardType != EType.MONSTER || user2Card.CardType != EType.SPELL)
                {
                    var strongs1 = mapper.Strongs.Exists(strong => strong.StrongElement == user1Card.Element && strong.WeakElement == user2Card.Element);
                    var strongs2 = mapper.Strongs.Exists(strong => strong.StrongElement == user2Card.Element && strong.WeakElement == user1Card.Element);

                    if (strongs1)
                    {
                        user1multiplier = 2;
                        user2multiplier = 0.5;
                    }

                    if (strongs2)
                    {
                        user1multiplier = 0.5;
                        user2multiplier = 2;
                    }
                }

                if (user1Card.Damage * user1multiplier < user2Card.Damage * user2multiplier)
                {
                    var res = $"BATTLE between {user1} and {user2} Round: {round}, {user2} won with {user2Card.Damage} Damage against {user1Card.Damage}";
                    Log.Information(res);
                    roundLog.Add(res);
                    user2Deck.Add(user1Card);
                    user1Deck.Remove(user1Card);
                    continue;
                }
                else if (user1Card.Damage * user1multiplier == user2Card.Damage * user2multiplier)
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
                    continue;
                }
            }
            Guid? winner = null;
            if (user1Deck.Count == 0)
            {
                winner = user2;
            }
            else if (user2Deck.Count == 0)
            {
                winner = user1;
            }

            if (winner != null)
            {
                Log.Information($"Winner: {winner}");
            }
            {
                Log.Information($"After 100 Rounds the battle ended -> DRAW!");
            }
            return new(winner, roundLog);
        }

    }
}
