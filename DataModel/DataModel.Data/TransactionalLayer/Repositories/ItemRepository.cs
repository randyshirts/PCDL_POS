using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class ItemRepository : PcdRepositoryBase<Item>, IItemRepository
    {
        public string SetItemBarcode(Item item)
        {
            var barcode =
                item.ListedDate.Year.ToString().Substring(Math.Max(0, item.ListedDate.Year.ToString().Length - 2)) +
                item.ListedDate.Month.ToString("D2") +
                item.Consignor.Id.ToString("D4") + item.Id.ToString("D7");

            if (Context.Items.Any(i => i.Barcode == barcode))
                throw new ArgumentException("Barcode already exists in SetItemBarcode in BusinessContext");

            return barcode;
        }



        public int UpdateItem(Item updatedItem)
        {
            //var result = Update(updatedItem); 
            //if(result != null) return result.Id;
            //return -1;
            var original = Context.Items.Find(updatedItem.Id);

            if (original == null) return -1;
            Context.Entry(original).CurrentValues.SetValues(updatedItem);
            Context.SaveChanges();
            return updatedItem.Id;
        }

        public void DeleteItemById(int id)
        {
            var item = Context.Items.Find(id);
            if (item != null)
            {
                Context.Entry(item).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        //Perform initial search of items - All searches are performed one at a time 
        public IEnumerable<Item> SearchAllItems(string barcode, string status, string itemType, string consignorName, DateTime? listedDate, string title)
        {
            var query = GetAll();

            if (barcode == "All") barcode = null;
            if (status == "All") status = null;
            if (itemType == "All") itemType = null;
            if (consignorName == "All") consignorName = null;

            if ((barcode == null) && (status == null) && (itemType == null) && (consignorName == null) && (title == null))
                return null;

            if (!String.IsNullOrEmpty(barcode))
            {
                query = query.Where(item => item.Barcode == barcode);
            }

            if (!String.IsNullOrEmpty(status))
            {
                query = query.Where(item => item.Status == status);
            }

            if (!String.IsNullOrEmpty(itemType))
            {
                query = query.Where(item => item.ItemType == itemType);
            }

            if (!String.IsNullOrEmpty(consignorName))
            {
                var names = consignorName.Split(' ');
                var firstName = names[0];
                var lastName = names[1];
                query = query.Where(item => item.Consignor.Consignor_Person.FirstName == firstName); 
                query = query.Where(item => item.Consignor.Consignor_Person.LastName == lastName);
            }

            if (listedDate.HasValue)
            {
                query = query.Where(item => item.ListedDate.Year == listedDate.Value.Year)
                                .Where(item => item.ListedDate.Month == listedDate.Value.Month)
                                .Where(item => item.ListedDate.Day == listedDate.Value.Day);
            }

            if (!String.IsNullOrEmpty(title))
            {
                if(itemType == "Book")
                    query = query.Where(item => item.Book.Title.Contains(title));

                if (itemType == "Game")
                    query = query.Where(item => item.Game.Title.Contains(title));

                if (itemType == "Other")
                    query = query.Where(item => item.Other.Title.Contains(title));

                if (itemType == "TeachingAide")
                    query = query.Where(item => item.TeachingAide.Title.Contains(title));

                if (itemType == "Video")
                    query = query.Where(item => item.Video.Title.Contains(title));
            }

            return query
                .OrderByDescending(item => item.Id)
                .Include(item => item.Consignor)
                .Include(item => item.ConsignorPmt)
                .Include(item => item.ItemSaleTransaction)
                .Include(item => item.Book)
                .Include(item => item.Game)
                .Include(item => item.Other)
                .Include(item => item.Video)
                .Include(item => item.TeachingAide)
                .ToList();
        }

        //Perform a search of items between a certain date range
        public IEnumerable<Item> SearchItemsDateRange(DateTime? fromDate, DateTime? toDate, string consignorName, string status)
        {
            var query = GetAll();

            if (status == "All") status = null;
            if (consignorName == "All") consignorName = null;

            if (!String.IsNullOrEmpty(status))
            {
                query = query.Where(item => item.Status == status);
            }

            if (!String.IsNullOrEmpty(consignorName))
            {
                var names = consignorName.Split(' ');
                var firstName = names[0];
                var lastName = names[1];
                query = query.Where(item => item.Consignor.Consignor_Person.FirstName == firstName); 
                query = query.Where(item => item.Consignor.Consignor_Person.LastName == lastName);
            }

            //if (fromDate.HasValue)
            //{
            //    query = query.Where(item => item.ListedDate.Year >= fromDate.Value.Year)
            //                    .Where(item => item.ListedDate.Month >= fromDate.Value.Month)
            //                    .Where(item => item.ListedDate.Day >= fromDate.Value.Day);
            //}

            //if (toDate.HasValue)
            //{
            //    query = query.Where(item => item.ListedDate.Year <= toDate.Value.Year)
            //                    .Where(item => item.ListedDate.Month <= toDate.Value.Month)
            //                    .Where(item => item.ListedDate.Day <= toDate.Value.Day);
            //}
            if (fromDate.HasValue)
            {
                query = query.Where(item => item.ListedDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(item => item.ListedDate <= toDate.Value);
            }


            return query
                .OrderByDescending(item => item.Id)
                .Include(item => item.Consignor)
                .Include(item => item.ConsignorPmt)
                .Include(item => item.ItemSaleTransaction)
                .Include(item => item.Book)
                .Include(item => item.Game)
                .Include(item => item.Other)
                .Include(item => item.Video)
                .Include(item => item.TeachingAide)
                .ToList();
        }

        //Perform search of items contained in existing list - All searches may be performed or one 
        public IEnumerable<Item> SearchListItems(IEnumerable<Item> list, string barcode, string status, string itemType, string consignorName, DateTime? listedDate)
        {
            var query = (IQueryable<Item>)(list);

            if (barcode == "All") barcode = null;
            if (status == "All") status = null;
            if (itemType == "All") itemType = null;
            if (consignorName == "All") consignorName = null;

            if (!query.Any())
            {
                query = GetAll();
            }

            if (!String.IsNullOrEmpty(barcode))
            {
                query = query.Where(item => item.Barcode == barcode);
            }

            if (!String.IsNullOrEmpty(status))
            {
                query = query.Where(item => item.Status == status);
            }

            if (!String.IsNullOrEmpty(itemType))
            {
                query = query.Where(item => item.ItemType == itemType);
            }

            if (!String.IsNullOrEmpty(consignorName))
            {
                var names = consignorName.Split(' ');
                var firstName = names[0];
                var lastName = names[1];
                query = query.Where(item => item.Consignor.Consignor_Person.FirstName == firstName); 
                query = query.Where(item => item.Consignor.Consignor_Person.LastName == lastName);
            }

            if (listedDate.HasValue)
            {
                query = query.Where(item => item.ListedDate.Year == listedDate.Value.Year)
                                .Where(item => item.ListedDate.Month == listedDate.Value.Month)
                                .Where(item => item.ListedDate.Day == listedDate.Value.Day);
            }


            return query
                .OrderByDescending(item => item.Id)
                .Include(item => item.Consignor)
                .Include(item => item.ConsignorPmt)
                .Include(item => item.ItemSaleTransaction)
                .Include(item => item.Book)
                .Include(item => item.Game)
                .Include(item => item.Other)
                .Include(item => item.Video)
                .Include(item => item.TeachingAide)
                .ToList();
        }

        public List<Item> GetItemsByConsignorName(string firstName, string lastName)
        {
            var query = (from i in Context.Items
                            where ((i.Consignor.Consignor_Person.FirstName == firstName) &&
                                    (i.Consignor.Consignor_Person.LastName == lastName))
                            select i);
            return query.ToList();
        }

        public List<Item> GetItemsByPartOfBarcode(string barcode)
        {
            var query = (from i in Context.Items
                         where ((i.Barcode.Contains(barcode.Trim())))
                         select i);
            return query.ToList();
        }

        public IEnumerable<Item> QueryItemsThatAreBooks()
        {
            var query = (from i in Context.Items
                         where (i.BookId.HasValue)
                         select i);
            return query.ToList();

        }


        public IEnumerable<Item> QueryItemsThatAreGames()
        {
            var query = (from i in Context.Items
                         where (i.GameId.HasValue)
                         select i);
            return query.ToList();
        }

        public IEnumerable<Item> QueryItemsThatAreOthers()
        {
            var query = (from i in Context.Items
                         where (i.OtherId.HasValue)
                         select i);
            return query.ToList();
        }


        public IEnumerable<Item> QueryItemsThatAreTeachingAides()
        {
            var query = (from i in Context.Items
                         where (i.TeachingAideId.HasValue)
                         select i);
            return query.ToList();
        }


        public IEnumerable<Item> QueryItemsThatAreVideos()
        {
            var query = (from i in Context.Items
                         where (i.VideoId.HasValue)
                         select i);
            return query.ToList();
        }

    }
}
