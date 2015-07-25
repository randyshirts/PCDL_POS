using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Controller.Elements
{
    class ConsignorPayment
    {
        public ConsignorPayment()
        { }

        public ConsignorPayment(Item item)
        {
            Consignor = item.Consignor;

            //Item info
            Id = item.Id;
            Consignor_Item = item;
            Transaction = item.ItemSaleTransaction;
            ItemStatus = item.Status;
            ListedPrice = item.ListedPrice.ToString("C2");
            if (item.SalePrice != null)
            {
                SoldPrice = ((double)item.SalePrice).ToString("C2");
                if (item.ConsignorPmt == null)
                    ConsignorPortion = ((double)(item.SalePrice) * ConfigSettings.CONS_CREDIT_PAYOUT_PCT).ToString("C2");
                else
                {
                    if(item.ConsignorPmt.DebitTransaction_ConsignorPmt != null)
                        ConsignorPortion = item.ConsignorPmt.DebitTransaction_ConsignorPmt.DebitTotal.ToString("C2");
                    else
                        ConsignorPortion = ((double)(item.SalePrice) * ConfigSettings.CONS_CREDIT_PAYOUT_PCT).ToString("C2");         
                }
            }
            DateListed = item.ListedDate;
            if (item.ItemSaleTransaction != null)
                DateSold = item.ItemSaleTransaction.CreditTransaction_ItemSale.TransactionDate;
            Barcode = item.Barcode;
            IsDiscountable = item.IsDiscountable;
            Subject = item.Subject;
            PayoutNow = true;
            CashPayout = false;
            ItemType = item.ItemType;

            if (ItemType == "Book")
            {
                Title = item.Book.Title;
                IsbnEan = item.Book.ISBN;
                Consignor_Book = item.Book;
            }
            else if (ItemType == "Game")
            {
                if (item.Game.Title != null)
                    Title = item.Game.Title;

                if (item.Game.EAN != null)
                    IsbnEan = item.Game.EAN;

                Consignor_Game = item.Game;
            }
            else if (ItemType == "TeachingAide")
            {
                if (item.TeachingAide.Title != null)
                    Title = item.TeachingAide.Title;

                if (item.TeachingAide.EAN != null)
                    IsbnEan = item.TeachingAide.EAN;

                Consignor_TeachingAide = item.TeachingAide;
            }
            else if (ItemType == "Video")
            {
                if (item.Video.Title != null)
                    Title = item.Video.Title;

                if (item.Video.EAN != null)
                    IsbnEan = item.Video.EAN;

                Consignor_Video = item.Video;
            }
            else if (ItemType == "Other")
            {
                if (item.Other.Title != null)
                    Title = item.Other.Title;

                if (item.Other.EAN != null)
                    IsbnEan = item.Other.EAN;

                Consignor_Other = item.Other;
            }
        }


        //Define members
        public int Id { get; set; }
        public DateTime? DateListed { get; set; }
        public DateTime DateSold { get; set; }
        public Consignor Consignor { get; set; }
        public ItemSaleTransaction Transaction { get; set; }
        public Item Consignor_Item { get; set; }
        public Book Consignor_Book { get; set; }
        public Game Consignor_Game { get; set; }
        public Video Consignor_Video { get; set; }
        public Other Consignor_Other { get; set; }
        public TeachingAide Consignor_TeachingAide { get; set; }
        public string ItemType { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string IsbnEan { get; set; }
        public string ListedPrice { get; set; }
        public bool IsDiscountable { get; set; }
        public string SoldPrice { get; set; }
        public string Barcode { get; set; }
        public double LowestNewPrice { get; set; }
        public double LowestUsedPrice { get; set; }
        public bool PayoutNow { get; set; }
        public bool CashPayout { get; set; }

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
                        controller.UpdateItem(ConvertConsignorPaymentToItem());
                    }
                    
                }
                OnPropertyChanged();
            }
        }

        private string _consignorPortion;
        public string ConsignorPortion 
        {
            get { return _consignorPortion; } 
            set
            {
                _consignorPortion = value;
                OnPropertyChanged();
            } 
        }

        public bool SetProperty(string column, string text)
        {
            switch (column)
            {
                case "Type":
                {
                    if (String.IsNullOrWhiteSpace(text)) return false;
                    //Change the member property that is displayed in grid
                    ItemType = text;

                    //change the value inside the Item class
                    Consignor_Item.ItemType = text;

                    return true;
                }
                case "Title":
                {
                    if (String.IsNullOrWhiteSpace(text)) return false;
                    //Change the member property that is displayed in grid
                    Title = text;


                    switch (ItemType)
                    {
                        case "Book":
                            Consignor_Book.Title = text;   //change the value inside the Book class
                            return true;
                        case "Game":
                            Consignor_Game.Title = text;
                            return true;
                        case "Other":
                            Consignor_Other.Title = text;
                            return true;
                        case "Video":
                            Consignor_Video.Title = text;
                            return true;
                        case "TeachingAide":
                            Consignor_TeachingAide.Title = text;
                            return true;
                        default:
                            return false;
                    }
                }
                case "ISBN/EAN":
                {
                    if (text == null) return false;
                    //Change the member property that is displayed in grid
                    

                    switch (ItemType)
                    {
                        case "Book":
                            if (String.IsNullOrWhiteSpace(text)) return false;
                            IsbnEan = text;
                            Consignor_Book.ISBN = text;   //change the value inside the Book class
                            return true;
                        case "Game":
                            IsbnEan = text;
                            Consignor_Game.EAN = !String.IsNullOrWhiteSpace(text) ? text : null;
                            return true;
                        case "Other":
                            IsbnEan = text;
                            Consignor_Other.EAN = !String.IsNullOrWhiteSpace(text) ? text : null;
                            return true;
                        case "Video":
                            IsbnEan = text;
                            Consignor_Video.EAN = !String.IsNullOrWhiteSpace(text) ? text : null;
                            return true;
                        case "TeachingAide":
                            IsbnEan = text;
                            Consignor_TeachingAide.EAN = !String.IsNullOrWhiteSpace(text) ? text : null;
                            return true;
                        default:
                            return false;
                    }
                }
                case "Status":
                {
                    if (String.IsNullOrWhiteSpace(text)) return false;
                    //Change the member property that is displayed in grid
                    ItemStatus = text;

                    //change the value inside the Item class
                    Consignor_Item.Status = text;
                    
                    return true;
                }

                case "Subject":
                {
                    if (String.IsNullOrWhiteSpace(text)) return false;
                    //Change the member property that is displayed in grid
                    Subject = text;

                    //change the value inside the Item class
                    Consignor_Item.Subject = text;

                    return true;
                }

                case "Listed Price":
                {
                    if (String.IsNullOrWhiteSpace(text)) return false;
                    //Change the member property that is displayed in grid
                    ListedPrice = text;

                    //change the value inside the Item class
                    Consignor_Item.ListedPrice = double.Parse(text, NumberStyles.Currency);
                            
                    return true;
                }
                case "Sold Price":
                {
                    if (String.IsNullOrWhiteSpace(text)) return false;
                    //Change the member property that is displayed in grid
                    SoldPrice = text;

                    //change the value inside the Item class
                    Consignor_Item.SalePrice = double.Parse(text, NumberStyles.Currency);

                    return true;
                }
                case "Consignor Pmt":
                {
                    if (String.IsNullOrWhiteSpace(text)) return false;
                    //Change the member property that is displayed in grid
                    ConsignorPortion = text;
                            
                    /*
                            //change the value inside the Item class
                            Consignor_Item. = double.Parse(text, NumberStyles.Currency);
                            */
                              
                    return true;
                }

                case "Date Listed":
                {
                    if (String.IsNullOrEmpty(text)) return false; //Non nullable
                    var orig = DateListed;
                    DateTime d;
                    DateListed = DateTime.TryParse(text, out d) ? d : DateTime.MinValue;
                    if (DateListed != DateTime.MinValue) return true;
                    DateListed = orig;
                    return false;
                }
                case "Date Sold":
                {
                    if (String.IsNullOrEmpty(text)) return false; //Non nullable
                    var orig = DateSold;
                    DateTime d;
                    DateSold = DateTime.TryParse(text, out d) ? d : DateTime.MinValue;
                    if (DateSold == DateTime.MinValue)
                    {
                        //Cancel edit
                        DateSold = orig;
                        return false;                               
                    }
                    if (Transaction == null) return false;
                    Transaction.CreditTransaction_ItemSale.TransactionDate = DateSold;
                    return true;
                }
                case "Cash Payout":
                {
                    if (String.IsNullOrWhiteSpace(text)) return false;
                    //Change the member property that is displayed in grid
                    CashPayout = Boolean.Parse(text);

                    /*
                            //change the value inside the Person class
                            Consignor_Person.MailingAddresses.FirstOrDefault().ZipCode = text;
                            */
                    return true;
                }
                case "Discount":
                    {
                        if (String.IsNullOrWhiteSpace(text)) return false;
                        IsDiscountable = text == "True";
                        return true;
                    }
            }
            return false;
        }

        public Item ConvertConsignorPaymentToItem()
        {
            var newItem = new Item();

            if (SoldPrice != null)
            {
                newItem.Id = Id;
                newItem.ItemType = ItemType;
                newItem.Condition = Consignor_Item.Condition;
                if (DateListed != null) newItem.ListedDate = (DateTime)DateListed;
                newItem.Consignor = Consignor;
                newItem.ConsignorId = Consignor_Item.ConsignorId;
                newItem.ItemSaleTransaction = Transaction;
                newItem.ListedPrice = double.Parse(ListedPrice, NumberStyles.Currency);
                newItem.SalePrice = double.Parse(SoldPrice, NumberStyles.Currency);
                newItem.CashPayout = CashPayout;
                newItem.Status = ItemStatus;
                newItem.Subject = Subject;
                newItem.Barcode = Barcode;
                newItem.IsDiscountable = IsDiscountable;
                newItem.Book = Consignor_Book;
                newItem.BookId = Consignor_Item.BookId;               
                newItem.Game = Consignor_Game;
                newItem.GameId = Consignor_Item.GameId;
                newItem.Other = Consignor_Other;
                newItem.OtherId = Consignor_Item.OtherId;
                newItem.TeachingAide = Consignor_TeachingAide;
                newItem.TeachingAideId = Consignor_Item.TeachingAideId;
                newItem.Video = Consignor_Video;
                newItem.VideoId = Consignor_Item.VideoId;
            
            }
            else
            {
                newItem.Id = Id;
                newItem.ItemType = ItemType;
                newItem.Condition = Consignor_Item.Condition;
                if (DateListed != null) newItem.ListedDate = (DateTime)DateListed;
                newItem.Consignor = Consignor;
                newItem.ConsignorId = Consignor_Item.ConsignorId;
                newItem.ItemSaleTransaction = Transaction;
                newItem.ListedPrice = double.Parse(ListedPrice, NumberStyles.Currency);
                newItem.Status = ItemStatus;
                newItem.Subject = Subject;
                newItem.Barcode = Barcode;
                newItem.IsDiscountable = IsDiscountable;
                newItem.Book = Consignor_Book;
                newItem.BookId = Consignor_Item.BookId;
                newItem.Game = Consignor_Game;
                newItem.GameId = Consignor_Item.GameId;
                newItem.Other = Consignor_Other;
                newItem.OtherId = Consignor_Item.OtherId;
                newItem.TeachingAide = Consignor_TeachingAide;
                newItem.TeachingAideId = Consignor_Item.TeachingAideId;
                newItem.Video = Consignor_Video;
                newItem.VideoId = Consignor_Item.VideoId;
            }
            return newItem;
        }

        public void ReportError(string column, string text)
        {
            if (String.IsNullOrEmpty(text) && ((column == "ISBN/EAN") || (column == "Title") || (column == "Listed Price")))
            {
                MessageBox.Show("Input Error - Column = '" + column +
                                             "' - Entry must have a value - Try again", "Error");
                return;
            }
            switch (column)
            {
                case "Title":
                {
                    MessageBox.Show("Input Error - '" + text +
                                             "' - Title must be 150 characters or less - Try again", "Error");
                    return;
                }
                case "ISBN/EAN":
                {
                    MessageBox.Show("Input Error - '" + text +
                                             "' - ISBN (books) must contain only numbers and must be 10 or 13 characters in length. " +
                                             "EAN (all other items) must be 13 characters in length - Try again", "Error");
                    return;
                }
                case "Status":
                {
                    MessageBox.Show("Input Error - '" + text +
                                    "' - Status must be a value - Try again", "Error");
                    return;
                }
                case "Listed Price":
                case "Sold Price":
                case "Consignor Pmt":
                {
                    MessageBox.Show("Input Error - '" + text +
                                    "' - Currency value must only contain numbers, '$', and decimals ('.') - Try again", "Error");
                    return;
                }
                default:
                {
                    MessageBox.Show("Input Error - '" + text + "' - Try again", "Error");
                    return;
                }
            }
        }

        public int Accept(IItemVisitor itemVisitor)
        {
            return itemVisitor.Visit(this);
        }
    
    }
}
