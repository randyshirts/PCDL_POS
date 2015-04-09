using System;
using System.Collections.Generic;
using System.Linq;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class ItemSaleTransactionAppService : PcdAppServiceBase, IItemSaleTransactionAppService
    {
        private readonly IItemSaleTransactionRepository _itemSaleRepository;

        public ItemSaleTransactionAppService(IItemSaleTransactionRepository itemSaleRepository)
        {
            _itemSaleRepository = itemSaleRepository;
        }


        public AddNewItemSaleTransactionOutput AddNewItemSaleTransaction(AddNewItemSaleTransactionInput input)
        {
            return new AddNewItemSaleTransactionOutput
            {
                Id = _itemSaleRepository.AddNewItemSaleTransaction(input.Sale.ConvertToItemSaleTransaction())
            };
            
        }

        public void DeleteTransactionById(DeleteTransactionByIdInput input)
        {
            _itemSaleRepository.DeleteTransactionById(input.Id);
        }

        public void UpdateItemSaleTransaction(UpdateItemSaleTransactionInput input)
        {
            _itemSaleRepository.UpdateItemSaleTransaction(input.Sale.ConvertToItemSaleTransaction());
        }

        public GetAllItemSaleTransactionsOutput GetAllItemSaleTransactions()
        {
            return new GetAllItemSaleTransactionsOutput
            {
                Sales = _itemSaleRepository.GetAllItemSaleTransactions().Select(i => new ItemSaleTransactionDto(i)).ToList()
            };
        }

        public GetItemSaleTransactionByIdOutput GetItemSaleTransactionById(GetItemSaleTransactionByIdInput input)
        {
            return new GetItemSaleTransactionByIdOutput
            {
                Sale = new ItemSaleTransactionDto(_itemSaleRepository.GetItemSaleTransactionById(input.Id))
            };
        }
    }
}
