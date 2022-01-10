using MonsterTradingCardsGame.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.Repositories
{
    public class CardRepository : IRepository<Card>
    {
        NpgsqlConnection npgsqlConnection = null;
        public CardRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }
        public Card? Add(Card obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO cards (c_id, u_id, p_id, c_description, c_kind, c_type, c_element, creationtime, damage) VALUES ((@c_id), (@u_id),(@p_id), (@c_description), (@c_kind), (@c_type), (@c_element), (@creationtime), (@damage))", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("u_id", obj.Owner.ToString());
            cmd.Parameters.AddWithValue("p_id", obj.Package.ToString());
            cmd.Parameters.AddWithValue("c_description", obj.Description);
            cmd.Parameters.AddWithValue("c_kind", obj.Kind);
            cmd.Parameters.AddWithValue("c_type", obj.Type);
            cmd.Parameters.AddWithValue("c_element", obj.Element);
            cmd.Parameters.AddWithValue("creationtime", new NpgsqlTypes.NpgsqlDateTime(obj.Timestamp));
            cmd.Parameters.AddWithValue("damage", obj.Damage);

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

        public bool Delete(Card obj)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM cards WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", obj.Id.ToString());

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
            using var cmd = new NpgsqlCommand("DELETE FROM cards WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public List<Card> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM cards", npgsqlConnection);

            List<Card?> cardsOfUser = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cardsOfUser.Add(new Card(
                        new Guid(reader.GetString(reader.GetOrdinal("c_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("u_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("p_id"))),
                        reader.GetString(reader.GetOrdinal("c_description")),
                        reader.GetFieldValue<EType>(reader.GetOrdinal("c_type")),
                        reader.GetFieldValue<EKind>(reader.GetOrdinal("c_kind")),
                        reader.GetFieldValue<EElement>(reader.GetOrdinal("c_element")),
                        reader.GetInt32(reader.GetOrdinal("damage"))));
                }
                return cardsOfUser;
            }
        }

        public Card? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM cards WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", id.ToString());

            using (var reader = cmd.ExecuteReader())
            {
               if(reader.HasRows)
               {
                    reader.Read();
                    return new Card(
                        new Guid(reader.GetString(reader.GetOrdinal("c_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("u_id"))),
                        new Guid(reader.GetString(reader.GetOrdinal("p_id"))),
                        reader.GetString(reader.GetOrdinal("c_description")),
                        reader.GetFieldValue<EType>(reader.GetOrdinal("c_type")),
                        reader.GetFieldValue<EKind>(reader.GetOrdinal("c_kind")),
                        reader.GetFieldValue<EElement>(reader.GetOrdinal("c_element")),
                        reader.GetInt32(reader.GetOrdinal("damage")));
               }
                return null;
            }
        }

        public List<Card> GetByUserId(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM cards WHERE u_id=@u_id", npgsqlConnection);

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

        public Card? Update(Card obj)
        {
            using var cmd = new NpgsqlCommand("UPDATE cards SET c_description=@c_description WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("c_description", obj.Description);

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

        public Card? UpdateOwner(Card obj)
        {
            using var cmd = new NpgsqlCommand("UPDATE cards SET u_id=@u_id WHERE c_id=@c_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("c_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("u_id", obj.Owner);

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
