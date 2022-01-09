using MonsterTradingCardsGame.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class DeckRepository : IRepository<Deck>
    {
        NpgsqlConnection npgsqlConnection = null;
        public DeckRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }

        public Deck? Add(Deck obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Deck obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Deck?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Deck? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Deck? Update(Deck obj)
        {
            throw new NotImplementedException();
        }
    }
}
