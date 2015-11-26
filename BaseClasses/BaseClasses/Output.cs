using DAO;

namespace Bitcoin
{
    public class Output
    {
        public string ID;
        public int N;
        public double Value;
        public string Type;
        private BitcoinDAO dao;

        public Output(string id, int n, double value, string type, BitcoinDAO dao)
        {
            ID = id; N = n; Value = value; Type = type; this.dao = dao;
        }

        public Address Address;
        public Transaction Transaction;
    }
}
