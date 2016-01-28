using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Bitcoin
{
	[Table]
	public class Payment
	{
		private int id;
		private string addressID;
		private string transactionID;
		private int blockHeight;
		private double value;

		private EntityRef<Address> address = new EntityRef<Address>();

		[Column(IsPrimaryKey = true, Storage = "id", IsDbGenerated = true, AutoSync = AutoSync.Never)]
		public int ID
		{
			get { return id; }
			set { id = value; }
		}

		[Column(Storage = "addressID", UpdateCheck = UpdateCheck.Never)]
		public string AddressID
		{
			get { return addressID; }
			set { addressID = value; }
		}

		[Column(Storage = "transactionID", UpdateCheck = UpdateCheck.Never)]
		public string TransactionID
		{
			get { return transactionID; }
			set { transactionID = value; }
		}

		[Column(Storage = "blockHeight", UpdateCheck = UpdateCheck.Never)]
		public int BlockHeight
		{
			get { return blockHeight; }
			set { blockHeight = value; }
		}

		[Column(Storage = "value", UpdateCheck = UpdateCheck.Never)]
		public double Value
		{
			get { return value; }
			set { this.value = value; }
		}

		[Association(Name = "AddressPayments", Storage = "address", ThisKey = "AddressID", IsForeignKey = true)]
		public Address Address
		{
			get { return address.Entity; }
			set { address.Entity = value; }
		}
	}
}
