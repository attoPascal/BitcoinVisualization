using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Bitcoin
{
    [Table]
    public class Transaction
    {
        private string id;
        private bool coinbase;
        private string blockID;

        private EntityRef<Block> block = new EntityRef<Block>();
        private EntitySet<Output> outputs = new EntitySet<Output>();
        private EntitySet<Output> inputs = new EntitySet<Output>();

        [Column(IsPrimaryKey = true, Storage = "id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [Column(Storage = "coinbase")]
        public bool Coinbase
        {
            get { return coinbase; }
            set { coinbase = value; }
        }

        [Column(Storage = "blockID")]
        public string BlockID
        {
            get { return blockID; }
            set { blockID = value; }
        }

        [Association(Name = "BlockTransactions", Storage = "block", ThisKey = "BlockID", IsForeignKey = true)]
        public Block Block
        {
            get { return block.Entity; }
            set { block.Entity = value; }
        }

        [Association(Name = "TransactionOutputs", Storage = "outputs", OtherKey = "TransactionID")]
        public EntitySet<Output> Outputs
        {
            get { return outputs; }
            set { outputs.Assign(value); }
        }

        [Association(Name = "TransactionInputs", Storage = "inputs", OtherKey = "InputTransactionID")]
        public EntitySet<Output> Inputs
        {
            get { return inputs; }
            set { inputs.Assign(value); }
        }
    }
}
