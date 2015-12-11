using DAO;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Bitcoin
{
    [Table]
    public class Transaction
    {
        [Column(IsPrimaryKey = true)]
        public string ID;

        [Column]
        public bool Coinbase;

        [Column]
        public string BlockID;

        private EntityRef<Block> block;
        private EntitySet<Output> outputs;
        private EntitySet<Output> inputs;

        [Association(Storage = "block", ThisKey = "BlockID")]
        public Block Block
        {
            get { return block.Entity; }
            set { block.Entity = value; }
        }

        [Association(Storage = "outputs", OtherKey = "TransactionID")]
        public EntitySet<Output> Outputs
        {
            get { return outputs; }
            set { outputs.Assign(value); }
        }

        [Association(Storage = "inputs", OtherKey = "InputTransactionID")]
        public EntitySet<Output> Inputs
        {
            get { return inputs; }
            set { inputs.Assign(value); }
        }
    }
}
