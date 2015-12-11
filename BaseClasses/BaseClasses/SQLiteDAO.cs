using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitcoin;
using System.Data.SQLite;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace DAO
{
    class SQLiteDAO : BitcoinDAO
    {
        private SQLiteConnection dbConnection;
        private DataContext context;

        public SQLiteDAO(string filePath)
        {
            var connectionString = $"DataSource={filePath};Version=3;";
            dbConnection = new SQLiteConnection(connectionString);
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
            return Addresses.Where(a => a.ID == id).First();
        }

        public Block BlockWithHeight(int height)
        {
            return Blocks.Where(b => b.Height == height).First();
        }

        public Block BlockWithID(string id)
        {
            return Blocks.Where(b => b.ID == id).First();
        }

        public Output OutputWithID(string id)
        {
            return Outputs.Where(o => o.ID == id).First();
        }

        public Transaction TransactionWithID(string id)
        {
            return Transactions.Where(t => t.ID == id).First();
        }
    }
}
