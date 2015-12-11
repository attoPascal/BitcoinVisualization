using DAO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bitcoin
{
    public class CSVAddress
    {
        public CSVAddress(string id, CSVDAO dao)
        {
            ID = id;
            this.dao = dao;
        }

        public string ID;

        private CSVDAO dao;

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

        public CSVTransaction FirstTransaction
        {
            get
            {
                throw new NotImplementedException();
                //var firstTx = Outputs.First().Transaction;
                //foreach (var output in Outputs)
                //{
                //    var tx = output.Transaction;
                //    if (tx.Block.Height < firstTx.Block.Height)
                //    {
                //        firstTx = tx;
                //    }
                //}
                //return firstTx;
            }
        }

        public double Balance
        {
            get
            {
                double balance = 0;
                foreach (var output in Outputs)
                {
                    //if (output.Transaction.Coinbase)
                    //{
                    balance += output.Value;
                    //}
                }
                return balance;
            }
        }
    }
}
