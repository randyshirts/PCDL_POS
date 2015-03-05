using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public interface IStoreCreditTransactionController
    {
        IEnumerable<StoreCreditTransaction> GetStoreCreditTransactionsByConsignorId(int id);
    }
}
