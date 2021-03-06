﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class ItemSaleTransactionRepository : PcdRepositoryBase<ItemSaleTransaction>, IItemSaleTransactionRepository
    {
        public int AddNewItemSaleTransaction(ItemSaleTransaction transaction)
        {
            if (transaction.Items_ItemSaleTransaction.ElementAt(0).ItemSaleTransaction != null)
            {
                throw new ArgumentNullException("transaction", "ItemSaleTransaction member Transaction must be null when adding item or SQL will throw exception saying violated relationship property");
            }

            //Validation if a record already exists for this Transaction
            var oldTransaction = GetItemSaleTransactionById(transaction.Id);
            if (oldTransaction != null)
            {
                //Update Items property
                if (transaction.Items_ItemSaleTransaction.Count == 1)
                    oldTransaction.Items_ItemSaleTransaction.Add(transaction.Items_ItemSaleTransaction.ElementAt(0));     //Add new item to old transaction record
                else
                    throw new ArgumentException("AddItemSaleTransaction - Can't add more than one item to a Transaction at the same time, or Transaction.Items is null");
            }
            //Transaction doesn't exist so add it
            else
            {
                Validation.PriceRequire(transaction.CreditTransaction_ItemSale.StateSalesTaxTotal);
                Validation.PriceRequire(transaction.CreditTransaction_ItemSale.LocalSalesTaxTotal);
                Validation.PriceRequire(transaction.CreditTransaction_ItemSale.CountySalesTaxTotal);
                Validation.PriceRequire(transaction.CreditTransaction_ItemSale.TransactionTotal);
                Validation.DateRequire(transaction.CreditTransaction_ItemSale.TransactionDate);

                var itemList = new Collection<Item>();
                foreach (var item in transaction.Items_ItemSaleTransaction)
                {
                    var temp = Context.Items.Find(item.Id);
                    
                    itemList.Add(temp);

                }

                transaction.Items_ItemSaleTransaction = null;
                
                Context.ItemSaleTransactions.Add(transaction);

                transaction.Items_ItemSaleTransaction = itemList;
            }

            Context.SaveChanges();

            //Change all items' status to sold
            if (transaction.Items_ItemSaleTransaction != null)
            {
                var items = new ItemRepository();
                foreach (var item in transaction.Items_ItemSaleTransaction)
                {
                    Context.Items.Find(item.Id).Status = "Sold";
                }
               
            }

            //Get TransactionId
            if (transaction.CreditTransaction_ItemSale != null)
            {
                transaction.CreditTransaction_ItemSale.ItemSaleTransactionId = transaction.Id;
            }
            try
            {
                Context.SaveChanges();
                return transaction.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AddNewItemSaleTransaction - Repository");
                return -1;
            }
            
        }

        public void DeleteTransactionById(int id)
        {
            var transaction = Context.ItemSaleTransactions.Find(id);
            if (transaction == null) return;
            Context.Entry(transaction).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void UpdateItemSaleTransaction(ItemSaleTransaction updatedTransaction)
        {

            var original = Context.ItemSaleTransactions.Find(updatedTransaction.Id);

            if (original == null) return;
            Context.Entry(original).CurrentValues.SetValues(updatedTransaction);
            Context.SaveChanges();
        }

        public List<ItemSaleTransaction> GetAllItemSaleTransactions()
        {
            var query = from t in Context.ItemSaleTransactions
                        orderby t.CreditTransaction_ItemSale.TransactionDate  
                        select t;

            return query.ToList();
        }
        
        public ItemSaleTransaction GetItemSaleTransactionById(int id)
        {
            var transaction = (from t in Context.ItemSaleTransactions
                               where t.Id == id
                               select t).FirstOrDefault<ItemSaleTransaction>();

            return transaction;
        }
    }
}
