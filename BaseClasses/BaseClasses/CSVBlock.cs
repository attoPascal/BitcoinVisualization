using DAO;
using System.Collections.Generic;
using System.Linq;

namespace Bitcoin
{
    public class CSVBlock
    {
        public string ID;
        public int Height;
        public int Timestamp;
        private CSVDAO dao;

        public CSVBlock(string id, int height, int timestamp, CSVDAO dao)
        {
            ID = id; Height = height; Timestamp = timestamp; this.dao = dao;
        }

        public List<string> TransactionIDs = new List<string>();
        private List<CSVTransaction> transactions = null;
        public List<CSVTransaction> Transactions
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
