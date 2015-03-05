using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
