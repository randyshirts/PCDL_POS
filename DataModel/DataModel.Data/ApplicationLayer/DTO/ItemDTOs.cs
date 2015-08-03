using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;
using Newtonsoft.Json;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class ItemDto : EntityDto
    {
        public ItemDto()
        {
        }

        public ItemDto(Item item)
        {
            Id = item.Id;
            Status = item.Status;
            ItemType = item.ItemType;
            ListedPrice = item.ListedPrice;
            SalePrice = item.SalePrice;
            CashPayout = item.CashPayout;
            ListedDate = item.ListedDate;
            Subject = item.Subject;
            Condition = item.Condition;
            IsDiscountable = item.IsDiscountable;
            Description = item.Description;
            Barcode = item.Barcode;
            BookId = item.BookId;
            Book = item.Book;
            GameId = item.GameId;
            Game = item.Game;
            TeachingAideId = item.TeachingAideId;
            TeachingAide = item.TeachingAide;
            OtherId = item.OtherId;
            Other = item.Other;
            VideoId = item.VideoId;
            Video = item.Video;
            ConsignorId = item.ConsignorId;
            Consignor = item.Consignor;
            Consignor.Consignor_Person = item.Consignor.Consignor_Person;
            ItemSaleTransactionId = item.ItemSaleTransactionId;
            ItemSaleTransaction = item.ItemSaleTransaction;
            ConsignorPmtId = item.ConsignorPmtId;
            ConsignorPmt = item.ConsignorPmt;
            if(ConsignorPmt != null) ConsignorPmt.DebitTransaction_ConsignorPmt = ConsignorPmt.DebitTransaction_ConsignorPmt;
        }

        [Required]
        public string Status { get; set; }
        [Required]
        public string ItemType { get; set; }
        [Required]
        public double ListedPrice { get; set; }
        public double? SalePrice { get; set; }
        public bool? CashPayout { get; set; }
        [Required]
        public DateTime ListedDate { get; set; }
        [Required]
        public string Subject { get; set; }
        public string Condition { get; set; }
        public bool IsDiscountable { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public int? BookId { get; set; }
        public Book Book { get; set; }
        public int? GameId { get; set; }
        public Game Game { get; set; }
        public int? TeachingAideId { get; set; }
        public TeachingAide TeachingAide { get; set; }
        public int? OtherId { get; set; }
        public Other Other { get; set; }
        public int? VideoId { get; set; }
        public Video Video { get; set; }
        public int ConsignorId { get; set; }
        public Consignor Consignor { get; set; }
        public int? ItemSaleTransactionId { get; set; }
        public ItemSaleTransaction ItemSaleTransaction { get; set; }
        public int? ConsignorPmtId { get; set; }
        public ConsignorPmt ConsignorPmt { get; set; }

        public Item ConvertToItem()
        {
            return new Item()
            {
                Id = Id,
                Status = Status,
                ItemType = ItemType,
                ListedPrice = ListedPrice,
                SalePrice = SalePrice,
                CashPayout = CashPayout,
                ListedDate = ListedDate,
                Subject = Subject,
                Condition = Condition,
                IsDiscountable = IsDiscountable,
                Description = Description,
                Barcode = Barcode,
                BookId = BookId,
                Book = Book,
                GameId = GameId,
                Game = Game,
                TeachingAideId = TeachingAideId,
                TeachingAide = TeachingAide,
                OtherId = OtherId,
                Other = Other,
                VideoId = VideoId,
                Video = Video,
                ConsignorId = ConsignorId,
                Consignor = Consignor,
                ItemSaleTransactionId = ItemSaleTransactionId,
                ItemSaleTransaction = ItemSaleTransaction,
                ConsignorPmtId = ConsignorPmtId,
                ConsignorPmt = ConsignorPmt,
            };
        }
    }

    public class SetItemBarcodeOutput : IOutputDto
    {
        public string Barcode { get; set; }
    }

    public class SetItemBarcodeInput : IInputDto
    {
        public ItemDto Item { get; set; }
    }

    public class GetItemByIsbnInput : IInputDto
    {
        public string Isbn { get; set; }
    }

    public class UpdateItemOutput : IOutputDto
    {
        public int Id { get; set; }
    }

    public class UpdateItemInput : IInputDto
    {
        public ItemDto Item { get; set; }
    }

    public class DeleteItemByIdInput : IInputDto
    {
        public int Id { get; set; }
    }

    public class GetItemsByConsignorNameOutput : IOutputDto
    {
        public List<ItemDto> Items { get; set; }   
    }

    public class GetItemsByConsignorNameInput : IInputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class GetItemsByPartOfBarcodeOutput : IOutputDto
    {
        public List<ItemDto> Items { get; set; }
    }

    public class GetItemsByPartOfBarcodeInput : IInputDto
    {
        public string Barcode { get; set; }
    }

    public class SearchAllItemsOutput : IOutputDto
    {
        public List<ItemDto> Items { get; set; }
    }

    public class SearchAllItemsInput : IInputDto
    {
        public string Barcode { get; set; }
        public string Status { get; set; }
        public string ItemType { get; set; }
        public string ConsignorName { get; set; }
        public DateTime? ListedDate { get; set; }
        public string Title { get; set; }
    }

    public class SearchItemsDateRangeInput : IInputDto
    {
        public DateTime? FromDate { get; set; } 
        public DateTime? EndDate { get; set; }  
        public string ConsignorName { get; set; }
        public string Status { get; set; }    
    }

    public class SearchItemsDateRangeOutput : IOutputDto
    {
        public List<ItemDto> Items { get; set; } 
    }

    public class SearchListItemsOutput : IOutputDto
    {
        public List<ItemDto> Items { get; set; }
    }

    public class SearchListItemsInput : IInputDto
    {
        public IEnumerable<ItemDto> ItemList { get; set; } 
        public string Barcode { get; set; }
        public string Status { get; set; }
        public string ItemType { get; set; }
        public string ConsignorName { get; set; }
        public DateTime? ListedDate { get; set; }
    }

    public class QueryItemsThatAreBooksOutput : IOutputDto
    {
        public List<ItemDto> Items { get; set; }
    }

    public class QueryItemsThatAreGamesOutput : IOutputDto
    {
        public List<ItemDto> Items { get; set; }
    }

    public class QueryItemsThatAreOthersOutput : IOutputDto
    {
        public List<ItemDto> Items { get; set; }
    }
    
    public class QueryItemsThatAreVideosOutput : IOutputDto
    {
        public List<ItemDto> Items { get; set; }
    }
    
    public class QueryItemsThatAreTeachingAidesOutput : IOutputDto
    {
        public List<ItemDto> Items { get; set; }
    }

    public class GetItemByIdOutput : IOutputDto
    {
        public ItemDto Item { get; set; } 
    }

    public class GetItemByIdInput : IInputDto
    {
        public int Id { get; set; }
    }

    
    public class GetAllItemsOutput : IOutputDto
    {
        public IEnumerable<ItemDto> Items { get; set; } 
    }

    public class GetItemTitleOutput : IOutputDto
    {
        public string Title { get; set; }
    }

    public class GetItemTitleInput : IInputDto
    {
        public Item Item { get; set; }
    }

    public class GetCurrentPriceOutput : IOutputDto
    {
        public double Price { get; set; }
    }

    public class GetCurrentPriceInput : IInputDto
    {
        public Item Item { get; set; }
    }
}
