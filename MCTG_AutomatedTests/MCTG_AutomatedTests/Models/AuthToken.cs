using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server
{
    public class AuthToken
    {
        public AuthToken(Guid userId, DateTime validity)
        {
            UserId = userId;
            Validity = validity;
        }

        public Guid UserId { get; private set; }
        public DateTime Validity { get; private set; }

    }
}
