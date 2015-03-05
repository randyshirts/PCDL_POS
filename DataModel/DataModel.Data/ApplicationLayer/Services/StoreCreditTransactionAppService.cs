

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class StoreCreditTransactionAppService : PcdAppServiceBase, IStoreCreditTransactionAppService
    {
        private readonly IStoreCreditTransactionRepository _storeCreditTransRepository;

        public StoreCreditTransactionAppService(IStoreCreditTransactionRepository storeCreditTransRepository)
        {
            _storeCreditTransRepository = storeCreditTransRepository;
        }

        public GetStoreCreditTransactionsByConsignorIdOutput GetStoreCreditTransactionsByConsignorId(
            GetStoreCreditTransactionsByConsignorIdInput input)
        {
            return new GetStoreCreditTransactionsByConsignorIdOutput
            {
                Transactions = _storeCreditTransRepository.GetStoreCreditTransactionsByConsignorId(input.ConsignorId).Select(i => new StoreCreditTransactionDto(i)).ToList()
            };
        }
    }
}
