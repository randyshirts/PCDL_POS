using System;
using System.Collections.Generic;
using System.Linq;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class StoreCreditPmtRepository : PcdRepositoryBase<StoreCreditPmt>, IStoreCreditPmtRepository
    {
        public IEnumerable<StoreCreditPmt> GetStoreCreditPmtsByConsignorId(int id)
        {
            var storeCreditPmts = (from c in Context.StoreCreditPmts
                                    where c.ConsignorId == id
                                    select c);

            return storeCreditPmts;
        }
    }
}
