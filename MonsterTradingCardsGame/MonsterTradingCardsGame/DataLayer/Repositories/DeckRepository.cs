using MonsterTradingCardsGame.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class DeckRepository : IRepository<Deck>
    {
        NpgsqlConnection npgsqlConnection = null;
        public DeckRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }

        public Deck? Add(Deck obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO deck (d_id, d_description, creationtime, u_id) VALUES ((@d_id), (@d_description), (@creationtime), (@u_id))", npgsqlConnection);

            cmd.Parameters.AddWithValue("d_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("d_description", obj.Description);
            cmd.Parameters.AddWithValue("creationtime", new NpgsqlTypes.NpgsqlDateTime(obj.CreationDate));
            cmd.Parameters.AddWithValue("u_id", obj.UserId);

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

        public bool Delete(Deck obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Deck?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Deck? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Deck? GetByUserId(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM decks WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", id.ToString());

            List<Card> cards = new List<Card>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cards.Add(new Card(
                        new Guid(reader.GetString(reader.GetOrdinal("c_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("u_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("p_id"))),
                        reader.GetString(reader.GetOrdinal("c_description")),
                        reader.GetFieldValue<EType>(reader.GetOrdinal("c_type")),
                        reader.GetFieldValue<EKind>(reader.GetOrdinal("c_kind")),
                        reader.GetFieldValue<EElement>(reader.GetOrdinal("c_element")),
                        reader.GetInt32(reader.GetOrdinal("damage"))));
                }
                return cards;
            }
        }

        public Deck? Update(Deck obj)
        {
            throw new NotImplementedException();
        }
    }
}
