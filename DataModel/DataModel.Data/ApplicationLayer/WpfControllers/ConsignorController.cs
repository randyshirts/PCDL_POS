using System;
using System.Collections.Generic;
using System.Linq;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class ConsignorController : IConsignorController
    {
        public int AddNewConsignor(Consignor consignor)
        {
            var input = new AddNewConsignorInput
            {
                Consignor = new ConsignorDto(consignor)
            };

            var emailInput = new GetPersonsByEmailInput
            {
                EmailAddress = consignor.Consignor_Person.EmailAddresses.FirstOrDefault().EmailAddress
            };

            var person = new Person();

            using (var repo = new EmailRepository())
            {
                var app = new EmailAppService(repo);
                person = app.GetPersonsByEmail(emailInput).EmailPersons
                    .Select(p => p.ConvertToPerson()).FirstOrDefault();
            }

            if (person != null)
            {
                //Add member to existing person if name matches
                if (person.FirstName == consignor.Consignor_Person.FirstName &&
                    (person.LastName == consignor.Consignor_Person.LastName))
                {
                    consignor.Consignor_Person = null;

                    var existingInput = new AddConsignorToPersonInput
                    {
                        Consignor = new ConsignorDto(consignor),
                        Person = new PersonDto(person)
                    };

                    using (var repo = new ConsignorRepository())
                    {
                        var app = new ConsignorAppService(repo);
                        if (app.AddConsignorToPerson(existingInput).Result)
                        {
                            return person.Id;
                        }
                    }
                }
                throw new Exception("First and Last names do not match email address on record");
            }

            using (var repo = new ConsignorRepository())
            {
                var app = new ConsignorAppService(repo);
                return app.AddNewConsignor(input).Id;
            }
        }

        public void DeleteConsignerById(int id)
        {
            var input = new DeleteConsignerByIdInput
            {
                Id = id
            };
            using (var repo = new ConsignorRepository())
            {
                var app = new ConsignorAppService(repo);
                app.DeleteConsignerById(input);
            }
        }

        public void UpdateConsignor(Consignor updatedConsignor)
        {
            var input = new UpdateConsignorInput
            {
                Consignor = new ConsignorDto(updatedConsignor)
            };
            using (var repo = new ConsignorRepository())
            {
                var app = new ConsignorAppService(repo);
                app.UpdateConsignor(input);
            }
        }

        public Consignor GetConsignorByName(string firstName, string lastName)
        {
            var input = new GetConsignorByNameInput
            {
                FirstName = firstName,
                LastName = lastName
            };
            using (var repo = new ConsignorRepository())
            {
                var app = new ConsignorAppService(repo);
                var output = app.GetConsignorByName(input);

                return output.Consignor.ConvertToConsignor();
            }
        }

        public List<string> GetConsignorNames()
        {
            using (var repo = new ConsignorRepository())
            {
                var app = new ConsignorAppService(repo);
                var output = app.GetConsignorNames();

                return output.ConsignorNames;
            }
        }

        public List<string> GetConsignorNamesLastnameFirst()
        {
            using (var repo = new ConsignorRepository())
            {
                var app = new ConsignorAppService(repo);
                var output = app.GetConsignorNamesLastnameFirst();

                return output.ConsignorNames;
            }
        }

        public Consignor GetConsignorByFullName(string fullName)
        {
            var input = new GetConsignorByFullNameInput
            {
                FullName = fullName
            };
            using (var repo = new ConsignorRepository())
            {
                var app = new ConsignorAppService(repo);
                var output = app.GetConsignorByFullName(input);

                return output.Consignor.ConvertToConsignor();
            }
        }

        public double GetConsignorCreditBalance(string name)
        {
            var input = new GetConsignorCreditBalanceInput
            {
                FullName = name
            };
            using (var repo = new ConsignorRepository())
            {
                var app = new ConsignorAppService(repo);
                var output = app.GetConsignorCreditBalance(input);

                return output.Balance;
            }
        }

        public IEnumerable<Consignor> GetAllConsignors()
        {
            using (var repo = new ConsignorRepository())
            {
                var app = new ConsignorAppService(repo);
                var output = app.GetAllConsignors();

                return output.Consignors.Select(c => c.ConvertToConsignor()).ToList();
                
            }
        }
    }
}
