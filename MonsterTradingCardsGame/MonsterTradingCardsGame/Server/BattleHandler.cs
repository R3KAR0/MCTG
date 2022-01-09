using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.DataLayer.DTO;

namespace MonsterTradingCardsGame.Server
{
    public static class BattleHandler
    {

        public static event Func<object, EventArgs, Task> battlevent;
        private static object queueLock = new();
        static List<Guid> queue = new List<Guid>();

        public static async Task<BattleLog> Battle(Guid guid)
        {
            lock(queueLock)
            {
                queue.Add(guid);
            }
            var res = await Calculate(guid);
            return res
        }

        public static async Task<BattleLog> Calculate(Guid guid)
        {
            while(queue.Count < 2)
            {
                Thread.Wait(1000);
            }
            return new BattleLog();
        }

    }
}
