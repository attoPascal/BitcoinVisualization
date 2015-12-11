using System.Collections.Generic;
using Bitcoin;
using System.Linq;
using System.IO;
using System.Globalization;

namespace DAO
{
    public class CSVDAO
    {
        private string dataPath;

        public CSVDAO(string dataPath)
        {
            this.dataPath = dataPath;
            initializeAddresses();
            initializeOutputs();
            initializeTransactions();
            initializeBlocks();
            initializeOutputAddressRelations();
            initializeTransactionInputRelations();
            initializeTransactionOutputRelations();
            initializeBlockTransactionRelations();
        }

        private Dictionary<string, CSVAddress> addresses;
        private Dictionary<string, CSVOutput> outputs;
        private Dictionary<string, CSVTransaction> transactions;
        private Dictionary<string, CSVBlock> blocks;

        public List<CSVAddress> Addresses
        {
            get
            {
                return addresses.Values.ToList();
            }
        }


        public List<CSVOutput> Outputs
        {
            get
            {
                return outputs.Values.ToList();
            }
        }

        public List<CSVTransaction> Transactions
        {
            get
            {
                return transactions.Values.ToList();
            }
        }

        public List<CSVBlock> Blocks
        {
            get
            {
                return blocks.Values.ToList();
            }
        }

        public CSVAddress AddressWithID(string id)
        {
            return addresses[id];
        }

        public CSVOutput OutputWithID(string id)
        {
            return outputs[id];
        }

        public CSVTransaction TransactionWithID(string id)
        {
            return transactions[id];
        }

        public CSVBlock BlockWithID(string id)
        {
            return blocks[id];
        }

        public CSVBlock BlockWithHeight(int height)
        {
            return Blocks.ElementAt(height);
        }

        private void initializeOutputs()
        {
            outputs = new Dictionary<string, CSVOutput>();

            var fileName = "outputs.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var id = fields[0];
                var n = int.Parse(fields[1]);
                var value = double.Parse(fields[2], CultureInfo.InvariantCulture);
                var type = fields[3];

                outputs.Add(id, new CSVOutput(id, n, value, type, dao: this));
            }
        }

        private void initializeAddresses()
        {
            addresses = new Dictionary<string, CSVAddress>();

            var fileName = "addresses.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var outputID = line;
                addresses.Add(outputID, new CSVAddress(outputID, dao: this));
            }
        }

        private void initializeTransactions()
        {
            transactions = new Dictionary<string, CSVTransaction>();

            var fileName = "transactions.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var id = fields[0];
                var coinbase = fields[1] == "True";

                transactions.Add(id, new CSVTransaction(id, coinbase, dao: this));
            }
        }

        private void initializeBlocks()
        {
            blocks = new Dictionary<string, CSVBlock>();

            var fileName = "blocks.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var id = fields[0];
                var height = int.Parse(fields[1]);
                var timestamp = int.Parse(fields[2]);

                blocks.Add(id, new CSVBlock(id, height, timestamp, dao: this));
            }
        }

        private void initializeOutputAddressRelations()
        {
            var fileName = "rel_output_address.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var outputID = fields[0];
                var addressID = fields[1];

                OutputWithID(outputID).AddressID = addressID;
                AddressWithID(addressID).OutputIDs.Add(outputID);
            }
        }

        private void initializeTransactionInputRelations()
        {
            var fileName = "rel_input.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var transactionID = fields[0];
                var inputID = fields[1];

                TransactionWithID(transactionID).InputIDs.Add(inputID);
                OutputWithID(inputID).TransactionID = transactionID;
            }
        }

        private void initializeTransactionOutputRelations()
        {
            var fileName = "rel_tx_output.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var transactionID = fields[0];
                var outputID = fields[1];

                TransactionWithID(transactionID).OutputIDs.Add(outputID);
                OutputWithID(outputID).TransactionID = transactionID;
            }
        }

        private void initializeBlockTransactionRelations()
        {
            var fileName = "rel_block_tx.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var blockID = fields[0];
                var transactionID = fields[1];

                BlockWithID(blockID).TransactionIDs.Add(transactionID);
                TransactionWithID(transactionID).BlockID = blockID;
            }
        }
    }
}
