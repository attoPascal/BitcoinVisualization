using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Bitcoin
{
    [Table]
    public class Address
    {
        private string id;
		private double profit;
		private int firstOccurrenceBlockHeight;
        private EntitySet<Output> outputs = new EntitySet<Output>();

        [Column(IsPrimaryKey = true, Storage = "id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

		[Column(Storage = "profit")]
		public double Profit
		{
			get { return profit; }
			set { profit = value; }
		}

		[Column(Storage = "firstOccurrenceBlockHeight")]
		public int FirstOccurrenceBlockHeight
		{
			get { return firstOccurrenceBlockHeight; }
			set { firstOccurrenceBlockHeight = value; }
		}

        [Association(Name = "AddressOutputs", Storage = "outputs", OtherKey = "AddressID")]
        public EntitySet<Output> Outputs
        {
            get { return outputs; }
            set { outputs.Assign(value); }
        }

		public double BalanceAfterBlock(int height) {
			// TODO: calculations
			return 0;
		}
    }
}
