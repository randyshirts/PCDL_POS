using System;
using System.Globalization;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using Inventory.Controller.Interfaces;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;
using RocketPos.Data.TransactionalLayer;

namespace Inventory.Controller.Elements.ItemElements
{
    public class VideoItem : ObservableObjects, IItemElement 
    {
        public VideoItem()
        { }

        public VideoItem(Video video, Item item)
        {
            VideoId = video.Id;
            ItemId = item.Id;
            Title = video.Title;
            Ean = video.EAN;
            Publisher = video.Publisher;
            VideoFormat = video.VideoFormat;
            AudienceRating = video.AudienceRating;
            VideoImage = video.ItemImage;
            ListedDate = item.ListedDate;
            ListedPrice = item.ListedPrice;
            Subject = item.Subject;
            IsDiscountable = item.IsDiscountable;
            ItemStatus = item.Status;
            Condition = item.Condition;
            Description = item.Description;
            Barcode = item.Barcode;
            ConsignorName = item.Consignor.Consignor_Person.FirstName + " " + item.Consignor.Consignor_Person.LastName;
        }


        //Define members
        public int VideoId { get; set; }
        public int ItemId { get; set; }
        public string Title { get; set; }
        public string Ean { get; set; }
        public string Publisher { get; set; }
        public string VideoFormat { get; set; }
        public string AudienceRating { get; set; }
        public string VideoImage { get; set; }
        public double ListedPrice { get; set; }
        public DateTime ListedDate { get; set; }
        public double LowestNewPrice { get; set; }
        public double LowestUsedPrice { get; set; }
        public string Subject { get; set; }
        public bool IsDiscountable { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public string ConsignorName { get; set; }
        public string Barcode { get; set; }
        private string _itemStatus;
        public string ItemStatus
        {
            get { return _itemStatus; }
            set
            {
                if (_itemStatus == null)
                {
                    _itemStatus = value;
                }
                else
                {
                    if (WpfHelpers.UpdateStatus(value, _itemStatus))
                    {
                        _itemStatus = value;
                        var controller = new ItemController();
                        controller.UpdateItem(ConvertVideoItemToItem());
                    }

                }
                OnPropertyChanged();
            }
        }

        public bool SetProperty(string column, string text)
        {
            switch (column)
            {
                case "Title":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return false; //Non-nullable
                        Title = text;
                        return true;
                    }
                case "Consignor Name":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return false; //Non-nullable
                        ConsignorName = text;
                        return true;
                    }
                case "EAN":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return text == null; //Nullable
                        Ean = text;
                        return true;
                    }
                case "Publisher":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return text == null; //Nullable
                        Publisher = text;
                        return true;
                    }
                case "Format":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return text == null; //Nullable
                        VideoFormat = text;
                        return true;
                    }
                case "Rating":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return text == null; //Nullable
                        AudienceRating = text;
                        return true;
                    }
                case "VideoImage":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return text == null; //Nullable
                        VideoImage = text;
                        return true;
                    }
                case "ListedPrice":
                    {
                        if (String.IsNullOrEmpty(text)) return false; //Non nullable

                        //Save original and attempt to parse text
                        var orig = ListedPrice;
                        ListedPrice = double.Parse(text, NumberStyles.Currency);

                        //Cancel edit if negative, return true if not
                        if (!(ListedPrice < 0)) return true;
                        ListedPrice = orig;
                        return false;
                    }
                case "ListedDate":
                    {
                        if (String.IsNullOrEmpty(text)) return false; //Non nullable

                        //Save original and attempt to parse text
                        var orig = ListedDate;
                        DateTime d;
                        ListedDate = DateTime.TryParse(text, out d) ? d : DateTime.MinValue;

                        //Cancel edit if negative, return true if not
                        if (ListedDate != DateTime.MinValue) return true;
                        ListedDate = orig;
                        return false;
                    }
                case "Subject":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return false; //Non-nullable
                        Subject = text;
                        return true;
                    }
                case "Discount":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return false;
                        IsDiscountable = text == "True";
                        return true;
                    }
                case "Status":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return false; //Non-nullable
                        ItemStatus = text;
                        return true;
                    }
                case "Condition":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return false; //Non-nullable
                        Condition = text;
                        return true;
                    }
                case "Description":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return text == null; //Non-nullable
                        Description = text;
                        return true;
                    }
            }
            return false;
        }

        public Video ConvertVideoItemToVideo()
        {
            var newVideo = new Video
            {
                Id = VideoId,
                Title = Title,
                EAN = Ean,
                Publisher = Publisher,
                VideoFormat = VideoFormat,
                AudienceRating = AudienceRating,
                ItemImage = VideoImage
            };

            return newVideo;
        }

        public Item ConvertVideoItemToItem()
        {
            var controller = new ConsignorController();
            var consignor = controller.GetConsignorByFullName(ConsignorName);
            

            var newItem = new Item
            {
                Id = ItemId,
                VideoId = VideoId,
                ItemType = "Video",
                ListedPrice = ListedPrice,
                ListedDate = ListedDate,
                Subject = Subject,
                IsDiscountable = IsDiscountable,
                Status = ItemStatus,
                Barcode = Barcode,
                Condition = Condition,
                Consignor = consignor,
                ConsignorId = consignor.Id
            };

            return newItem;
        }

        public int Accept(IItemVisitor itemVisitor)
        {
            return itemVisitor.Visit(this);
        }
    }

}
