using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Bitcoin
{
    [Table]
    public class Output
    {
        [Column(IsPrimaryKey = true)]
        public string ID;

        [Column]
        public int N;

        [Column]
        public double Value;

        [Column]
        public string Type;

        [Column]
        public string AddressID;

        [Column]
        public string TransactionID;

        [Column]
        public string InputTransactionID;

        private EntityRef<Address> address;
        private EntityRef<Transaction> transaction;
        private EntityRef<Transaction> inputTransaction;

        [Association(Storage = "address", ThisKey = "AddressID")]
        public Address Address
        {
            get { return address.Entity; }
            set { address.Entity = value; }
        }

        [Association(Storage = "transaction", ThisKey = "TransactionID")]
        public Transaction Transaction
        {
            get { return transaction.Entity; }
            set { transaction.Entity = value; }
        }

        [Association(Storage = "inputTransaction", ThisKey = "InputTransactionID")]
        public Transaction InputTransaction
        {
            get { return inputTransaction.Entity; }
            set { inputTransaction.Entity = value; }
        }
    }
}
