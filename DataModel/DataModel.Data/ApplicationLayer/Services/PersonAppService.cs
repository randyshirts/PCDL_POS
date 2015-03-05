using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class PersonAppService : PcdAppServiceBase, IPersonAppService
    {
        private readonly IPersonRepository _personRepository;

        public PersonAppService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public void UpdatePerson(UpdatePersonInput input)
        {
            _personRepository.UpdatePerson(input.PersonDto.ConvertToPerson());
        }

        public QueryPersonsThatAreConsignersOutput QueryPersonsThatAreConsigners()
        {
            var output = _personRepository.QueryPersonsThatAreConsigners();
            var list = output.Select(p => (new PersonDto(p))).ToList();
            
            return new QueryPersonsThatAreConsignersOutput
            {              
                Persons = list
            };

        }
    }
}
