using System;
using DataModel.Data.DataLayer.Entities;
using RocketPos.Common.Foundation;

namespace RocketPos.Common.Helpers
{
    public class BarcodeItem : ObservableObjects
    {
        public BarcodeItem()
        { }
        
        public BarcodeItem(Item item)
        {
            Id = item.Id;
            BarcodeItemBc = item.Barcode;
            ItemType = item.ItemType;
            Subject = item.Subject;
            DateListed = item.ListedDate;
            PriceListed = item.ListedPrice;
            IsPrintBarcode = true;
            IsDiscountable = item.IsDiscountable;

            if (item.BookId != null)
                Title = item.Book.Title;
            else if (item.GameId != null)
                Title = item.Game.Title;
            else if (item.OtherId != null)
                Title = item.Other.Title;
            else if (item.VideoId != null)
                Title = item.Video.Title;
            else if (item.TeachingAideId != null)
                Title = item.TeachingAide.Title;
            else
                Title = null;
        }


        //Define members
        public int Id { get; set; }
        public string Title { get; set; }
        public string BarcodeItemBc { get; set; }
        public string ItemType { get; set; }
        public string Subject { get; set; }
        public bool IsPrintBarcode { get; set; }
        public bool IsDiscountable { get; set; }
        public DateTime DateListed { get; set; }
        public double PriceListed { get; set; }
        
    }


}
