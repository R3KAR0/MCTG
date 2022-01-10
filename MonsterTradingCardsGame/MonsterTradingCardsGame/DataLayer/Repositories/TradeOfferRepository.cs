using MonsterTradingCardsGame.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class TradeOfferRepository : IRepository<TradeOffer>
    {
        NpgsqlConnection npgsqlConnection = null;
        public TradeOfferRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }


        public TradeOffer? Add(TradeOffer obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO trading_offer (c_id, seller, creationtime, desiredType, minDamage) VALUES ((@c_id), (@seller),(@creationtime), (@desiredType), (@minDamage))", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", obj.CardId.ToString());
            cmd.Parameters.AddWithValue("seller", obj.SellerId.ToString());
            cmd.Parameters.AddWithValue("creationtime", new NpgsqlTypes.NpgsqlDateTime(obj.Timestamp));
            cmd.Parameters.AddWithValue("desiredType", obj.DesiredType);
            cmd.Parameters.AddWithValue("minDamage", obj.MinDamage);

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

        public bool Delete(TradeOffer obj)
        {
            try
            {
                using var cmd = new NpgsqlCommand("DELETE FROM trading_offer WHERE c_id=@c_id", npgsqlConnection);

                cmd.Parameters.AddWithValue("c_id", obj.CardId.ToString());

                cmd.Prepare();
                int res = cmd.ExecuteNonQuery();

                if (res != 0)
                {
                    return true;
                }
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }

        }

        public bool Delete(Guid id)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM trading_offer WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public List<TradeOffer> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM trading_offer", npgsqlConnection);

            List<TradeOffer> tradeOffers = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tradeOffers.Add(new TradeOffer(
                        new Guid(reader.GetString(reader.GetOrdinal("c_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("seller"))),
                        reader.GetDateTime(reader.GetOrdinal("creationtime")),
                        reader.GetFieldValue<EType>(reader.GetOrdinal("desiredType")),
                        reader.GetInt32(reader.GetOrdinal("minDamage"))));
                }
                return tradeOffers;
            }
        }

        public TradeOffer? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM trading_offer WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", id.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new TradeOffer(
                        new Guid(reader.GetString(reader.GetOrdinal("c_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("seller"))),
                        reader.GetDateTime(reader.GetOrdinal("creationtime")),
                        reader.GetFieldValue<EType>(reader.GetOrdinal("desiredType")),
                        reader.GetInt32(reader.GetOrdinal("minDamage")));
                }
                return null;
            }
        }

        public TradeOffer? Update(TradeOffer obj)
        {
            using var cmd = new NpgsqlCommand("UPDATE trading_offer SET desiredType=@desiredType, minDamage=@minDamage WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("desiredType", obj.DesiredType);
            cmd.Parameters.AddWithValue("minDamage", obj.MinDamage);

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
