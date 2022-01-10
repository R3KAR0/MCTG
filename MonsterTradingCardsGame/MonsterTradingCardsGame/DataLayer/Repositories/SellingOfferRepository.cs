using MonsterTradingCardsGame.Models;
using Npgsql;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class SellingOfferRepository : IRepository<SellingOffer>
    {
        NpgsqlConnection npgsqlConnection;
        public SellingOfferRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }
        public SellingOffer? Add(SellingOffer obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO selling_offer (c_id, seller, creationtime, price) VALUES ((@c_id), (@seller), (@creationtime), (@price))", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", obj.CardId.ToString());
            cmd.Parameters.AddWithValue("seller", obj.SellerId.ToString());
            cmd.Parameters.AddWithValue("creationtime", new NpgsqlTypes.NpgsqlDateTime(obj.Timestamp));
            cmd.Parameters.AddWithValue("price", obj.Price);

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

        public bool Delete(SellingOffer obj)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM selling_offer WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", obj.CardId.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public bool Delete(Guid id)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM selling_offer WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public List<SellingOffer> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM selling_offer", npgsqlConnection);

            List<SellingOffer> sellingOffers = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    sellingOffers.Add(new SellingOffer(
                        new Guid(reader.GetString(reader.GetOrdinal("c_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("seller"))),
                        reader.GetDateTime(reader.GetOrdinal("creationtime")),
                        reader.GetInt32(reader.GetOrdinal("price"))));
                }
                return sellingOffers;
            }
        }

        public SellingOffer? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM selling_offer WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", id.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new SellingOffer(
                        new Guid(reader.GetString(reader.GetOrdinal("c_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("seller"))),
                        reader.GetDateTime(reader.GetOrdinal("creationtime")),
                        reader.GetInt32(reader.GetOrdinal("price")));
                }
                return null;
            }
        }

        public SellingOffer? Update(SellingOffer obj)
        {
            using var cmd = new NpgsqlCommand("UPDATE selling_offer SET price=@price WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("desiredType", obj.Price);

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();
            if (res != 0)
            {
                return GetById(obj.CardId);
            }
            else
            {
                return null;
            }
        }
    }
}
