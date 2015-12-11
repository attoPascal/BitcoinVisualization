using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Bitcoin
{
    [Table]
    public class Address
    {
        [Column(IsPrimaryKey = true)]
        public string ID;

        private EntitySet<Output> outputs;

        [Association(Storage = "outputs", OtherKey = "AddressID")]
        public EntitySet<Output> Outputs
        {
            get { return outputs; }
            set { outputs.Assign(value); }
        }

        public Transaction FirstTransaction
        {
            get
            {
                // TODO: linq
                var firstTx = Outputs.First().Transaction;
                foreach (var output in Outputs)
                {
                    var tx = output.Transaction;
                    if (tx.Block.Height < firstTx.Block.Height)
                    {
                        firstTx = tx;
                    }
                }
                return firstTx;
            }
        }

        public double Balance
        {
            get
            {
                // TODO: correct implementation
                var balance = 0.0;
                foreach (var output in Outputs)
                {
                    balance += output.Value;
                }
                return balance;
            }
        }
    }
}
