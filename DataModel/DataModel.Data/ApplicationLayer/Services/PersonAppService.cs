using System.Collections.Generic;
using System.Linq;
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

        public QueryPersonsThatAreMembersOutput QueryPersonsThatAreMembers()
        {
            var output = _personRepository.QueryPersonsThatAreMembers();
            var list = output.Select(p => (new PersonDto(p))).ToList();

            return new QueryPersonsThatAreMembersOutput
            {
                Persons = list
            };

        }

        public GetPersonOutput GetPerson(GetPersonInput input)
        {
            return new GetPersonOutput
            {
                PersonDto = new PersonDto(_personRepository.Get(input.Id))
            };
        }


        public GetAllPersonsOutput GetAllPersons()
        {
            var output = _personRepository.GetAllPersons();
            var list = output.Select(p => (new PersonDto(p))).ToList();

            return new GetAllPersonsOutput
            {
                Persons = list
            };
        }
        
    }
}
