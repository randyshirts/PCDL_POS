
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class StoreCreditPmtAppService : PcdAppServiceBase, IStoreCreditPmtAppservice
    {
        private readonly IStoreCreditPmtRepository _storeCreditPmtRepository;

        public StoreCreditPmtAppService(IStoreCreditPmtRepository storeCreditPmtRepository)
        {
            _storeCreditPmtRepository = storeCreditPmtRepository;
        }
        
        public GetStoreCreditPmtsByConsignorIdOutput GetStoreCreditPmtsByConsignorId(GetStoreCreditPmtsByConsignorIdInput input)
        {
            return new GetStoreCreditPmtsByConsignorIdOutput
            {
                PmtDtos = _storeCreditPmtRepository.GetStoreCreditPmtsByConsignorId(input.ConsignorId).Select(i => new StoreCreditPmtDto(i)).ToList()
            };
        }
    }
}
