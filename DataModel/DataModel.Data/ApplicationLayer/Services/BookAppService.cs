using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class BookAppService : GenericItemAppService<Book>, IBookAppService 
    {
        private readonly IBookRepository _bookRepository;

        public BookAppService(IBookRepository bookRepository)
            : base(bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public GetAllBooksOutput GetAllBooks()
        {
            return new GetAllBooksOutput
            {
                Books = _bookRepository.GetAllBooks().Select(i => new BookDto(i)).ToList()
            };
        }

        public GetBookByIsbnOutput GetBookByIsbn(GetBookByIsbnInput input)
        {
            return new GetBookByIsbnOutput
            {
                Book = new BookDto(_bookRepository.GetBookByIsbn(input.Isbn))
            };
        }

        public AddNewBookOutput AddNewBook(AddNewBookInput input)
        {
            return new AddNewBookOutput
            {
                Id = _bookRepository.AddNewItem(input.Book.ConvertToBook())
            };
        }

        public UpdateBookOutput UpdateBook(UpdateBookInput input)
        {
            return new UpdateBookOutput
            {
                Id = _bookRepository.UpdateItem(input.Book.ConvertToBook())
            };
        }
    }
}
