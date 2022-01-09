using MonsterTradingCardsGame.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class PackageRepository : IRepository<Package>
    {
        NpgsqlConnection npgsqlConnection = null;
        public PackageRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }
        public Package? Add(Package obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO packages (p_id, p_description, creationtime, price, buyer) VALUES ((@p_id), (@p_description), (@creationtime), (@price), (@buyer))", npgsqlConnection);

            cmd.Parameters.AddWithValue("p_id", obj.ID.ToString());
            cmd.Parameters.AddWithValue("p_description", obj.Description);
            cmd.Parameters.AddWithValue("creationtime", new NpgsqlTypes.NpgsqlDateTime(obj.CreationDate));
            cmd.Parameters.AddWithValue("price", obj.Price);
            cmd.Parameters.AddWithValue("buyer", obj.BuyerID.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();
            if (res != 0)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        public bool Delete(Package obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Package?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Package? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Package? Update(Package obj)
        {
            throw new NotImplementedException();
        }
    }
}
