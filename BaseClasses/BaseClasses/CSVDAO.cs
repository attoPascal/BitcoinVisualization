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
            setRelations();
        }

        private Dictionary<string, Address> addresses;
        private Dictionary<string, Output> outputs;
        private Dictionary<string, Transaction> transactions;
        private Dictionary<string, Block> blocks;
        private List<Tuple<string, string>> outputAddressRelations;
        private List<Tuple<string, string>> transactionOutputRelations;
        private List<Tuple<string, string>> transactionInputRelations;
        private List<Tuple<string, string>> blockTransactionRelations;

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
            outputAddressRelations = new List<Tuple<string, string>>();

            var fileName = "rel_output_address.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var outputID = fields[0];
                var addressID = fields[1];

                outputAddressRelations.Add(new Tuple<string, string>(outputID, addressID));
            }
        }

        private void initializeTransactionInputRelations()
        {
            transactionInputRelations = new List<Tuple<string, string>>();

            var fileName = "rel_input.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var transactionID = fields[0];
                var inputID = fields[1];

                transactionInputRelations.Add(new Tuple<string, string>(transactionID, inputID));
            }
        }

        private void initializeTransactionOutputRelations()
        {
            transactionOutputRelations = new List<Tuple<string, string>>();

            var fileName = "rel_tx_output.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var transactionID = fields[0];
                var outputID = fields[1];

                transactionOutputRelations.Add(new Tuple<string, string>(transactionID, outputID));
            }
        }

        private void initializeBlockTransactionRelations()
        {
            blockTransactionRelations = new List<Tuple<string, string>>();

            var fileName = "rel_block_tx.csv";
            var lines = File.ReadAllLines(dataPath + fileName);

            foreach (string line in lines)
            {
                var fields = line.Split(',');

                var blockID = fields[0];
                var transactionID = fields[1];

                blockTransactionRelations.Add(new Tuple<string, string>(blockID, transactionID));
            }
        }

        private void setRelations()
        {
            foreach (var relation in outputAddressRelations)
            {
                var output = OutputWithID(relation.Item1);
                var address = AddressWithID(relation.Item2);

                output.Address = address;
                address.Outputs.Add(output);
            }

            foreach (var relation in transactionInputRelations)
            {
                var transaction = TransactionWithID(relation.Item1);
                var input = OutputWithID(relation.Item2); // inputs are modelled as outputs

                transaction.Inputs.Add(input);
                input.Transaction = transaction;
            }

            foreach (var relation in transactionOutputRelations)
            {
                var transaction = TransactionWithID(relation.Item1);
                var output = OutputWithID(relation.Item2);

                transaction.Outputs.Add(output);
                output.Transaction = transaction;
            }

            foreach (var relation in blockTransactionRelations)
            {
                var block = BlockWithID(relation.Item1);
                var transaction = TransactionWithID(relation.Item2);

                block.Transactions.Add(transaction);
                transaction.Block = block;
            }
        }


    }
}
