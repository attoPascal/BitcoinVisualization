using System;
using System.Data;
using System.Linq;
using Bitcoin;
using DAO;

namespace DatabaseTools
{
	class MainClass
	{
		private static SQLiteDAO dao;

		public static void Main (string[] args)
		{
			dao = new SQLiteDAO ("../../../big-database.sqlite");

			Console.WriteLine ("Inizializing payments...");
			initializePayments ();
		}

		private static void initializePayments()
		{
			using (var context = dao.NewDataContext) {
				var payments = context.GetTable<Payment> ();

				for (int i = 0; i <= 181; i++) {
					var lowerBound = i * 1000;
					var upperBound = lowerBound + 1000;

					Console.Out.WriteLine ("Get blocks between " + lowerBound + " and " + upperBound);

					foreach (var block in context.GetTable<Block>().Where(b => (b.Height >= lowerBound && b.Height < upperBound))) {
						Console.Write ("Block " + block.Height);

						foreach (var tx in block.Transactions) {
							// negative values
							foreach (var input in tx.Inputs) {
								Payment payment = new Payment {
									AddressID = input.AddressID,
									TransactionID = input.TransactionID,
									BlockHeight = block.Height,
									Value = -input.Value
								};

								payments.InsertOnSubmit(payment);
							}

							// positive values
							foreach (var output in tx.Outputs) {
								Payment payment = new Payment {
									AddressID = output.AddressID,
									TransactionID = output.TransactionID,
									BlockHeight = block.Height,
									Value = output.Value
								};

								payments.InsertOnSubmit(payment);
							}
						}


						Console.WriteLine (" done");
						if (i % 1000 == 0) {
							
						}
					}

					context.SubmitChanges ();
					Console.WriteLine ("changes submitted");
				}

				context.SubmitChanges ();
				Console.WriteLine ("finished");
			}
		}
	}
}
