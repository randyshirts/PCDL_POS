using System;
using System.Collections.Generic;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class Book : Entity, IGenericItem
    {
        #region Ctors
        public Book()
        {

        }

        //public Book(string title, string isbn, string author, string binding, int numberOfPages,
        //            DateTime publicationDate, double tradeInValue, bool isAudioBook, string bookImage, Item item)
        //{
        //    Items_Books = new Collection<Item>();
            
        //    Title = title;         
        //    ISBN = isbn;
        //    Author = author;
        //    Binding = binding;
        //    NumberOfPages = numberOfPages;
        //    PublicationDate = publicationDate;
        //    TradeInValue = tradeInValue;
        //    BookImage = bookImage;
        //    Items_Books.Add(item);
        //}


        #endregion


        #region Define Members
        //Define members
        //public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Binding { get; set; }
        public int? NumberOfPages { get; set; }
        public DateTime? PublicationDate { get; set; }
        public double? TradeInValue { get; set; }
        public string ItemImage { get; set; }
        public string EAN { get; set; }
        public string Manufacturer { get; set; }
        public virtual ICollection<Item> Items_TItems { get; set; }
        #endregion




    }
}
