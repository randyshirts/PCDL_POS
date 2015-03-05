using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class ItemSaleTransactionController : IItemSaleTransactionController
    {
        public int AddNewItemSaleTransaction(ItemSaleTransaction transaction)
        {
            var input = new AddNewItemSaleTransactionInput
            {
                Sale = new ItemSaleTransactionDto(transaction)
            };


            using (var repo = new ItemSaleTransactionRepository())
            {
                var app = new ItemSaleTransactionAppService(repo);
                return app.AddNewItemSaleTransaction(input).Id;
            }          
        }

        public void DeleteTransactionById(int id)
        {
            var input = new DeleteTransactionByIdInput
            {
                Id = id 
            };

            try
            {
                using (var repo = new ItemSaleTransactionRepository())
                {
                    var app = new ItemSaleTransactionAppService(repo);
                    app.DeleteTransactionById(input);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DeleteItemTransactionById - Controller");
            }
        }

        public void UpdateItemSaleTransaction(ItemSaleTransaction updatedTransaction)
        {
            var input = new UpdateItemSaleTransactionInput
            {
                Sale = new ItemSaleTransactionDto(updatedTransaction)
            };

            try
            {
                using (var repo = new ItemSaleTransactionRepository())
                {
                    var app = new ItemSaleTransactionAppService(repo);
                    app.UpdateItemSaleTransaction(input);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdateItemSaleTransaction - Controller");
            }
        }

        public List<ItemSaleTransaction> GetAllItemSaleTransactions()
        {
            try
            {
                using (var repo = new ItemSaleTransactionRepository())
                {
                    var app = new ItemSaleTransactionAppService(repo);
                    var output = app.GetAllItemSaleTransactions();
                    return output.Sales.Select(i => i.ConvertToItemSaleTransaction()).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GetAllItemSaleTransactions - Controller");
                return null;
            }
        }

        public ItemSaleTransaction GetItemSaleTransactionById(int id)
        {
            var input = new GetItemSaleTransactionByIdInput
            {
                Id = id
            };

            try
            {
                using (var repo = new ItemSaleTransactionRepository())
                {
                    var app = new ItemSaleTransactionAppService(repo);
                    return app.GetItemSaleTransactionById(input).Sale.ConvertToItemSaleTransaction();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdateItemSaleTransaction - Controller");
                return null;
            }
        }
    }
}
