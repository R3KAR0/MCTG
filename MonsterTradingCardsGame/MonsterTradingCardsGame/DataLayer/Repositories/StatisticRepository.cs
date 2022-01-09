using MonsterTradingCardsGame.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class StatisticRepository : IRepository<BattleResult>
    {
        NpgsqlConnection npgsqlConnection = null;
        public PackageRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }

        public BattleResult? Add(BattleResult obj)
        {
            if (GetById(obj.UserId) != null)
            {
                Update(obj);
                return obj;
            }
            using var cmd = new NpgsqlCommand("INSERT INTO battleResults (br_id,user1,user2, winner, battletime,   valid_until) VALUES ((@u_id), (@valid_until))", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", obj.UserId.ToString());
            cmd.Parameters.AddWithValue("valid_until", new NpgsqlTypes.NpgsqlDateTime(obj.Validity));

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
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<BattleResult?> GetAll()
        {
            throw new NotImplementedException();
        }

        public BattleResult? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public BattleResult? Update(BattleResult obj)
        {
            throw new NotImplementedException();
        }
    }
}
