using MonsterTradingCardsGame.DataLayer.Repositories;
using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly string connString;
        private readonly NpgsqlConnection npgsqlConnection;
        private bool disposedValue;
        private NpgsqlTransaction? sqlTran;

        #region Repository Declarations
        private UserRepository? userRepository = null;
        private TokenRepository? tokenRepository = null;
        private PackageRepository? packageRepository = null;
        private CardRepository? cardRepository = null;
        private DeckRepository? deckRepository = null;
        private DeckCardRepository? deckCardRepository = null;
        private BattleResultsRepository? statisticRepository = null;
        private UserSelectedDeckRepository? userSelectedDeckRepository = null;
        private TradeOfferRepository? tradeOfferRepository = null;

        #endregion

        #region RepositoryGetters
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

        public DeckCardRepository DeckCardRepository()
        {
            if (deckCardRepository == null)
            {
                deckCardRepository = new DeckCardRepository(npgsqlConnection);
            }
            return deckCardRepository;
        }

        public BattleResultsRepository StatisticRepository()
        {
            if (statisticRepository == null)
            {
                statisticRepository = new BattleResultsRepository(npgsqlConnection);
            }
            return statisticRepository;
        }

        public UserSelectedDeckRepository UserSelectedDeckRepository()
        {
            if (userSelectedDeckRepository == null)
            {
                userSelectedDeckRepository = new UserSelectedDeckRepository(npgsqlConnection);
            }
            return userSelectedDeckRepository;
        }

        public TradeOfferRepository TradeOfferRepository()
        {
            if (tradeOfferRepository == null)
            {
                tradeOfferRepository = new TradeOfferRepository(npgsqlConnection);
            }
            return tradeOfferRepository;
        }


        #endregion 

        public UnitOfWork()
        {
            var mapper = Program.GetConfigMapper();
            if (mapper == null) throw new NullReferenceException();

            connString = mapper.ConnectionString;

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
            //Log.Information("Transaction started");
            sqlTran = npgsqlConnection.BeginTransaction();
        }

        public void Commit()
        {
            //Log.Information("Commited changes");
            sqlTran?.Commit();
        }

        public void Rollback()
        {
            //Log.Information("Rollback started");
            sqlTran?.Rollback();
            sqlTran = null;
        }
    }
}
