using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class EmailAppService
    {
        
        private readonly IEmailRepository _emailRepository;

        public EmailAppService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public GetPersonsByEmailOutput GetPersonsByEmail(GetPersonsByEmailInput input)
        {
            return new GetPersonsByEmailOutput
            {
                EmailPersons = _emailRepository.GetPersonsByEmail(input.EmailAddress).Select(p => new PersonDto(p)).ToList()
            };
        }
    }
}
