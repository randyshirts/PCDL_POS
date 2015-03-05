
using System.Collections.Generic;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public interface IItemSaleTransactionController
    {
        int AddNewItemSaleTransaction(ItemSaleTransaction transaction);
        void DeleteTransactionById(int id);
        void UpdateItemSaleTransaction(ItemSaleTransaction updatedTransaction);
        List<ItemSaleTransaction> GetAllItemSaleTransactions();
        ItemSaleTransaction GetItemSaleTransactionById(int id);
    }
}
