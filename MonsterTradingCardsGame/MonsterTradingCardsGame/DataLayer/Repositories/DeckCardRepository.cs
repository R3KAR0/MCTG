using MonsterTradingCardsGame.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class DeckCardRepository : IRepository<DeckCard>
    {
        NpgsqlConnection npgsqlConnection = null;
        public DeckCardRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }

        public DeckCard? Add(DeckCard obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO deck_card (d_id, c_id, creationtime) VALUES ((@d_id), (@c_id), (@creationtime))", npgsqlConnection);

            cmd.Parameters.AddWithValue("d_id", obj.DeckId.ToString());
            cmd.Parameters.AddWithValue("c_id", obj.CardId.ToString());
            cmd.Parameters.AddWithValue("creationtime", new NpgsqlTypes.NpgsqlDateTime(obj.Timestamp));

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

        public bool Delete(DeckCard obj)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM deck_card WHERE d_id=@d_id AND c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("d_id", obj.DeckId.ToString());
            cmd.Parameters.AddWithValue("c_id", obj.CardId.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public bool Delete(Guid d_id, Guid c_id)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM deck_card WHERE d_id=@d_id AND c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("d_id", d_id.ToString());
            cmd.Parameters.AddWithValue("c_id", c_id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public List<DeckCard> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM deck_card", npgsqlConnection);

            List<DeckCard> deckCards = new List<DeckCard>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    deckCards.Add(new DeckCard(new Guid(reader.GetString(reader.GetOrdinal("d_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("c_id")))));
                }
                return deckCards;
            }
        }

        public DeckCard? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<DeckCard> GetByDeckId(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM deck_card WHERE d_id=@d_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("d_id", id.ToString());

            List<DeckCard> deckCards = new List<DeckCard>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    deckCards.Add(new DeckCard(new Guid(reader.GetString(reader.GetOrdinal("d_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("c_id")))));
                }
                return deckCards;
            }
        }

        public List<DeckCard> GetByCardId(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM deck_card WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", id.ToString());

            List<DeckCard> deckCards = new List<DeckCard>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    deckCards.Add(new DeckCard(new Guid(reader.GetString(reader.GetOrdinal("d_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("c_id")))));
                }
                return deckCards;
            }
        }


        #region unused
        public DeckCard? Update(DeckCard obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
