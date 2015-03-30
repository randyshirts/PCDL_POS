using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Abp.Domain.Entities;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;
using MySql.Data.MySqlClient;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class GenericItemRepositoryBase<T> : PcdRepositoryBase<T>, IGenericItemRepository<T>
        where T : class, IEntity, IGenericItem
    {
        
        public virtual int UpdateItem(T updatedTItem)
        {
            var original = Context.Set(typeof(T)).Find(updatedTItem.Id);
  
            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(updatedTItem);
                Context.SaveChanges();
                return updatedTItem.Id;
            }

            return -1;
        }

        public virtual int AddNewItem(T item)
        {

            //Give dummy value for barcode
            if (item.Items_TItems != null)
            {
                item.Items_TItems.ElementAt(0).Barcode = "9999999999999";
            }

            //Only update items table if a record already exists for this item
            var oldItem = GetItemByTitle(item.Title);
            if (oldItem != null)
            {
                
                //Update Items property
                T updatedItem;
                if (item.Items_TItems != null && item.Items_TItems.Count == 1)
                {
                    updatedItem = Get(oldItem.Id);
                    updatedItem.Items_TItems.Add(item.Items_TItems.ElementAt(0));
                    Context.SaveChanges();
                }
                //oldItem.Items_TItems.Add();     //Add new item to old item record
                else
                    throw new ArgumentException(
                        "Can't add more than one item to a item at the same time, or Item.Items is null");
            }
            //Item doesn't exist so add it
            else
            {
                Validation.StringRequire(item.Title);
                Validation.TitleLength(item.Title);
                Validation.EanLength(item.EAN);
                Validation.ImageLength(item.ItemImage);
                Context.Set(typeof(T)).Add(item);
                Context.SaveChanges();
            }

            //Set Barcode here because we need Id

            if (item.Items_TItems != null)
            {
                var itemId = item.Items_TItems.ElementAt(0).Id;
                return itemId;
            }
            return -1;
        }

        public virtual T GetItemByTitle(string title)
        {
            var sqlCommand = "SELECT * FROM " + typeof (T).Name + "s WHERE Title = @p0";
            var item = Context.Database.SqlQuery<T>(sqlCommand, new MySqlParameter("p0", title)).FirstOrDefault();

            return item;
        }
    }
}
