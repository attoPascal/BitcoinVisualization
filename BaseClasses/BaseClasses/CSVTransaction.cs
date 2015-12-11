using DAO;
using System.Collections.Generic;
using System.Linq;

namespace Bitcoin
{
    public class CSVTransaction
    {
        public string ID;
        public bool Coinbase;
        private CSVDAO dao;

        public CSVTransaction(string id, bool coinbase, CSVDAO dao)
        {
            ID = id; Coinbase = coinbase; this.dao = dao;
        }

        public List<string> InputIDs = new List<string>();
        private List<CSVOutput> inputs = null;
        public List<CSVOutput> Inputs
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
        private List<CSVOutput> outputs = null;
        public List<CSVOutput> Outputs
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
        private CSVBlock block = null;
        public CSVBlock Block
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
