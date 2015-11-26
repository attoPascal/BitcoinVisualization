using DAO;
using System.Collections.Generic;

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

        public List<Transaction> Transactions = new List<Transaction>();
    }
}