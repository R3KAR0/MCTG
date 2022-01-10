using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    internal class SellingOfferRepository : IRepository<SellingOffer>
    {
        NpgsqlConnection npgsqlConnection = null;
        public SellingOfferRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }
        public SellingOffer? Add(SellingOffer obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(SellingOffer obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<SellingOffer> GetAll()
        {
            throw new NotImplementedException();
        }

        public SellingOffer? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public SellingOffer? Update(SellingOffer obj)
        {
            throw new NotImplementedException();
        }
    }
}
