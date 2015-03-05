using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class ConsignorDto : EntityDto
    {
        public ConsignorDto(Consignor cons)
        {
            Id = cons.Id;
            DateAdded = cons.DateAdded;
            Consignor_Person = cons.Consignor_Person;
            ConsignorPmts_Consignor = cons.ConsignorPmts_Consignor;
            StoreCreditPmts_Consignor = cons.StoreCreditPmts_Consignor;
            StoreCreditTransactions_Consignor = cons.StoreCreditTransactions_Consignor;
            Items_Consignor = cons.Items_Consignor;
        }
           
        public DateTime DateAdded { get; set; }
        public Person Consignor_Person { get; set; }
        public ICollection<ConsignorPmt> ConsignorPmts_Consignor { get; set; }
        public ICollection<StoreCreditTransaction> StoreCreditTransactions_Consignor { get; set; }
        public ICollection<StoreCreditPmt> StoreCreditPmts_Consignor { get; set; }
        public ICollection<Item> Items_Consignor { get; set; }

        public Consignor ConvertToConsignor()
        {
            return new Consignor()
            {
                Id = Id,
                DateAdded = DateAdded,
                Consignor_Person = Consignor_Person,
                ConsignorPmts_Consignor = ConsignorPmts_Consignor,
                StoreCreditPmts_Consignor = StoreCreditPmts_Consignor,
                StoreCreditTransactions_Consignor = StoreCreditTransactions_Consignor,
                Items_Consignor = Items_Consignor
            };
        }

    }

    public class AddNewConsignorOutput : IOutputDto
    {
        public int Id { get; set; }
    }

    public class AddNewConsignorInput : IInputDto
    {
        public ConsignorDto Consignor { get; set; }
    }

    public class DeleteConsignerByIdInput : IInputDto
    {
        public int Id { get; set; }
    }

    public class UpdateConsignorInput : IInputDto
    {
        public ConsignorDto Consignor { get; set; }
    }

    public class GetConsignorByNameOutput : IOutputDto
    {
        public ConsignorDto Consignor { get; set; }
    }

    public class GetConsignorByNameInput : IInputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class GetConsignorNamesOutput : IOutputDto
    {
        public List<string> ConsignorNames { get; set; } 
    }

    public class GetConsignorNamesLastnameFirstOutput : IOutputDto
    {
        public List<string> ConsignorNames { get; set; } 
    }

    public class GetConsignorByFullNameOutput : IOutputDto
    {
        public ConsignorDto Consignor { get; set; }
    }

    public class GetConsignorByFullNameInput : IInputDto
    {
        public string FullName { get; set; }
    }

    public class GetConsignorCreditBalanceOutput : IOutputDto
    {
        public double Balance { get; set; }
    }

    public class GetConsignorCreditBalanceInput : IInputDto
    {
        public string FullName { get; set; }
    }

    public class GetAllConsignorsOutput : IOutputDto
    {
        public List<ConsignorDto> Consignors { get; set; } 
    }
}
