using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class EmailDto
    {
        public EmailDto(Email email)
        {
            Id = email.Id;
            EmailAddress = email.EmailAddress;
            EmailPerson = email.EmailAddress_Person.Select(p => new PersonDto(p)).ToList();
        }
        
        
        [EmailAddress]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public ICollection<PersonDto> EmailPerson { get; set; }

        public Email ConvertToEmail()
        {
            return new Email
            {
                Id = Id,
                EmailAddress = EmailAddress,
                EmailAddress_Person = EmailPerson.Select(p => p.ConvertToPerson()).ToList()
            };
        }
    }

    public class GetPersonsByEmailOutput : IOutputDto
    {
        public List<PersonDto> EmailPersons { get; set; } 
    }

    public class GetPersonsByEmailInput : IInputDto
    {
        public string EmailAddress { get; set; }
    }
}
