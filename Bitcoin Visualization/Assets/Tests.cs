using Bitcoin;
using DAO;
using System.IO;

namespace Tests
{
    class DAOTests
    {
        static void NotMain()
        {
            var dataPath = @"..\..\..\..\data\1000blocks\";
            //var dataPath = @"..\..\..\..\..\blocks_1_180000\";
            BitcoinDAO dao = new CSVDAO(dataPath);

            Test1(dao);
            Test2(dao: dao, outputStream: new FileStream(@"..\..\..\..\output\out.txt", FileMode.Create));

        }

        static void Test1(BitcoinDAO dao)
        {
            var addresses = dao.Addresses;
           // System.Diagnostics.Debug.WriteLine("Addresses:");
            for (int i = 0; i < 100; i++)
            {
                Address address = addresses[i];
             //   System.Diagnostics.Debug.WriteLine(address.ID);
            }

            var outputs = dao.Outputs;
            System.Diagnostics.Debug.WriteLine("Outputs:");
            for (int i = 0; i < 100; i++)
            {
                Output output = outputs[i];
              //  System.Diagnostics.Debug.WriteLine($"{output.ID} ({output.Value} -> {output.Address.ID})");
            }

            var transactions = dao.Transactions;
            System.Diagnostics.Debug.WriteLine("Transaction:");
            for (int i = 0; i < 100; i++)
            {
                Transaction tx = transactions[i];
               // System.Diagnostics.Debug.WriteLine($"{tx.ID}: {tx.Inputs.Count} inputs, {tx.Outputs.Count} outputs" + (tx.Coinbase ? " (coinbase)" : ""));
            }

            var blocks = dao.Blocks;
            System.Diagnostics.Debug.WriteLine("Blocks:");
            for (int i = 0; i < 100; i++)
            {
                Block block = blocks[i];
              //  System.Diagnostics.Debug.WriteLine($"Block {block.Height}: {block.ID}, {block.Transactions.Count} txs");
            }
        }

        static void Test2(BitcoinDAO dao, Stream outputStream)
        {
            using (var writer = new StreamWriter(outputStream))
            {
                foreach (var block in dao.Blocks)
                {
                    //writer.WriteLine($"Block {block.Height}: {block.Transactions.Count} txs");
                    foreach (var tx in block.Transactions)
                    {
                       // writer.WriteLine($"\tTX {tx.ID}:");
                        if (tx.Coinbase)
                        {
                          //  writer.WriteLine($"\t\tIN:  {tx.Outputs[0].Value} BTC from Coinbase");
                        }
                        foreach (var input in tx.Inputs)
                        {
                          //  writer.WriteLine($"\t\tIN:  {input.Value} BTC from {input.Address.ID}");
                        }
                        foreach (var output in tx.Outputs)
                        {
                         //   writer.WriteLine($"\t\tOUT: {output.Value} BTC to   {output.Address.ID}");
                        }
                    }
                }
            }
        }
    }

    
}
