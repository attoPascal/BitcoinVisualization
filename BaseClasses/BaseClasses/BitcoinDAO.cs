using Bitcoin;
using System.Collections.Generic;

namespace DAO
{
    public interface BitcoinDAO
    {
        List<Address> Addresses { get; }
        List<Output> Outputs { get; }
        List<Transaction> Transactions { get; }
        List<Block> Blocks { get; }

        Address AddressWithID(string id);
        Output OutputWithID(string id);
        Transaction TransactionWithID(string id);
        Block BlockWithID(string id);
        Block BlockWithHeight(int height);
    }
}
