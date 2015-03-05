using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class StoreCreditTransactionDto : EntityDto
    {
        public StoreCreditTransactionDto(StoreCreditTransaction trans)
        {
            Id = trans.Id;
            ConsignorId = trans.ConsignorId;
            CreditTransaction_StoreCredit = trans.CreditTransaction_StoreCredit;
            StoreCreditTransactionAmount = trans.StoreCreditTransactionAmount;
        }
        
        public CreditTransaction CreditTransaction_StoreCredit { get; set; }
        public int ConsignorId { get; set; }
        public Consignor Consignor_StoreCreditTransaction { get; set; }
        public double StoreCreditTransactionAmount { get; set; }

        public StoreCreditTransaction ConvertToStoreCreditTransaction()
        {
            return new StoreCreditTransaction()
            {
                Id = Id,
                ConsignorId = ConsignorId,
                CreditTransaction_StoreCredit = CreditTransaction_StoreCredit,
                StoreCreditTransactionAmount = StoreCreditTransactionAmount
            };
        }
    }

    public class GetStoreCreditTransactionsByConsignorIdOutput
    {
        public IEnumerable<StoreCreditTransactionDto> Transactions { get; set; }
    }

    public class GetStoreCreditTransactionsByConsignorIdInput
    {
        public int ConsignorId { get; set; }
    }
}
