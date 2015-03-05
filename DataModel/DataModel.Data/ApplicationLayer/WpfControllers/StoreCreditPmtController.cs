using System.Collections.Generic;
using System.Linq;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class StoreCreditPmtController : IStoreCreditController
    {
        public IEnumerable<StoreCreditPmt> GetStoreCreditPmtsByConsignorId(int id)
        {
            var input = new GetStoreCreditPmtsByConsignorIdInput
            {
                ConsignorId = id
            };

            using (var repo = new StoreCreditPmtRepository())
            {
                var app = new StoreCreditPmtAppService(repo);
                return app.GetStoreCreditPmtsByConsignorId(input)
                        .PmtDtos.Select(i => i.ConvertToConsignorPmt())
                        .ToList();
            }
        }
    }
}
