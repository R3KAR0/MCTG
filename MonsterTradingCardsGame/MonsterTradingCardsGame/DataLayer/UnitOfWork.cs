using MonsterTradingCardsGame.DataLayer.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly string connString = Program.GetConfigMapper().ConnectionString;
        private readonly NpgsqlConnection npgsqlConnection;
        private bool disposedValue;
        private NpgsqlTransaction? sqlTran;

        private UserRepository? userRepository = null;
        private TokenRepository? tokenRepository = null;
        private PackageRepository? packageRepository = null;
        private CardRepository? cardRepository = null;
        private DeckRepository? deckRepository = null;

        public UserRepository UserRepository()
        {
            if(userRepository == null)
            {
                userRepository = new UserRepository(npgsqlConnection);
            }
            return userRepository;
        }

        public TokenRepository TokenRepository()
        {
            if (tokenRepository == null)
            {
                tokenRepository = new TokenRepository(npgsqlConnection);
            }
            return tokenRepository;
        }

        public PackageRepository PackageRepository()
        {
            if (packageRepository == null)
            {
                packageRepository = new PackageRepository(npgsqlConnection);
            }
            return packageRepository;
        }

        public CardRepository CardRepository()
        {
            if (cardRepository == null)
            {
                cardRepository = new CardRepository(npgsqlConnection);
            }
            return cardRepository;
        }

        public DeckRepository DeckRepository()
        {
            if (deckRepository == null)
            {
                deckRepository = new DeckRepository(npgsqlConnection);
            }
            return deckRepository;
        }

        public UnitOfWork()
        {
            npgsqlConnection = new NpgsqlConnection(connString);
            npgsqlConnection.Open();
            CreateTransaction();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(sqlTran!=null)
                    {
                        Commit();
                    }
                    npgsqlConnection?.Close();
                    npgsqlConnection?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void CreateTransaction()
        {
            sqlTran = npgsqlConnection.BeginTransaction();
        }

        public void Commit()
        {
            sqlTran?.Commit();
        }

        public void Rollback()
        {
            sqlTran?.Rollback();
            sqlTran = null;
        }
    }
}
