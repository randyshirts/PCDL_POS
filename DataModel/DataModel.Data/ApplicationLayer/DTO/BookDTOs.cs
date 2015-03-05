using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class BookDto : GenericItemDto<Book>
    {
        public BookDto(Book book)
            : base(book)
        {
            Id = book.Id;
            Title = book.Title;
            Isbn = book.ISBN;
            Author = book.Author;
            Binding = book.Binding;
            NumberOfPages = book.NumberOfPages;
            PublicationDate = book.PublicationDate;
            TradeInValue = book.TradeInValue;
            ItemImage = book.ItemImage;
            Items_Books = book.Items_TItems;
        }
        
        
        [Required]
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Binding { get; set; }
        public int? NumberOfPages { get; set; }
        public DateTime? PublicationDate { get; set; }
        public double? TradeInValue { get; set; }
        public ICollection<Item> Items_Books { get; set; }

        public Book ConvertToBook()
        {
            return new Book()
            {
                Id = Id,
                Title = Title,
                ISBN = Isbn,
                Author = Author,
                Binding = Binding,
                NumberOfPages = NumberOfPages,
                PublicationDate = PublicationDate,
                TradeInValue = TradeInValue,
                ItemImage = ItemImage,
                Items_TItems = Items_Books,
            };
        }
        
    }

    public class GetAllBooksOutput : IOutputDto
    {
        public List<BookDto> Books { get; set; }
    }

    public class GetBookByIsbnOutput : IOutputDto
    {
        public BookDto Book { get; set; }
    }

    public class GetBookByIsbnInput : IInputDto
    {
        public string Isbn { get; set; }
    }

    public class AddNewBookOutput : IOutputDto
    {
        public int Id { get; set; }
    }

    public class AddNewBookInput : IInputDto
    {
        public BookDto Book { get; set; }
    }
         
    public class UpdateBookOutput : IOutputDto
    {
        public int Id { get; set; }
    }

    public class UpdateBookInput : IInputDto
    {
        public BookDto Book { get; set; }
    }
}
