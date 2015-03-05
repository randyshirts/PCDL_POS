using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class StoreCreditPmtDto : EntityDto
    {
        public StoreCreditPmtDto(StoreCreditPmt pmt)
        {
            Id = pmt.Id;
            ConsignorId = pmt.ConsignorId;
            DebitTransaction_StoreCreditPmt = pmt.DebitTransaction_StoreCreditPmt;
            Consignor_StoreCreditPmt = pmt.Consignor_StoreCreditPmt;
            StoreCreditPmtAmount = pmt.StoreCreditPmtAmount;
        }

        public DebitTransaction DebitTransaction_StoreCreditPmt { get; set; }
        public int ConsignorId { get; set; }
        public Consignor Consignor_StoreCreditPmt { get; set; }
        public double StoreCreditPmtAmount { get; set; }

        public StoreCreditPmt ConvertToConsignorPmt()
        {
            return new StoreCreditPmt
            {
                Id = Id,
                ConsignorId = ConsignorId,
                DebitTransaction_StoreCreditPmt = DebitTransaction_StoreCreditPmt,
                Consignor_StoreCreditPmt = Consignor_StoreCreditPmt,
                StoreCreditPmtAmount = StoreCreditPmtAmount
            };
        }

    }

    public class GetStoreCreditPmtsByConsignorIdOutput
    {
        public List<StoreCreditPmtDto> PmtDtos { get; set; }
    }

    public class GetStoreCreditPmtsByConsignorIdInput
    {
        public int ConsignorId { get; set; }
    }
}
