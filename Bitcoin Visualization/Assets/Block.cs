using DAO;
using System.Collections.Generic;
using System.Linq;

namespace Bitcoin
{
    public class Block
    {
        public string ID;
        public int Height;
        public int Timestamp;
        private BitcoinDAO dao;

        public Block(string id, int height, int timestamp, BitcoinDAO dao)
        {
            ID = id; Height = height; Timestamp = timestamp; this.dao = dao;
        }

        public List<string> TransactionIDs = new List<string>();
        private List<Transaction> transactions = null;
        public List<Transaction> Transactions
        {
            get
            {
                if (transactions == null)
                {
                    transactions = TransactionIDs.Select(id => dao.TransactionWithID(id)).ToList();
                }
                return transactions;
            }
        }
    }
}