using DAO;
using System.Collections.Generic;

namespace Bitcoin
{
    public class Address
    {
        public Address(string id, BitcoinDAO dao)
        {
            ID = id;
            this.dao = dao;
        }

        public string ID;
        private BitcoinDAO dao;

        public List<Output> Outputs = new List<Output>();
    }
}
