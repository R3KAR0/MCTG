using MonsterTradingCardsGame.Models;
using Npgsql;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class UserSelectedDeckRepository : IRepository<UserSelectedDeck>
    {
        NpgsqlConnection npgsqlConnection;
        public UserSelectedDeckRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }

        public UserSelectedDeck? Add(UserSelectedDeck obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO user_selected_deck (u_id, d_id) VALUES ((@u_id), (@d_id))", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", obj.UserId.ToString());
            cmd.Parameters.AddWithValue("d_id", obj.DeckId);

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

        public bool Delete(UserSelectedDeck obj)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM user_selected_deck WHERE u_id=@u_id", npgsqlConnection);

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
            using var cmd = new NpgsqlCommand("DELETE FROM user_selected_deck WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public List<UserSelectedDeck> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM user_selected_deck", npgsqlConnection);

            List<UserSelectedDeck> userSelectedDecks = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    userSelectedDecks.Add(new UserSelectedDeck(new Guid(reader.GetString(reader.GetOrdinal("u_id"))), new Guid(reader.GetString(reader.GetOrdinal("d_id")))));
                }
                return userSelectedDecks;
            }
        }

        public UserSelectedDeck? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM user_selected_deck WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", id.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new UserSelectedDeck(new Guid(reader.GetString(reader.GetOrdinal("u_id"))), new Guid(reader.GetString(reader.GetOrdinal("d_id"))));
                }
            }
            return null;
        }

        public UserSelectedDeck? GetByDeckId(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM user_selected_deck WHERE d_id=@d_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("d_id", id.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new UserSelectedDeck(new Guid(reader.GetString(reader.GetOrdinal("u_id"))), new Guid(reader.GetString(reader.GetOrdinal("d_id"))));
                }
            }
            return null;
        }

        public UserSelectedDeck? Update(UserSelectedDeck obj)
        {
            using var cmd = new NpgsqlCommand("UPDATE user_selected_deck SET d_id=@d_id WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", obj.UserId.ToString());
            cmd.Parameters.AddWithValue("d_id", obj.DeckId.ToString());

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
