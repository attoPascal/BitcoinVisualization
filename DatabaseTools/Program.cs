using Bitcoin;
using DAO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Data.Linq;

namespace DatabaseTools
{
	class MainClass
	{
		private static SQLiteDAO dao;

		public static void Main (string[] args)
		{
			dao = new SQLiteDAO ("../../../big-database.sqlite");
			var newDAO = new SQLiteDAO ("../../../pizza-1000.sqlite");

			var minBlock = 56044;
			var maxBlock = 57043;

			insertPayments (minBlock, maxBlock, newDAO);
			Console.Out.WriteLine ("payments inserted");

			var interestingAddresses = new List<Address> ();

			for (int i = minBlock; i <= maxBlock; i++) {
				var newAddresses = addressesInBlock (i);

				foreach (var address in newAddresses) {
					if (!interestingAddresses.Any (a => a.ID == address.ID)) {
						interestingAddresses.Add (address);
					}
				}
			}
			Console.Out.WriteLine ("found " + interestingAddresses.Count + " addresses");

			using (var context = newDAO.NewDataContext) {
				context.GetTable<Address> ().InsertAllOnSubmit (interestingAddresses);
				context.SubmitChanges ();
				Console.Out.WriteLine ("inserted " + interestingAddresses.Count + " addresses");

				updateFirstOcccurrences (interestingAddresses, minBlock, maxBlock);
				context.SubmitChanges ();
				Console.Out.WriteLine ("first occurences updated");

				foreach (var address in interestingAddresses) {
					var profit = address.BalanceAfterBlock (maxBlock);
					if (profit > 0) {
						address.Profit = profit;
					}
				}
				context.SubmitChanges ();
				Console.Out.WriteLine ("balance updated");
			}

			insertOtherTables (minBlock, maxBlock, newDAO);
			Console.Out.WriteLine ("other tables inserted");

		}

		private static List<Address> addressesInBlock(int height) {
			var block = dao.BlockWithHeight (height);
			var addresses = new List<Address> ();

			foreach (var tx in block.Transactions) {
				foreach (var output in tx.Outputs) {
					if (!addresses.Any (a => a.ID == output.AddressID)) {
						addresses.Add (output.Address);
					}
				}
			}

			return addresses;
		}

		private static void insertPayments(int minBlock, int maxBlock, SQLiteDAO newDAO) {
			Console.Out.WriteLine ("blocks between " + minBlock + " and " + maxBlock);

			using (var oldContext = dao.NewDataContext) {
				using (var newContext = newDAO.NewDataContext) {
					var blocks = oldContext.GetTable<Block> ().Where (b => (b.Height >= minBlock && b.Height <= maxBlock));
					var payments = newContext.GetTable<Payment> ();

					foreach (var block in blocks) {
						Console.WriteLine ("Block " + block.Height);

						// insert payments
						foreach (var tx in block.Transactions) {
							// negative values
							foreach (var input in tx.Inputs) {
								Payment payment = new Payment {
									AddressID = input.AddressID,
									TransactionID = input.TransactionID,
									BlockHeight = block.Height,
									Value = -input.Value
								};

								payments.InsertOnSubmit (payment);
							}

							// positive values
							foreach (var output in tx.Outputs) {
								Payment payment = new Payment {
									AddressID = output.AddressID,
									TransactionID = output.TransactionID,
									BlockHeight = block.Height,
									Value = output.Value
								};

								payments.InsertOnSubmit (payment);
							}
						}
					}

					newContext.SubmitChanges ();
					Console.WriteLine ("changes submitted");
				}
			}
		}

		private static void updateFirstOcccurrences(IEnumerable addresses, int minBlock, int maxBlock) {
			var blocks = dao.Blocks.Where (b => (b.Height >= minBlock && b.Height <= maxBlock));

			foreach (var block in blocks) {
				foreach (var tx in block.Transactions) {
					foreach (var output in tx.Outputs) {
						var address = output.Address;
						if (address != null && address.FirstOccurrenceBlockHeight < minBlock) {
							address.FirstOccurrenceBlockHeight = block.Height;
						}
					}
				}
			}
		}

		private static void insertOtherTables(int minBlock, int maxBlock, SQLiteDAO newDAO) {
			var oldBlocks = dao.Blocks.Where (b => (b.Height >= minBlock && b.Height <= maxBlock));

			var blocks = new List<Block> ();
			var transactions = new List<Transaction> ();
			var outputs = new List<Output> ();

			foreach (var block in oldBlocks) {
				if (!blocks.Any (b => b.ID == block.ID)) {
					blocks.Add (block);
				}

				foreach (var tx in block.Transactions) {
					if (transactions.Exists (t => t.ID == tx.ID)) {
						Console.Out.WriteLine ("found double: " + tx.ID);
					} else {
						transactions.Add (tx);
//						Console.Out.WriteLine (tx.ID);
//						context.GetTable<Transaction>().InsertOnSubmit (tx);
//						context.SubmitChanges ();
					}

					foreach (var output in tx.Outputs) {
						if (!outputs.Any (o => o.ID == output.ID)) {
							outputs.Add (output);
						}
					}
				}
			}

			using (var context = newDAO.NewDataContext) {
				context.GetTable<Block> ().InsertAllOnSubmit (blocks);
				Console.Out.WriteLine ("inserting " + blocks.Count + " blocks");
				context.SubmitChanges ();
			}

//				//context.GetTable<Transaction>().InsertAllOnSubmit (transactions);
//				var txTable = context.GetTable<Transaction> ();
//				Console.Out.WriteLine ("inserting " + transactions.Count + " transactions");
//				foreach (var tx in transactions) {
//					txTable.InsertOnSubmit (tx);
//					Console.Out.WriteLine (tx.ID);
//					context.SubmitChanges ();
//				}
//
//				context.SubmitChanges ();
//
//				context.GetTable<Output> ().InsertAllOnSubmit (outputs);
//				Console.Out.WriteLine ("inserting " + outputs.Count + " outputs");
//				context.SubmitChanges ();




			Console.Out.WriteLine ("all changes submitted");
		}
	}
}