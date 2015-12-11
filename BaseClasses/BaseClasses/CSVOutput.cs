using DAO;

namespace Bitcoin
{
    public class CSVOutput
    {
        public string ID;
        public int N;
        public double Value;
        public string Type;
        private CSVDAO dao;

        public CSVOutput(string id, int n, double value, string type, CSVDAO dao)
        {
            ID = id; N = n; Value = value; Type = type; this.dao = dao;
        }

        public string AddressID;
        private CSVAddress address = null;
        public CSVAddress Address
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
        private CSVTransaction transaction = null;
        public CSVTransaction Transaction
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
