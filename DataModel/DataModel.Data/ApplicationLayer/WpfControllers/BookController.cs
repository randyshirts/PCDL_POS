using System.Collections.Generic;
using System.Linq;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class BookController : GenericItemController<Book>, IBookController
    {
        public override IEnumerable<Book> GetAllItems()
        {
            using (var repo = new BookRepository())
            {
                var app = new BookAppService(repo);
                var output = app.GetAllBooks();
                return output.Books.Select(book => book.ConvertToBook()).ToList();
            }  
        }

        public Book GetBookByIsbn(string isbn)
        {
            var input = new GetBookByIsbnInput
            {
                Isbn = isbn
            };
            using (var repo = new BookRepository())
            {
                var app = new BookAppService(repo);
                var output = app.GetBookByIsbn(input);
                return output.Book.ConvertToBook();
            }   
        }

        public override int UpdateItem(Book updatedBook)
        {
            var input = new UpdateBookInput
            {
                Book = new BookDto(updatedBook)
            };
            using (var repo = new BookRepository())
            {
                var app = new BookAppService(repo);
                return app.UpdateBook(input).Id;
            }
        }

        public override int AddNewItem(Book book)
        {

            var itemInput = new GetItemByIdInput();
            var barcodeInput = new SetItemBarcodeInput();
            var updateInput = new UpdateItemInput();

            var input = new AddNewBookInput
            {
                Book = new BookDto(book)
            };
            using (var repo = new BookRepository())
            {
                var app = new BookAppService(repo);
                itemInput.Id = app.AddNewBook(input).Id;
            }
            using (var itemRepo = new ItemRepository())
            {
                var app = new ItemAppService(itemRepo);
                var thisItem = app.GetItemById(itemInput);
                barcodeInput.Item = thisItem.Item;
                var barcodeOutput = app.SetItemBarcode(barcodeInput);
                thisItem.Item.Barcode = barcodeOutput.Barcode;
                updateInput.Item = thisItem.Item;
                app.UpdateItem(updateInput);
            }
            return itemInput.Id;
        }
    }
}
