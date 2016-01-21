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

			// Console.WriteLine ("Inizializing payments...");
			initializePayments ();

			//Console.WriteLine ("Initializing first occurrences...");
			//initializeAddressFirstOccurrences2();
		}

		private static void initializePayments()
		{
			for (int i = 50; i <= 181; i++) {
				var lowerBound = i * 1000;
				var upperBound = lowerBound + 1000;

				Console.Out.WriteLine ("Get blocks between " + lowerBound + " and " + upperBound);

				using (var context = dao.NewDataContext) {
					var payments = context.GetTable<Payment> ();

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
					}

					context.SubmitChanges ();
					Console.WriteLine ("changes submitted");
				}

				Console.WriteLine ("finished");
			}
		}

		private static void initializeAddressProfits()
		{
			using (var context = dao.NewDataContext) {
				foreach (var address in context.GetTable<Address>()) {
					double profit = 0.0;

					foreach (var output in address.Outputs) {
						bool splitTX = false;
						foreach (var input in output.Transaction.Inputs) {
							if (input.AddressID == address.ID) {
								splitTX = true;
								break;
							}
						}

						if (!splitTX) {
							profit += output.Value;
						}
					}

					address.Profit = profit;
				}

				context.SubmitChanges ();
				Console.Out.WriteLine ("db updated");
			}
		}

		private static void initializeAddressFirstOccurrences()
		{
			using (var context = dao.NewDataContext) {
				int thousands = 0;

				while (true) {
					var lowerBound = thousands * 1000;
					var upperBound = lowerBound + 1000;

					Console.Out.WriteLine ("Get addresses between " + lowerBound + " and " + upperBound);

					var addresses = context.GetTable<Address> ();
					foreach (var address in addresses.Skip(lowerBound).Take(1000)) {
						var firstOccHeight = address.Outputs.First().Transaction.Block.Height;
						foreach (var output in address.Outputs) {
							var height = output.Transaction.Block.Height;
							if (height < firstOccHeight) {
								firstOccHeight = height;
							}
						}

						address.FirstOccurrenceBlockHeight = firstOccHeight;
					}

					context.SubmitChanges();
					Console.Out.WriteLine (upperBound + " addresses updated");

					thousands++;
				}
			}
		}

		private static void initializeAddressFirstOccurrences2()
		{
			int thousands = 28;

			while (true) {
				var lowerBound = thousands * 1000;
				var upperBound = lowerBound + 1000;

				Console.Out.WriteLine ("Get blocks between " + lowerBound + " and " + upperBound);

				using (var context = dao.NewDataContext) {
					foreach (var block in context.GetTable<Block>().Where(b => (b.Height >= lowerBound && b.Height < upperBound))) {
						Console.Write ("Block " + block.Height);

						foreach (var tx in block.Transactions) {
							foreach (var output in tx.Outputs) {
								var address = output.Address;
								if (address.FirstOccurrenceBlockHeight == 0) {
									address.FirstOccurrenceBlockHeight = block.Height;
								}
							}
						}

						Console.WriteLine (" done");
					}

					context.SubmitChanges ();
					Console.WriteLine ("changes submitted");
				}

				thousands++;
			}
		}
	}
}
