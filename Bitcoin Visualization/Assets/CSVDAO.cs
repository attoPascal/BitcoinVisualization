using System;
using System.Collections.Generic;
using Bitcoin;
using System.Linq;
using System.IO;
using System.Globalization;

namespace DAO
{
    public class CSVDAO : BitcoinDAO
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

        private Dictionary<string, Address> addresses;
        private Dictionary<string, Output> outputs;
        private Dictionary<string, Transaction> transactions;
        private Dictionary<string, Block> blocks;

        public List<Address> Addresses
        {
            get
            {
                return addresses.Values.ToList();
            }
        }

        
        public List<Output> Outputs
        {
            get
            {
                return outputs.Values.ToList();
            }
        }

        public List<Transaction> Transactions
        {
            get
            {
                return transactions.Values.ToList();
            }
        }

        public List<Block> Blocks
        {
            get
            {
                return blocks.Values.ToList();
            }
        }

        public Address AddressWithID(string id)
        {
            return addresses[id];
        }

        public Output OutputWithID(string id)
        {
            return outputs[id];
        }

        public Transaction TransactionWithID(string id)
        {
            return transactions[id];
        }

        public Block BlockWithID(string id)
        {
            return blocks[id];
        }

        public Block BlockWithHeight(int height)
        {
            return Blocks.ElementAt(height);
        }

        private void initializeOutputs()
        {
            outputs = new Dictionary<string, Output>();

            var fileName = "outputs.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var id = fields[0];
                var n = int.Parse(fields[1]);
                var value = double.Parse(fields[2], CultureInfo.InvariantCulture);
                var type = fields[3];

                outputs.Add(id, new Output(id, n, value, type, dao: this));
            }
        }

        private void initializeAddresses()
        {
            addresses = new Dictionary<string, Address>();

            var fileName = "addresses.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var outputID = line;
                addresses.Add(outputID, new Address(outputID, dao: this));
            }
        }

        private void initializeTransactions()
        {
            transactions = new Dictionary<string, Transaction>();

            var fileName = "transactions.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var id = fields[0];
                var coinbase = fields[1] == "True";

                transactions.Add(id, new Transaction(id, coinbase, dao: this));
            }
        }

        private void initializeBlocks()
        {
            blocks = new Dictionary<string, Block>();

            var fileName = "blocks.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var id = fields[0];
                var height = int.Parse(fields[1]);
                var timestamp = int.Parse(fields[2]);

                blocks.Add(id, new Block(id, height, timestamp, dao: this));
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
