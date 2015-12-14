using System.Linq;
using Bitcoin;
using System.Data.Linq;
using System.Data.Common;
using Mono.Data.Sqlite;

namespace DAO
{
    class SQLiteDAO : BitcoinDAO
    {
        private DbConnection dbConnection;
        private DataContext context;

        public SQLiteDAO(string filePath)
        {
            var connectionString = "DbLinqProvider=SQLite;Data Source=" + filePath + ";Version=3;";
            dbConnection = new SqliteConnection(connectionString);
            context = new DataContext(dbConnection);
        }

        public IQueryable<Address> Addresses
        {
            get
            {
                return context.GetTable<Address>();
            }
        }

        public IQueryable<Block> Blocks
        {
            get
            {
                return context.GetTable<Block>();
            }
        }

        public IQueryable<Output> Outputs
        {
            get
            {
                return context.GetTable<Output>();
            }
        }

        public IQueryable<Transaction> Transactions
        {
            get
            {
                return context.GetTable<Transaction>();
            }
        }

        public Address AddressWithID(string id)
        {
            return Addresses.Where(a => a.ID == id).Single();
        }

        public Block BlockWithHeight(int height)
        {
            return Blocks.Where(b => b.Height == height).Single();
        }

        public Block BlockWithID(string id)
        {
            return Blocks.Where(b => b.ID == id).Single();
        }

        public Output OutputWithID(string id)
        {
            return Outputs.Where(o => o.ID == id).Single();
        }

        public Transaction TransactionWithID(string id)
        {
            return Transactions.Where(t => t.ID == id).Single();
        }
    }
}
