using System.Collections.Generic;

namespace Bitcoin
{
    public class Block
    {
        public string ID;
        public int Height;
        public int Timestamp;

        public List<Transaction> Transactions;
    }
}