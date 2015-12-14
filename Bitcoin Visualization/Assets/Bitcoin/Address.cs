using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Bitcoin
{
    [Table]
    public class Address
    {
        private string id;
        private EntitySet<Output> outputs = new EntitySet<Output>();

        [Column(IsPrimaryKey = true, Storage = "id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [Association(Name = "AddressOutputs", Storage = "outputs", OtherKey = "AddressID")]
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
                //TODO: correct implementation
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
