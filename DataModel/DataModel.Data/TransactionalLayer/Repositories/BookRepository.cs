using System;
using System.Collections.Generic;
using System.Linq;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class BookRepository : GenericItemRepositoryBase<Book>, IBookRepository
    {

        public override int AddNewItem(Book book)
        {

            if (book.Items_TItems.ElementAt(0).Book != null)
            {
                throw new NullReferenceException(
                    "Item member Book must be null when adding item or SQL will throw exception saying violated relationship property");
            }

            //Give dummy value for barcode
            if (book.Items_TItems != null)
            {
                book.Items_TItems.ElementAt(0).Barcode = "9999999999999";
            }

            //Validation if a record already exists for this book
            var oldBook = GetBookByTitle(book.Title);
            if (oldBook != null)
            {

                Validation.ItemDuplicate(book, oldBook);

                //Update Items property
                if (book.Items_TItems != null && book.Items_TItems.Count == 1)
                    oldBook.Items_TItems.Add(book.Items_TItems.ElementAt(0)); //Add new item to old book record
                else
                    throw new ArgumentException(
                        "Can't add more than one item to a book at the same time, or Book.Items is null");
            }
            //Book doesn't exist so add it
            else
            {
                Validation.StringRequire(book.Title);
                Validation.StringRequire(book.ISBN);
                Validation.ItemDuplicate(book);
                Validation.TitleLength(book.Title);
                Validation.IsbnLength(book.ISBN);
                Validation.AuthorLength(book.Author);
                Validation.ImageLength(book.ItemImage);
                //Validation.BoolRequire(book.IsAudioBook);

                Context.Books.Add(book);
            }
            //InsertOrUpdateAndGetId(book);
            Context.SaveChanges();

            //Set Barcode here because we need Id
            if (book.Items_TItems != null)
            {
                var itemId = book.Items_TItems.ElementAt(0).Id;
                return itemId;
            }
            return -1;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            var query = from b in Context.Books
                        orderby b.Title
                        select b;

            return query.ToList();
        }

        public Book GetBookByIsbn(string isbn)
        {
            var book = (from s in Context.Books
                        where s.ISBN == isbn
                        select s).FirstOrDefault<Book>();

            return book;
        }

        public Book GetBookByTitle(string title)
        {
            var book = (from s in Context.Books
                        where s.Title == title
                        select s).FirstOrDefault<Book>();

            return book;
        }
    }
}
