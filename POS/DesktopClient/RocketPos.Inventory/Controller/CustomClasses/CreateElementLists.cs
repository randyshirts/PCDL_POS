using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DataModel.Data.DataLayer.Entities;
using Inventory.Controller.Elements;
using Inventory.Controller.Elements.ItemElements;
using RocketPos.Common.Foundation;

namespace Inventory.Controller.CustomClasses
{
    public static class CreateElementLists
    {

        //Make a collection of BarcodeItems with info from a collection of Items 
        public static TrulyObservableCollection<ConsignorItem> CreateConsignorItemsList(IEnumerable<Item> items)
        {
            //Temp Collection Definition
            var tempConsignorItems = new TrulyObservableCollection<ConsignorItem>();

            if (items == null) return tempConsignorItems;

            //Make the collection
            var i = items.GetEnumerator();
            while (i.MoveNext())
            {
                //Create an instance of a BarcodeItem with the current item
                var currentConsignorItem = new ConsignorItem(i.Current);
                //Add the new instance to the collection
                tempConsignorItems.Add(currentConsignorItem);
            }

            return tempConsignorItems;
        }

        
        //Make a collection of BarcodeItems with info from a collection of Items 
        public static TrulyObservableCollection<ConsignorPayment> CreateConsignorPaymentsList(IEnumerable<StoreCreditTransaction> transactions)
        {
            //Temp Collection Definition
            var tempConsignorPayments = new TrulyObservableCollection<ConsignorPayment>();

            if (transactions == null) return tempConsignorPayments;

            //Make the collection
            var i = transactions.GetEnumerator();
            while (i.MoveNext())
            {
                //Create an instance of a ConsignorPayment with the current transaction
                var currentConsignorPayment = new ConsignorPayment(i.Current);
                //Add the new instance to the collection
                tempConsignorPayments.Add(currentConsignorPayment);
            }

            return tempConsignorPayments;
        }

        //Make a collection of BarcodeItems with info from a collection of Items 
        public static TrulyObservableCollection<BookItem> CreateBookItemsList(IEnumerable<Item> items)
        {
            var dataGridBooks = new TrulyObservableCollection<BookItem>();

            foreach (var currentBookItem in items.Select(item => new BookItem(item.Book, item)))
            {
                dataGridBooks.Add(currentBookItem);
            }

            return dataGridBooks;

        }

        //Make a collection of BarcodeItems with info from a collection of Items 
        public static TrulyObservableCollection<GameItem> CreateGameItemsList(IEnumerable<Item> items)
        {
            var dataGridGames = new TrulyObservableCollection<GameItem>();

            foreach (var currentGameItem in items.Select(item => new GameItem(item.Game, item)))
            {
                dataGridGames.Add(currentGameItem);
            }

            return dataGridGames;

        }

        //Make a collection of BarcodeItems with info from a collection of Items 
        public static TrulyObservableCollection<OtherItem> CreateOtherItemsList(IEnumerable<Item> items)
        {
            var dataGridOthers = new TrulyObservableCollection<OtherItem>();

            foreach (var currentOtherItem in items.Select(item => new OtherItem(item.Other, item)))
            {
                dataGridOthers.Add(currentOtherItem);
            }

            return dataGridOthers;

        }

        //Make a collection of BarcodeItems with info from a collection of Items 
        public static TrulyObservableCollection<VideoItem> CreateVideoItemsList(IEnumerable<Item> items)
        {
            var dataGridVideos = new TrulyObservableCollection<VideoItem>();

            foreach (var currentVideoItem in items.Select(item => new VideoItem(item.Video, item)))
            {
                dataGridVideos.Add(currentVideoItem);
            }

            return dataGridVideos;
        }

        //Make a collection of BarcodeItems with info from a collection of Items 
        public static TrulyObservableCollection<TeachingAideItem> CreateTeachingAideItemsList(IEnumerable<Item> items)
        {
            var dataGridTeachingAides = new TrulyObservableCollection<TeachingAideItem>();

            foreach (var currentTeachingAideItem in items.Select(item => new TeachingAideItem(item.TeachingAide, item)))
            {
                dataGridTeachingAides.Add(currentTeachingAideItem);
            }

            return dataGridTeachingAides;
        }

    }
}
