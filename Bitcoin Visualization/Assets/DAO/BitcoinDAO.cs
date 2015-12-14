using Bitcoin;
using System.Collections.Generic;
using System.Linq;

namespace DAO
{
    public interface BitcoinDAO
    {
        IQueryable<Address> Addresses { get; }
        IQueryable<Output> Outputs { get; }
        IQueryable<Transaction> Transactions { get; }
        IQueryable<Block> Blocks { get; }

        Address AddressWithID(string id);
        Output OutputWithID(string id);
        Transaction TransactionWithID(string id);
        Block BlockWithID(string id);
        Block BlockWithHeight(int height);
    }
}
