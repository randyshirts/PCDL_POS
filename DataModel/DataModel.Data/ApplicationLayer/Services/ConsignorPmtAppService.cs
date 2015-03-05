using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class ConsignorPmtAppService : PcdAppServiceBase, IConsignorPmtAppService
    {
        private readonly IConsignorPmtRepository _consignorPmtRepository;

        public ConsignorPmtAppService(IConsignorPmtRepository consignorPmtRepository)
        {
            _consignorPmtRepository = consignorPmtRepository;
        }
        
        
        public AddNewConsignorPmtOutput AddNewConsignorPmt(AddNewConsignorPmtInput input)
        {
            return new AddNewConsignorPmtOutput
            {
                Id = _consignorPmtRepository.AddNewConsignorPmt(input.PmtDto.ConvertToConsignorPmt())
            };
        }

        public GetConsignorPmtByIdOutput GetConsignorPmtById(GetConsignorPmtByIdInput input)
        {
            return new GetConsignorPmtByIdOutput
            {
                PmtDto = new ConsignorPmtDto(_consignorPmtRepository.GetConsignorPmtById(input.Id))
            };
        }

        public GetConsignorPmtsByConsignorIdOutput GetConsignorPmtsByConsignorId(GetConsignorPmtsByConsignorIdInput input)
        {
            return new GetConsignorPmtsByConsignorIdOutput
            {
                PmtDtos = _consignorPmtRepository.GetConsignorPmtsByConsignorId(input.ConsignorId).Select(i => new ConsignorPmtDto(i)).ToList()
            };
        }
    }
}
