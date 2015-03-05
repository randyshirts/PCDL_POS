using Abp.Application.Services;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IItemSaleTransactionAppService : IApplicationService
    {
        AddNewItemSaleTransactionOutput AddNewItemSaleTransaction(AddNewItemSaleTransactionInput input);
        void DeleteTransactionById(DeleteTransactionByIdInput input);
        void UpdateItemSaleTransaction(UpdateItemSaleTransactionInput input);
        GetAllItemSaleTransactionsOutput GetAllItemSaleTransactions();
        GetItemSaleTransactionByIdOutput GetItemSaleTransactionById(GetItemSaleTransactionByIdInput input);
    }
}
