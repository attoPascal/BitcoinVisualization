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

        public string AddressID;
        private Address address = null;
        public Address Address
        {
            get
            {
                if (address == null)
                {
                    address = dao.AddressWithID(AddressID);
                }
                return address;
            }
        }

        public string TransactionID;
        private Transaction transaction = null;
        public Transaction Transaction
        {
            get
            {
                if (transaction == null)
                {
                    transaction = dao.TransactionWithID(TransactionID);
                }
                return transaction;
            }
        }
    }
}
