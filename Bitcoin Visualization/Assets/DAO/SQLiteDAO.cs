﻿using System.Linq;
using Bitcoin;
using System.Data.Linq;
using System.Data.Common;
using Mono.Data.Sqlite;

namespace DAO
{
    class SQLiteDAO : BitcoinDAO
    {
        private DbConnection dbConnection;
        private DataContext context;

        public SQLiteDAO(string filePath)
        {
            var connectionString = "DbLinqProvider=SQLite;Data Source=" + filePath + ";Version=3;";
            dbConnection = new SqliteConnection(connectionString);
            context = new DataContext(dbConnection);
        }

        public IQueryable<Address> Addresses
        {
            get
            {
                return context.GetTable<Address>();
            }
        }

        public IQueryable<Block> Blocks
        {
            get
            {
                return context.GetTable<Block>();
            }
        }

        public IQueryable<Output> Outputs
        {
            get
            {
                return context.GetTable<Output>();
            }
        }

        public IQueryable<Transaction> Transactions
        {
            get
            {
                return context.GetTable<Transaction>();
            }
        }

        public Address AddressWithID(string id)
        {
            return Addresses.Where(a => a.ID == id).Single();
        }

        public Block BlockWithHeight(int height)
        {
            return Blocks.Where(b => b.Height == height).Single();
        }

        public Block BlockWithID(string id)
        {
            return Blocks.Where(b => b.ID == id).Single();
        }

        public Output OutputWithID(string id)
        {
            return Outputs.Where(o => o.ID == id).Single();
        }

        public Transaction TransactionWithID(string id)
        {
            return Transactions.Where(t => t.ID == id).Single();
        }

		public void InitializeAddressProfits()
		{
			using (var context = new DataContext(dbConnection)) {
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
				UnityEngine.Debug.Log ("db updated");
			}
		}

		public void InitializeAddressFirstOccurrences()
		{
			using (var context = new DataContext(dbConnection)) {
				foreach (var address in context.GetTable<Address>()) {
					UnityEngine.Debug.Log ("updating address " + address.ID);
					
					var firstOccHeight = address.Outputs.First ().Transaction.Block.Height;
					foreach (var output in address.Outputs) {
						var height = output.Transaction.Block.Height;
						if (height < firstOccHeight) {
							firstOccHeight = height;
						}
					}

					UnityEngine.Debug.Log ("previous height: " + address.FirstOccurrenceBlockHeight);

					address.FirstOccurrenceBlockHeight = firstOccHeight;
					UnityEngine.Debug.Log ("new height: " + address.FirstOccurrenceBlockHeight);
				}

				context.SubmitChanges ();
				UnityEngine.Debug.Log ("db updated");
			}
		}

		public void InitializePayments()
		{
			using (var context = new DataContext (dbConnection)) {
				foreach (var block in context.GetTable<Block>()) {
					foreach (var tx in block.Transactions) {
						// negative values
						foreach (var input in tx.Inputs) {
							Payment payment = new Payment {
								AddressID = input.AddressID,
								TransactionID = input.TransactionID,
								BlockHeight = block.Height,
								Value = -input.Value
							};

							context.GetTable<Payment> ().InsertOnSubmit (payment);
						}

						// positive values
						foreach (var output in tx.Outputs) {
							Payment payment = new Payment {
								AddressID = output.AddressID,
								TransactionID = output.TransactionID,
								BlockHeight = block.Height,
								Value = output.Value
							};

							context.GetTable<Payment> ().InsertOnSubmit (payment);
						}
					}
				}

				context.SubmitChanges ();
			}
		}
    }
}
