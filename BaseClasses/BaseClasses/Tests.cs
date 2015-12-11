using DAO;
using System.IO;

namespace Tests
{
    class DAOTests
    {
        static void Main()
        {
            var dataPath = @"..\..\..\..\data\1000blocks.db";
            BitcoinDAO dao = new SQLiteDAO(dataPath);

            testIteration(dao);
            //testAssociations(dao: dao, outputStream: new FileStream(@"..\..\..\..\output\out.txt", FileMode.Create));

        }

        static void testIteration(BitcoinDAO dao)
        {
            var i = 0;

            System.Diagnostics.Debug.WriteLine("Addresses:");

            foreach (var address in dao.Addresses)
            {
                System.Diagnostics.Debug.WriteLine($"{i++}: {address.ID}. 1st Block: {address.FirstTransaction.Block.Height}");
            }

            //i = 0;
            //System.Diagnostics.Debug.WriteLine("Outputs:");
            //foreach (var output in dao.Outputs)
            //{
            //    System.Diagnostics.Debug.WriteLine($"{i++}: {output.Value} BTC");
            //}

            //System.Diagnostics.Debug.WriteLine("Transactions:");
            //foreach (var tx in dao.Transactions)
            //{
            //    System.Diagnostics.Debug.WriteLine($"{tx.ID}: " + (tx.Coinbase ? " (coinbase)" : ""));
            //}

            //System.Diagnostics.Debug.WriteLine("Blocks:");
            //foreach (var block in dao.Blocks)
            //{
            //    System.Diagnostics.Debug.WriteLine($"Block {block.Height}: {block.ID}, {block.Transactions.Count} txs");
            //}
        }

        static void testAssociations(BitcoinDAO dao, Stream outputStream)
        {
            using (var writer = new StreamWriter(outputStream))
            {
                foreach (var block in dao.Blocks)
                {
                    writer.WriteLine($"Block {block.Height}: {block.Transactions.Count} txs");
                    foreach (var tx in block.Transactions)
                    {
                        writer.WriteLine($"\tTX {tx.ID}:");
                        if (tx.Coinbase)
                        {
                            writer.WriteLine($"\t\tIN:  {tx.Outputs[0].Value} BTC from Coinbase");
                        }
                        foreach (var input in tx.Inputs)
                        {
                            writer.WriteLine($"\t\tIN:  {input.Value} BTC from {input.Address.ID}");
                        }
                        foreach (var output in tx.Outputs)
                        {
                            writer.WriteLine($"\t\tOUT: {output.Value} BTC to   {output.Address.ID}");
                        }
                    }
                }
            }
        }

        //private void testDB1()
        //{
        //    dbConnection.Open();
        //    var command = new SQLiteCommand(dbConnection);
        //    command.CommandText = "SELECT * FROM output";

        //    System.Diagnostics.Debug.WriteLine("yoooo");

        //    var reader = command.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        System.Diagnostics.Debug.WriteLine($"{reader["output_id"]} {reader["value"]}");
        //    }
        //    reader.Close();

        //    dbConnection.Close();
        //    dbConnection.Dispose();

        //}

        //private void testDB2()
        //{
        //    var context = new DataContext(dbConnection);
        //    var outputs = from output in context.GetTable<Output>() select output;

        //    foreach (var o in outputs)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"{o.ID} {o.Value} ");
        //        var tx = o.Transaction?.Coinbase;
        //        //if (tx == null)
        //        //    System.Diagnostics.Debug.WriteLine($" null");
        //        //else
        //        //    System.Diagnostics.Debug.WriteLine($" yui");
        //    }

        //    //var txs = context.GetTable<Transaction>();
        //    //foreach (var tx in txs)
        //    //{
        //    //    System.Diagnostics.Debug.WriteLine($"{tx.TransactionID} {tx.Coinbase}");
        //    //}
        //}
    }


}
