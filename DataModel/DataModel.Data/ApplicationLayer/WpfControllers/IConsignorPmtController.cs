using System.Collections.Generic;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public interface IConsignorPmtController
    {
        int AddNewConsignorPmt(ConsignorPmt consignorPmt);
        ConsignorPmt GetConsignorPmtById(int id);
        IEnumerable<ConsignorPmt> GetConsignorPmtsByConsignorId(int id);
    }
}
