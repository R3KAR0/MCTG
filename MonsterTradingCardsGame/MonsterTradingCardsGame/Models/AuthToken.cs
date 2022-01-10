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
