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
            
            Context.CreditTransactions.Add(transaction.CreditTransaction_StoreCredit);

            Context.SaveChanges();

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
