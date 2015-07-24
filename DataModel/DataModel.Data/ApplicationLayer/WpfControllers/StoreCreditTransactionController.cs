using System;
using System.Collections.Generic;
using System.Linq;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class StoreCreditTransactionController : IStoreCreditTransactionController
    {
        public IEnumerable<StoreCreditTransaction> GetStoreCreditTransactionsByConsignorId(int id)
        {
            var input = new GetStoreCreditTransactionsByConsignorIdInput
            {
                ConsignorId = id
            };

            using (var repo = new StoreCreditTransactionRepository())
            {
                var app = new StoreCreditTransactionAppService(repo);

                return
                    app.GetStoreCreditTransactionsByConsignorId(input)
                        .Transactions.Select(i => i.ConvertToStoreCreditTransaction())
                        .ToList();
            }
        }

        public int AddNewStoreCreditTransaction(StoreCreditTransaction transaction)
        {
            var input = new AddNewStoreCreditTransactionInput
            {
                Payment = new StoreCreditTransactionDto(transaction)
            };


            using (var repo = new StoreCreditTransactionRepository())
            {
                var app = new StoreCreditTransactionAppService(repo);
                return app.AddNewStoreCreditTransaction(input).Id;
            }
        }
    }
}
