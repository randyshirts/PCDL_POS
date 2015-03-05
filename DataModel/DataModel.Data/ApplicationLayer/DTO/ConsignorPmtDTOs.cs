using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{

    public class ConsignorPmtDto : EntityDto
    {
        public ConsignorPmtDto(ConsignorPmt pmt)
        {
            Id = pmt.Id;
            ConsignorId = pmt.ConsignorId;
            DebitTransaction_ConsignorPmt = pmt.DebitTransaction_ConsignorPmt;
            Consignor_ConsignorPmt = pmt.Consignor_ConsignorPmt;
            Items_ConsignorPmt = pmt.Items_ConsignorPmt;
        }

        public DebitTransaction DebitTransaction_ConsignorPmt { get; set; }
        public int ConsignorId { get; set; }
        public Consignor Consignor_ConsignorPmt { get; set; }
        public ICollection<Item> Items_ConsignorPmt { get; set; }

        public ConsignorPmt ConvertToConsignorPmt()
        {
            return new ConsignorPmt
            {
                Id = Id,
                ConsignorId = ConsignorId,
                DebitTransaction_ConsignorPmt = DebitTransaction_ConsignorPmt,
                Consignor_ConsignorPmt = Consignor_ConsignorPmt,
                Items_ConsignorPmt = Items_ConsignorPmt
            };
        }

    }

    public class AddNewConsignorPmtInput
    {
        public ConsignorPmtDto PmtDto { get; set; }
    }

    public class AddNewConsignorPmtOutput
    {
        public int Id { get; set; }    
    }

    public class GetConsignorPmtByIdOutput
    {
        public ConsignorPmtDto PmtDto { get; set; }
    }

    public class GetConsignorPmtByIdInput
    {
        public int Id { get; set; }
    }

    public class GetConsignorPmtsByConsignorIdOutput
    {
        public List<ConsignorPmtDto> PmtDtos { get; set; }
    }

    public class GetConsignorPmtsByConsignorIdInput
    {
        public int ConsignorId { get; set; }
    }
}
