using DAO;
using System.Collections.Generic;
using System.Linq;

namespace Bitcoin
{
    public class Transaction
    {
        public string ID;
        public bool Coinbase;
        private BitcoinDAO dao;

        public Transaction(string id, bool coinbase, BitcoinDAO dao)
        {
            ID = id; Coinbase = coinbase; this.dao = dao;
        }

        public List<string> InputIDs = new List<string>();
        private List<Output> inputs = null;
        public List<Output> Inputs
        {
            get
            {
                if (inputs == null)
                {
                    inputs = InputIDs.Select(id => dao.OutputWithID(id)).ToList();
                }
                return inputs;
            }
        }

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

        public string BlockID;
        private Block block = null;
        public Block Block
        {
            get
            {
                if (block == null)
                {
                    block = dao.BlockWithID(BlockID);
                }
                return block;
            }
        }
    }
}
