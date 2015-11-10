using System.Collections.Generic;

namespace Bitcoin
{
    public class Transaction
    {
        public string ID;
        public bool Coinbase;

        public List<Output> Inputs;
        public List<Output> Outputs;
        public Block Block;
    }
}
