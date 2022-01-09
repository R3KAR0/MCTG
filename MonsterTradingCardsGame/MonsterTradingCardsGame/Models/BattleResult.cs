using MonsterTradingCardsGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class BattleResult : IJsonConvertable
    {
        public BattleResult(Guid user1, Guid user2, Guid winner)
        {
            Id = new Guid();
            User1 = user1;
            User2 = user2;
            BattleTime = DateTime.Now;

            if (winner != user1 && winner != user2) throw new InvalidDataException();

            Winner = winner;
        }

        public Guid Id { get; private set; }
        public Guid User1 { get; private set; }
        public Guid User2 { get; private set; }
        public DateTime BattleTime { get; private set; }
        public Guid Winner { get; private set; }


    }
}
