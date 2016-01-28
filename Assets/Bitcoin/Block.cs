using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Bitcoin
{
    [Table]
    public class Block
    {
        private string id;
        private int height;
        private int timestamp;

        private EntitySet<Transaction> transactions = new EntitySet<Transaction>();

        [Column(IsPrimaryKey = true, Storage = "id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [Column(Storage = "height")]
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        [Column(Storage = "timestamp")]
        public int Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        [Association(Name = "BlockTransactions", Storage = "transactions", OtherKey = "BlockID")]
        public EntitySet<Transaction> Transactions
        {
            get { return transactions; }
            set { transactions.Assign(value); }
        }
    }
}
