using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class PersonDto : EntityDto
    {
        public PersonDto(Person person)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            Consignor = person.Consignor;
            Member = person.Member;
            Volunteer = person.Volunteer;
            MailingAddresses = person.MailingAddresses;
            EmailAddresses = person.EmailAddresses;
            PhoneNumbers = person.PhoneNumbers;
        }
           
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Consignor Consignor { get; set; }
        public Member Member { get; set; }
        public Volunteer Volunteer { get; set; }
        public ICollection<MailingAddress> MailingAddresses { get; set; }
        public ICollection<Email> EmailAddresses { get; set; }
        public PhoneNumber PhoneNumbers { get; set; }

        public Person ConvertToPerson()
        {
            return new Person
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Consignor = Consignor,
                Member = Member,
                Volunteer = Volunteer,
                MailingAddresses = MailingAddresses,
                EmailAddresses = EmailAddresses,
                PhoneNumbers = PhoneNumbers
            };
        }
    }

    public class UpdatePersonInput : IInputDto
    {
        public PersonDto PersonDto { get; set; } 
    }

    public class QueryPersonsThatAreConsignersOutput : IOutputDto
    {
        public List<PersonDto> Persons { get; set; } 
    }
}
