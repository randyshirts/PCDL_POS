using System.Collections.Generic;
using System.Linq;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class StoreCreditTransactionRepository : PcdRepositoryBase<StoreCreditTransaction>, IStoreCreditTransactionRepository
    {
        public IEnumerable<StoreCreditTransaction> GetStoreCreditTransactionsByConsignorId(int id)
        {
            var storeCreditTrans = (from c in Context.StoreCreditTransactions
                                 where c.ConsignorId == id
                                 select c);

            return storeCreditTrans;
        }
    }
}
