using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        bool Delete(TEntity obj);
        bool Delete(Guid id);
        TEntity? GetById(Guid id);
        TEntity? Add(TEntity obj);
        TEntity? Update(TEntity obj);
        List<TEntity> GetAll();
    }
}
