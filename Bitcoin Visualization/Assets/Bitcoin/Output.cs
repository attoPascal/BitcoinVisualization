using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Bitcoin
{
    [Table]
    public class Output
    {
        private string id;
        private int n;
        private double value;
        private string type;

        private string addressID;
        private string transactionID;
        private string inputTransactionID;

        private EntityRef<Address> address = new EntityRef<Address>();
        private EntityRef<Transaction> transaction = new EntityRef<Transaction>();
        private EntityRef<Transaction> inputTransaction = new EntityRef<Transaction>();

        [Column(IsPrimaryKey = true, Storage = "id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [Column(Storage = "n")]
        public int N
        {
            get { return n; }
            set { n = value; }
        }

        [Column(Storage = "value")]
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        [Column(Storage = "type")]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        [Column(Storage = "addressID")]
        public string AddressID
        {
            get { return addressID; }
            set { addressID = value; }
        }

        [Column(Storage = "transactionID")]
        public string TransactionID
        {
            get { return transactionID; }
            set { transactionID = value; }
        }

        [Column(Storage = "inputTransactionID")]
        public string InputTransactionID
        {
            get { return inputTransactionID; }
            set { inputTransactionID = value; }
        }

        [Association(Name = "AddressOutputs", Storage = "address", ThisKey = "AddressID", IsForeignKey = true)]
        public Address Address
        {
            get { return address.Entity; }
            set { address.Entity = value; }
        }

        [Association(Name = "TransactionOutputs", Storage = "transaction", ThisKey = "TransactionID", IsForeignKey = true)]
        public Transaction Transaction
        {
            get { return transaction.Entity; }
            set { transaction.Entity = value; }
        }

        [Association(Name = "TransactionInputs", Storage = "inputTransaction", ThisKey = "InputTransactionID", IsForeignKey = true)]
        public Transaction InputTransaction
        {
            get { return inputTransaction.Entity; }
            set { inputTransaction.Entity = value; }
        }
    }
}
