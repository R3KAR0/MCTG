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

            cmd.Parameters.AddWithValue("p_id", obj.Id.ToString());
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

        public List<Package> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM packages", npgsqlConnection);

            List<Package> packages = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    packages.Add(new Package(new Guid(reader.GetString(reader.GetOrdinal("p_id"))), new Guid(reader.GetString(reader.GetOrdinal("buyer"))), reader.GetString(reader.GetOrdinal("p_description")), reader.GetInt32(reader.GetOrdinal("price")), reader.GetDateTime(reader.GetOrdinal("creationtime"))));
                }
                return packages;
            }
        }

        public Package? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM packages WHERE p_id=@p_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("p_id", id.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new Package(new Guid(reader.GetString(reader.GetOrdinal("p_id"))), new Guid(reader.GetString(reader.GetOrdinal("buyer"))), reader.GetString(reader.GetOrdinal("p_description")), reader.GetInt32(reader.GetOrdinal("price")), reader.GetDateTime(reader.GetOrdinal("creationtime")));
                }
            }
            return null;
        }

        public Package? Update(Package obj)
        {
            using var cmd = new NpgsqlCommand("UPDATE packages SET p_description=@p_description WHERE p_id=@p_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("p_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("p_description", obj.Description);

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();
            if (res != 0)
            {
                return GetById(obj.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
