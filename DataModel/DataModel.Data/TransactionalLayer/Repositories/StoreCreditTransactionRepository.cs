using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class StoreCreditTransactionRepository : PcdRepositoryBase<StoreCreditTransaction>, IStoreCreditTransactionRepository
    {
        public int AddNewStoreCreditTransaction(StoreCreditTransaction transaction)
        {
            //if (transaction.Items_StoreCreditTransaction.ElementAt(0).StoreCreditTransaction != null)
            //{
            //    throw new ArgumentNullException("transaction", "StoreCreditTransaction member Transaction must be null when adding item or SQL will throw exception saying violated relationship property");
            //}

            //Validation if a record already exists for this Transaction
            //var oldTransaction = GetStoreCreditTransactionById(transaction.Id);
            //if (oldTransaction != null)
            //{
            //    //Update Items property
            //    if (transaction.Items_StoreCreditTransaction.Count == 1)
            //        oldTransaction.Items_StoreCreditTransaction.Add(transaction.Items_StoreCreditTransaction.ElementAt(0));     //Add new item to old transaction record
            //    else
            //        throw new ArgumentException("AddStoreCreditTransaction - Can't add more than one item to a Transaction at the same time, or Transaction.Items is null");
            //}
            //Transaction doesn't exist so add it

            //Validation.PriceRequire(transaction.CreditTransaction_StoreCredit.StateSalesTaxTotal);
            //Validation.PriceRequire(transaction.CreditTransaction_StoreCredit.LocalSalesTaxTotal);
            //Validation.PriceRequire(transaction.CreditTransaction_StoreCredit.CountySalesTaxTotal);
            Validation.PriceRequire(transaction.CreditTransaction_StoreCredit.TransactionTotal);
            Validation.DateRequire(transaction.CreditTransaction_StoreCredit.TransactionDate);

            //var itemList = new Collection<Item>();
            //foreach (var item in transaction.Items_StoreCreditTransaction)
            //{
            //    var temp = Context.Items.Find(item.Id);

            //    itemList.Add(temp);

            //}

            //transaction.Items_StoreCreditTransaction = null;

            Context.CreditTransactions.Add(transaction.CreditTransaction_StoreCredit);

            //transaction.Items_StoreCreditTransaction = itemList;


            Context.SaveChanges();

            //Change all items' status to sold
            //if (transaction.Items_StoreCreditTransaction != null)
            //{
            //    var items = new ItemRepository();
            //    foreach (var item in transaction.Items_StoreCreditTransaction)
            //    {
            //        Context.Items.Find(item.Id).Status = "Sold";
            //    }

            //}

            //Get TransactionId
            if (transaction.CreditTransaction_StoreCredit != null)
            {
                transaction.CreditTransaction_StoreCredit.StoreCreditTransactionId = transaction.CreditTransaction_StoreCredit.StoreCreditTransaction.Id;
            }
            try
            {
                Context.SaveChanges();
                return transaction.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AddNewStoreCreditTransaction - Repository");
                return -1;
            }

        }
        
        
        public IEnumerable<StoreCreditTransaction> GetStoreCreditTransactionsByConsignorId(int id)
        {
            var storeCreditTrans = (from c in Context.StoreCreditTransactions
                                 where c.ConsignorId == id
                                 select c);

            return storeCreditTrans;
        }


    }
}
