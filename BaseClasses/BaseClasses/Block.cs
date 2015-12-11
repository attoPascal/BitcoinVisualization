using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Bitcoin
{
    [Table]
    public class Block
    {
        [Column(IsPrimaryKey = true)]
        public string ID;

        [Column]
        public int Height;

        [Column]
        public int Timestamp;

        private EntitySet<Transaction> transactions;

        [Association(Storage = "transactions", OtherKey = "BlockID")]
        public EntitySet<Transaction> Transactions
        {
            get { return transactions; }
            set { transactions.Assign(value); }
        }
    }
}
