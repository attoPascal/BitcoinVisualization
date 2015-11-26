using DAO;
using System.Collections.Generic;

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

        public List<Output> Inputs = new List<Output>();
        public List<Output> Outputs = new List<Output>();
        public Block Block;
    }
}
