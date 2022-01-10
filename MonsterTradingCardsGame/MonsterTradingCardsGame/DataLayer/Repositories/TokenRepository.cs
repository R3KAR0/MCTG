using MonsterTradingCardsGame.Server;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class TokenRepository : IRepository<AuthToken>
    {
        NpgsqlConnection npgsqlConnection = null;
        public TokenRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }
        public AuthToken? Add(AuthToken obj)
        {

            if(GetById(obj.UserId) != null)
            {
                Update(obj);
                return obj;
            }
            using var cmd = new NpgsqlCommand("INSERT INTO auth_token (u_id, valid_until) VALUES ((@u_id), (@valid_until))", npgsqlConnection);

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

        public bool Delete(AuthToken obj)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM auth_token WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", obj.UserId.ToString());

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
            using var cmd = new NpgsqlCommand("DELETE FROM auth_token WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public List<AuthToken> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM auth_token", npgsqlConnection);

            List<AuthToken> tokens = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tokens.Add(new AuthToken(
                        new Guid(reader.GetString(reader.GetOrdinal("c_id"))),
                        reader.GetDateTime(reader.GetOrdinal("valid_until"))));
                }
                return tokens;
            }
        }

        public AuthToken? GetById(Guid? id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM auth_token WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", id.Value.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    AuthToken token = new AuthToken(new Guid(reader.GetString(reader.GetOrdinal("u_id"))), reader.GetDateTime(reader.GetOrdinal("valid_until")));
                    return token;
                }

            }
            return null;
        }

        public AuthToken? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM auth_token WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", id.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    AuthToken token = new AuthToken(new Guid(reader.GetString(reader.GetOrdinal("u_id"))), reader.GetDateTime(reader.GetOrdinal("valid_until")));
                    return token;
                }

            }
            return null;
        }

        public AuthToken? Update(AuthToken obj)
        {
            using var cmd = new NpgsqlCommand("UPDATE auth_token SET valid_until=@valid_until WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", obj.UserId.ToString());
            cmd.Parameters.AddWithValue("valid_until", new NpgsqlTypes.NpgsqlDateTime(obj.Validity));

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();
            if (res != 0)
            {
                return GetById(obj.UserId);
            }
            else
            {
                return null;
            }
        }
    }
}
