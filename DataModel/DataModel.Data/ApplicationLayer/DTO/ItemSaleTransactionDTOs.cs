using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class ItemSaleTransactionDto : EntityDto
    {
        public ItemSaleTransactionDto(ItemSaleTransaction sale)
        {
            CreditTransaction_ItemSale = sale.CreditTransaction_ItemSale;
            Items_ItemSaleTransaction = sale.Items_ItemSaleTransaction;
        }
        
        
        public CreditTransaction CreditTransaction_ItemSale { get; set; }
        public ICollection<Item> Items_ItemSaleTransaction { get; set; }


        public ItemSaleTransaction ConvertToItemSaleTransaction()
        {
            return new ItemSaleTransaction
            {
                CreditTransaction_ItemSale = CreditTransaction_ItemSale,
                Items_ItemSaleTransaction = Items_ItemSaleTransaction
            };
        }
    }

    public class AddNewItemSaleTransactionInput : IInputDto
    {
        public ItemSaleTransactionDto Sale { get; set; }
    }

    public class DeleteTransactionByIdInput : IInputDto
    {
        public int Id { get; set; }
    }

    public class UpdateItemSaleTransactionInput : IInputDto
    {
        public ItemSaleTransactionDto Sale { get; set; }
    }

    public class GetAllItemSaleTransactionsOutput : IOutputDto
    {
        public IEnumerable<ItemSaleTransactionDto> Sales { get; set; } 
    }

    public class GetItemSaleTransactionByIdOutput : IOutputDto
    {
        public ItemSaleTransactionDto Sale { get; set; }
    }

    public class GetItemSaleTransactionByIdInput : IInputDto
    {
        public int Id { get; set; }
    }

    public class AddNewItemSaleTransactionOutput : IOutputDto
    {
        public int Id { get; set; }
    }
}
