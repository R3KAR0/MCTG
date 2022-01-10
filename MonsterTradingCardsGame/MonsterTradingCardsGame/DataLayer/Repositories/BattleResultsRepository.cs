using MonsterTradingCardsGame.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class BattleResultsRepository : IRepository<BattleResult>
    {
        NpgsqlConnection npgsqlConnection = null;
        public BattleResultsRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }

        public BattleResult? Add(BattleResult obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO battleResults (br_id,user1,user2, winner, battletime) VALUES ((@br_id), (@user1), (@user2), (@winner), (@battletime))", npgsqlConnection);

            cmd.Parameters.AddWithValue("br_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("user1", obj.User1.ToString());
            cmd.Parameters.AddWithValue("user2", obj.User2.ToString());
            if (obj.Winner == null)
            {
                cmd.Parameters.AddWithValue("winner", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("winner", obj.Winner);
            }
            cmd.Parameters.AddWithValue("battletime", new NpgsqlTypes.NpgsqlDateTime(obj.BattleTime));

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

        public bool Delete(BattleResult obj)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM battleResults WHERE br_id=@br_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("br_id", obj.Id.ToString());

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
            using var cmd = new NpgsqlCommand("DELETE FROM battleResults WHERE br_id=@br_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("br_id", id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public List<BattleResult> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM battleResults", npgsqlConnection);

            List<BattleResult> results = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(new BattleResult(
                            new Guid(reader.GetString(reader.GetOrdinal("br_id"))),
                            new Guid(reader.GetString(reader.GetOrdinal("user1"))),
                            new Guid(reader.GetString(reader.GetOrdinal("user2"))),
                            new Guid(reader.GetString(reader.GetOrdinal("winner"))),
                            reader.GetDateTime(reader.GetOrdinal("battletime"))));
                }
                return results;
            }
        }

        public BattleResult? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM battleResults WHERE br_id = @br_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("br_id", id.ToString());

            BattleResult? result;
            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        reader.Read();
                        result = new BattleResult(
                            new Guid(reader.GetString(reader.GetOrdinal("br_id"))),
                            new Guid(reader.GetString(reader.GetOrdinal("user1"))),
                            new Guid(reader.GetString(reader.GetOrdinal("user2"))),
                            new Guid(reader.GetString(reader.GetOrdinal("winner"))),
                            reader.GetDateTime(reader.GetOrdinal("battletime")));
                        return result;
                    } 
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<BattleResult> GetBattleResultsByUserId(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM battleResults WHERE user1=@u_id or user2=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", id.ToString());

            List<BattleResult> results = new List<BattleResult>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(new BattleResult(
                        new Guid(reader.GetString(reader.GetOrdinal("br_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("user1"))),
                        new Guid(reader.GetString(reader.GetOrdinal("user2"))),
                        new Guid(reader.GetString(reader.GetOrdinal("winner"))),
                        reader.GetDateTime(reader.GetOrdinal("battletime"))));
                }
                return results;
            }
        }

        public BattleResult? Update(BattleResult obj)
        {
            throw new NotImplementedException();
        }
    }
}
