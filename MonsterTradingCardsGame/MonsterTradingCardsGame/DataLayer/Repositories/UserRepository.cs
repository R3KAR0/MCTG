using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Server;
using Npgsql;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class UserRepository : IRepository<User>
    {
        NpgsqlConnection npgsqlConnection;
        public UserRepository(NpgsqlConnection npgsqlConnection) 
        {
            this.npgsqlConnection = npgsqlConnection;
        }
        public User? Add(User obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO users (u_id, username, u_password, coins, u_description, elo) VALUES ((@u_id), (@username), (@u_password), (@coins), (@u_description), (@elo))", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("username", obj.Username);
            cmd.Parameters.AddWithValue("u_password", obj.Password);
            cmd.Parameters.AddWithValue("coins", obj.Coins);
            cmd.Parameters.AddWithValue("u_description", obj.Description);
            cmd.Parameters.AddWithValue("elo", obj.Elo);

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

        public bool Delete(User obj)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM users WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", obj.Id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if(res != 0)
            {
                return true;
            }
            return false;
        }

        public bool Delete(Guid id)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM users WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public List<User> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM users", npgsqlConnection);

            List<User> users = new();
            using (var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    users.Add(new User(reader.GetString(reader.GetOrdinal("username")), new Guid(reader.GetString(reader.GetOrdinal("u_id"))), reader.GetString(reader.GetOrdinal("u_password")), reader.GetInt32(reader.GetOrdinal("coins")), reader.GetString(reader.GetOrdinal("u_description")), reader.GetInt32(reader.GetOrdinal("elo"))));
                }
                return users;
            }
        }

        public User? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM users WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", id.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new User(reader.GetString(reader.GetOrdinal("username")), new Guid(reader.GetString(reader.GetOrdinal("u_id"))), reader.GetString(reader.GetOrdinal("u_password")), reader.GetInt32(reader.GetOrdinal("coins")), reader.GetString(reader.GetOrdinal("u_description")), reader.GetInt32(reader.GetOrdinal("elo")));
                }
            }
            return null;
        }

        public User? GetByUsername(string username)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM users WHERE username=@username", npgsqlConnection);

            cmd.Parameters.AddWithValue("username", username);

            using (var reader = cmd.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    User user = new User(reader.GetString(reader.GetOrdinal("username")), new Guid(reader.GetString(reader.GetOrdinal("u_id"))), reader.GetString(reader.GetOrdinal("u_password")), reader.GetInt32(reader.GetOrdinal("coins")), reader.GetString(reader.GetOrdinal("u_description")), reader.GetInt32(reader.GetOrdinal("elo")));
                    return user;
                }

            }
            return null;
        }

        public User? Update(User obj)
        {
            using var cmd = new NpgsqlCommand("UPDATE users SET u_password=@u_password,u_description=@u_description WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("u_password", obj.Password);
            cmd.Parameters.AddWithValue("u_description", obj.Description);

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
        public bool CheckCredentials(string username, string password)
        {
            using var cmd = new NpgsqlCommand("SELECT u_id FROM users WHERE username=@username AND u_password=@u_password", npgsqlConnection);

            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("u_password", SecurityHelper.sha256_hash(password));

            cmd.Prepare();
            using (var reader = cmd.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public User? UpdateElo(User obj, int change)
        {
            using var cmd = new NpgsqlCommand("UPDATE users SET elo=@elo WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("elo", obj.Elo+change);

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

        public User? UpdateCoins(User obj, int change)
        {
            using var cmd = new NpgsqlCommand("UPDATE users SET coins=@coins WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("coins", obj.Coins + change);

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
