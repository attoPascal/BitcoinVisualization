using DAO;
using System.Collections.Generic;
using System.Linq;

namespace Bitcoin
{
    public class Address
    {
        public Address(string id, BitcoinDAO dao)
        {
            ID = id;
            this.dao = dao;
        }

        public string ID;

        private BitcoinDAO dao;

        public List<string> OutputIDs = new List<string>();
        private List<Output> outputs = null;
        public List<Output> Outputs
        {
            get
            {
                if (outputs == null)
                {
                    outputs = OutputIDs.Select(id => dao.OutputWithID(id)).ToList();
                }
                return outputs;
            }
        }

        public Transaction FirstTransaction
        {
            get
            {
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
    }
}
