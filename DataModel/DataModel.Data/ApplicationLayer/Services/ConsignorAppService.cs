using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.DataLayer.Repositories;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class ConsignorAppService : PcdAppServiceBase, IConsignorAppService
    {
        private readonly IConsignorRepository _consignorRepository;

        public ConsignorAppService(IConsignorRepository consignorRepository)
        {
            _consignorRepository = consignorRepository;
        }
        
        public AddNewConsignorOutput AddNewConsignor(AddNewConsignorInput input)
        {
            return new AddNewConsignorOutput
            {
                Id = _consignorRepository.AddNewConsignor(input.Consignor.ConvertToConsignor())
            };
        }

        public void DeleteConsignerById(DeleteConsignerByIdInput input)
        {
            _consignorRepository.DeleteConsignerById(input.Id);
        }

        public void UpdateConsignor(UpdateConsignorInput input)
        {
            _consignorRepository.UpdateConsignor(input.Consignor.ConvertToConsignor());
        }

        public GetConsignorByNameOutput GetConsignorByName(GetConsignorByNameInput input)
        {
            return new GetConsignorByNameOutput
            {
                Consignor = new ConsignorDto(_consignorRepository.GetConsignorByName(input.FirstName, input.LastName))
            };
        }

        public GetConsignorNamesOutput GetConsignorNames()
        {
            return new GetConsignorNamesOutput
            {
                ConsignorNames = _consignorRepository.GetConsignorNames().ToList()
            };
        }

        public GetConsignorNamesLastnameFirstOutput GetConsignorNamesLastnameFirst()
        {
            return new GetConsignorNamesLastnameFirstOutput
            {
                ConsignorNames = _consignorRepository.GetConsignorNamesLastnameFirst().ToList()
            };
        }

        public GetConsignorByFullNameOutput GetConsignorByFullName(GetConsignorByFullNameInput input)
        {
            return new GetConsignorByFullNameOutput
            {
                Consignor = new ConsignorDto(_consignorRepository.GetConsignorByFullName(input.FullName))
            };
        }

        public GetConsignorCreditBalanceOutput GetConsignorCreditBalance(GetConsignorCreditBalanceInput input)
        {
            return new GetConsignorCreditBalanceOutput
            {
                Balance = _consignorRepository.GetConsignorCreditBalance(input.FullName)
            };
        }

        public GetAllConsignorsOutput GetAllConsignors()
        {
            var output = _consignorRepository.GetAllList();
            var list = output.Select(c => (new ConsignorDto(c))).ToList();
            
            return new GetAllConsignorsOutput
            {
                Consignors = list
            };
        }

        public GetConsignorByEmailOutput GetConsignorByEmail(GetConsignorByEmailInput input)
        {
            return new GetConsignorByEmailOutput
            {
                Consignor = new ConsignorDto(_consignorRepository.GetConsignorByEmail(input.Email))
            };
        }
    }
}
