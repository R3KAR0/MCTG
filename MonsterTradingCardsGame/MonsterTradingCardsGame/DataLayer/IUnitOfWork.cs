namespace MonsterTradingCardsGame.DataLayer
{
    public interface IUnitOfWork
    {
            void CreateTransaction();
            void Commit();
            void Rollback();
    }
}
