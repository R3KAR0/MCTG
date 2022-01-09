using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer
{
    public interface IUnitOfWork
    {
            void CreateTransaction();
            void Commit();
            void Rollback();
            //void Save();
    }
}
