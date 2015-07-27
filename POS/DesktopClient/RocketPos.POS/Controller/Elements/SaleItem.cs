using System;
using System.Linq;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using POS.Controller.Interfaces;
using POS.Controller.Visitors;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;

//using Inventory.Controller.Interfaces;

namespace POS.Controller.Elements
{
    public class SaleItem : ObservableObjects, ISaleElement
    {
        //Define members
        public int Quantity { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public double LinePrice { get; set; }
        public double Tax { get; set; }
        public double CityTaxAmount { get; set; }
        public double StateTaxAmount { get; set; }
        public double CountyTaxAmount { get; set; }
        public string BookImage { get; set; }
        public double UnitPrice { get; set; }
        public DateTime ListedDate { get; set; }
        public double SaleAmount { get; set; }
        public double DateDiscount { get; set; }
        public double MemberDiscount { get; set; }
        public double AddlDiscount { get; set; }
        public double DiscountAmount { get; set; }
        public double TaxAmount { get; set; }
        public bool IsDiscountable { get; set; }
        public string Barcode { get; set; }


        public SaleItem(string barcode, bool isMember)
        {
            var controller = new ItemController();
            var item = controller.GetItemsByPartOfBarcode(barcode).FirstOrDefault();


            if (item == null) return;
            Id = item.Id;
            IsDiscountable = item.IsDiscountable;
            ListedDate = item.ListedDate;
            UnitPrice = item.ListedPrice;
            Quantity = 1;
            Barcode = barcode;

            //Find date discount
            var dateSpan = DateTimeSpan.CompareDates(ListedDate, DateTime.Now);

            if (IsDiscountable)
            {
                if (dateSpan.Months < 6)
                    DateDiscount = dateSpan.Months * .10; //10% discount for every month it has been listed
                else
                    DateDiscount = .50;
            }
            else
            {
                DateDiscount = 0;
            }           

            //Get member, volunteer, owner discount
            ComputeMemberDiscount(isMember);

            AddlDiscount = 0;

            //Compute taxes and linePrice
            ComputeTaxesAndLinePrice();
            

            switch (item.ItemType)
            {
                case ("Book"):
                    {
                        Title = item.Book.Title;
                        break;
                    }
                case ("Game"):
                    {
                        Title = item.Game.Title;
                        break;
                    }
                case ("Other"):
                    {
                        Title = item.Other.Title;
                        break;
                    }
                case ("TeachingAide"):
                    {
                        Title = item.TeachingAide.Title;
                        break;
                    }
                case ("Video"):
                    {
                        Title = item.Video.Title;
                        break;
                    }
            }

        }

        public SaleItem(Item item)
        {
            Id = item.Id;
            IsDiscountable = item.IsDiscountable;
            ListedDate = item.ListedDate;
            UnitPrice = item.ListedPrice;
            Quantity = 1;
            Barcode = item.Barcode;

            //Find date discount
            var dateDiff = Math.Abs(DateTime.Now.Month - ListedDate.Month);
            if (DateTime.Now.Day < ListedDate.Day)
                dateDiff -= 1;

            if (dateDiff < 6)
                DateDiscount = dateDiff * .10;  //10% discount for every month it has been listed
            DateDiscount = .50;

            
            AddlDiscount = 0;

            //Compute tax
            //Tax = ??

            //Compute LinePrice
            LinePrice = UnitPrice * (DateDiscount + AddlDiscount + MemberDiscount) + Tax;

            switch (item.ItemType)
            {
                case ("Book"):
                    {
                        Title = item.Book.Title;
                        break;
                    }
                case ("Game"):
                    {
                        Title = item.Game.Title;
                        break;
                    }
                case ("Other"):
                    {
                        Title = item.Other.Title;
                        break;
                    }
                case ("TeachingAide"):
                    {
                        Title = item.TeachingAide.Title;
                        break;
                    }
                case ("Video"):
                    {
                        Title = item.Video.Title;
                        break;
                    }
            }
        }


        public Item ConvertSaleItemToItem()
        {
            Item thisItem;

            var controller = new ItemController();
            thisItem = controller.GetItemById(Id);

            return thisItem;

        }

        public void ComputeMemberDiscount(bool isMember)
        {
            //Implement member discounts 
            if (isMember)
            {
                MemberDiscount = ConfigSettings.MEMBER_PURCHASE_DISCOUNT_PCT;
                //Set SaleAmount - Amount reported to consignor
                SaleAmount = UnitPrice - UnitPrice * (MemberDiscount + DateDiscount);
            }
            else
            {
                MemberDiscount = 0.0;
                //Set SaleAmount - Amount reported to consignor
                SaleAmount = UnitPrice - DateDiscount * UnitPrice;
            }
        }

        public void ComputeTaxesAndLinePrice()
        {
            var stateTaxVisitor = new StateTaxVisitor();
            var stateTax = Accept(stateTaxVisitor);

            var cityTaxVisitor = new CityTaxVisitor();
            var cityTax = Accept(cityTaxVisitor);

            var countyTaxVisitor = new CountyTaxVisitor();
            var countyTax = Accept(countyTaxVisitor);

            //Compute DiscountAmount
            DiscountAmount = UnitPrice * (DateDiscount + AddlDiscount + MemberDiscount);

            //Compute Taxes and LinePrice
            var noTaxAmount = UnitPrice - DiscountAmount;

            CityTaxAmount = cityTax * noTaxAmount;        //Round up for taxes
            StateTaxAmount = stateTax * noTaxAmount;
            CountyTaxAmount = countyTax * noTaxAmount;
            Tax = cityTax + stateTax + countyTax;
            TaxAmount = Math.Ceiling((CityTaxAmount + StateTaxAmount + CountyTaxAmount) * 100) / 100;

            LinePrice = Math.Ceiling((noTaxAmount + TaxAmount) * 100) / 100;    //Round up for total price
        }

        public double Accept(ITaxVisitor itemVisitor)
        {
            return itemVisitor.Visit(this);
        }

        public string Accept(IReceiptVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }


}
