using System;
using System.Globalization;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
//using DataModel.Data.TransactionalLayer.Repositories;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;
using Inventory.Controller.Interfaces;

namespace Inventory.Controller.Elements.ItemElements
{
    public class BookItem : ObservableObjects, IItemElement 
    {
        public BookItem()
        { }
        
        public BookItem(Book book, Item item)
        {
            BookId = book.Id;
            ItemId = item.Id;
            Title = book.Title;
            Isbn = book.ISBN;
            Author = book.Author;
            Binding = book.Binding;
            NumberOfPages = book.NumberOfPages;
            PublicationDate = book.PublicationDate;
            TradeInValue = book.TradeInValue;
            BookImage = book.ItemImage;
            IsDiscountable = item.IsDiscountable;
            ListedDate = item.ListedDate;
            ListedPrice = item.ListedPrice;
            Subject = item.Subject;
            ItemStatus = item.Status;
            Condition = item.Condition;
            Description = item.Description;
            Barcode = item.Barcode;
            ConsignorName = item.Consignor.Consignor_Person.FirstName + " " + item.Consignor.Consignor_Person.LastName;
        }
        
        
        //Define members
        public int BookId { get; set; }
        public int ItemId { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Binding { get; set; }
        public int? NumberOfPages { get; set; }
        public DateTime? PublicationDate { get; set; }
        public double? TradeInValue { get; set; }
        public string BookImage { get; set; }
        public double ListedPrice { get; set; }
        public DateTime ListedDate { get; set; }
        public string Subject { get; set; }
        public string Barcode { get; set; }
        public double LowestNewPrice { get;set;}
        public double LowestUsedPrice { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public string ConsignorName { get; set; }
        public bool IsDiscountable { get; set; }
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
                        controller.UpdateItem(ConvertBookItemToItem());
                    }

                }
                OnPropertyChanged();
            }
        }

        public bool SetProperty(string column, string text)
        {   
            switch(column)
            {
                case "Title":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return false; //Non-nullable
                        Title = text;
                        return true;
                    }
                case "ISBN":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return false; //Non-nullable
                        {
                            Isbn = text;
                            return true;
                        }
                    }
                case "Author":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return text == null; //Nullable
                        Author = text;
                        return true;
                    }
                case "Consignor Name":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return false; //Non-nullable
                        ConsignorName = text;
                        return true;
                    }
                case "Binding":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return text == null; //Nullable
                        Binding = text;
                        return true;
                    }
                case "# Of Pages":
                    {
                        if (!String.IsNullOrEmpty(text))
                        {
                            var orig = NumberOfPages;
                            int i;
                            
                            //Get the # of pages from the text or return -1 
                            NumberOfPages = int.TryParse(text, out i) ? i : -1;
                            if (!(NumberOfPages < 0)) return true;
                            
                            //If -1 cancel the edit
                            NumberOfPages = orig;
                            return false;
                        }
                        NumberOfPages = null;
                        return true;    //nullable
                    }
                case "PublicationDate":
                    {
                        if (String.IsNullOrEmpty(text)) return true; //nullable
                        DateTime d;

                        PublicationDate = DateTime.TryParse(text, out d) ? (DateTime?)d : null;
                        return PublicationDate != null;
                    }
                case "TradeInValue":
                    {
                        if (!String.IsNullOrEmpty(text))
                        {
                            var orig = NumberOfPages;
                            double i;

                            //Get the value from the text or return -1 
                            TradeInValue = double.TryParse(text, out i) ? i : -1;
                            if (!(TradeInValue < 0)) return true;

                            //If -1 cancel the edit
                            TradeInValue = orig;
                            return false;
                        }
                        TradeInValue = null;
                        return true;    //nullable
                    }
                case "BookImage":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return text == null; //Nullable
                        BookImage = text;
                        return true;
                    }
                case "ListedPrice":
                    {
                        if (String.IsNullOrEmpty(text)) return false; //Non nullable
                        
                        var orig = ListedPrice;
                        
                        //Get value from text
                        ListedPrice = double.Parse(text, NumberStyles.Currency);
                        if (!(ListedPrice < 0)) return true;
                        
                        //Cancel the edit if less than 0
                        ListedPrice = orig;
                        return false;
                    }
                case "Date Listed":
                    {
                        if (String.IsNullOrEmpty(text)) return false; //Non nullable
                        
                        //Save old value and try to parse from text
                        var orig = ListedDate;
                        DateTime d;
                        ListedDate = DateTime.TryParse(text, out d) ? d : DateTime.MinValue;
                        
                        //Cancel edit if parse failed
                        if (ListedDate != DateTime.MinValue) return true;
                        ListedDate = orig;
                        return false;
                    }
                case "Subject":
                {
                    if (String.IsNullOrWhiteSpace(text)) return text == null; //Non-Nullable
                    Subject = text;
                    return true;
                }
                case "Status":
                {
                    if (String.IsNullOrWhiteSpace(text)) return text == null; //Non-Nullable
                    ItemStatus = text;
                    return true;
                }
                case "Discount":
                {
                    if (String.IsNullOrWhiteSpace(text)) return false;
                    IsDiscountable = text == "True";
                    return true;
                }
                case "Condition":
                {
                    if (String.IsNullOrWhiteSpace(text)) return false;
                    Condition = text;
                    return true;
                }
                case "Description":
                {
                    if (String.IsNullOrWhiteSpace(text)) return text == null;
                    Description = text;
                    return true;
                }              
            } 
            return false;
        }

        public Book ConvertBookItemToBook()
        {
            var newBook = new Book
            {
                Id = BookId,
                Title = Title,
                ISBN = Isbn,
                Author = Author,
                Binding = Binding,
                NumberOfPages = NumberOfPages,
                PublicationDate = PublicationDate,
                TradeInValue = TradeInValue,
                ItemImage = BookImage
            };

            return newBook;
        }

        public Item ConvertBookItemToItem()
        {
            var controller = new ConsignorController();
            
            var consignor = controller.GetConsignorByFullName(ConsignorName);
            

            var newItem = new Item
            {
                Id = ItemId,
                BookId = BookId,
                ItemType = "Book",
                ListedPrice = ListedPrice,
                ListedDate = ListedDate,
                Subject = Subject,
                Status = ItemStatus,
                Barcode = Barcode,
                IsDiscountable = IsDiscountable,
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
