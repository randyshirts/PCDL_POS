﻿using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Repositories
{   
    public interface IBookRepository : IRepository<Book>, IGenericItemRepository<Book>
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookByIsbn(string isbn);
        //int UpdateBook(Book updatedBook);
        //int AddNewItem(Book book);
    }

    public interface IClassPmtTransactionRepository : IRepository<ClassPmtTransaction>
    {

    }

    public interface IConsignorRepository : IRepository<Consignor>
    {
        int AddNewConsignor(Consignor consignor);
        void DeleteConsignerById(int id);
        void UpdateConsignor(Consignor updatedConsignor);        
        Consignor GetConsignorByName(string firstName, string lastName);
        List<string> GetConsignorNames();
        List<string> GetConsignorNamesLastnameFirst();
        Consignor GetConsignorByFullName(string fullName);
        double GetConsignorCreditBalance(string name);

    }

    public interface IConsignorPmtRepository : IRepository<ConsignorPmt>
    {
        int AddNewConsignorPmt(ConsignorPmt consignorPmt);
        ConsignorPmt GetConsignorPmtById(int id);
        IEnumerable<ConsignorPmt> GetConsignorPmtsByConsignorId(int id);
    }

    public interface ICreditTransactionRepository : IRepository<CreditTransaction>
    {

    }

    public interface IDebitTransactionRepository : IRepository<DebitTransaction>
    {

    }

    public interface IEmailRepository : IRepository<Email>
    {

    }

    public interface IGameRepository : IRepository<Game>, IGenericItemRepository<Game>
    {

    }

    public interface IItemRepository : IRepository<Item>
    {
        string SetItemBarcode(Item item);
        int UpdateItem(Item updatedItem);
        void DeleteItemById(int id);

        IEnumerable<Item> SearchAllItems(string barcode, string status, string itemType, string consignorName,
            DateTime? listedDate);

        IEnumerable<Item> SearchListItems(IEnumerable<Item> list, string barcode, string status, string itemType,
            string consignorName, DateTime? listedDate);

        List<Item> GetItemsByConsignorName(string firstName, string lastName);
        List<Item> GetItemsByPartOfBarcode(string barcode);

        IEnumerable<Item> QueryItemsThatAreBooks();
        IEnumerable<Item> QueryItemsThatAreGames();
        IEnumerable<Item> QueryItemsThatAreOthers();
        IEnumerable<Item> QueryItemsThatAreTeachingAides();
        IEnumerable<Item> QueryItemsThatAreVideos();
    }

    public interface IItemSaleTransactionRepository : IRepository<ItemSaleTransaction>
    {
        int AddNewItemSaleTransaction(ItemSaleTransaction transaction);
        void DeleteTransactionById(int id);
        void UpdateItemSaleTransaction(ItemSaleTransaction updatedTransaction);
        List<ItemSaleTransaction> GetAllItemSaleTransactions();
        ItemSaleTransaction GetItemSaleTransactionById(int id);
    }

    public interface IMailingAddressRepository : IRepository<MailingAddress>
    {

    }

    public interface IMemberRepository : IRepository<Member>
    {

    }

    public interface IOtherRepository : IRepository<Other>, IGenericItemRepository<Other>
    {

    }

    public interface IPersonRepository : IRepository<Person>
    {
        void UpdatePerson(Person updatedPerson);
        IEnumerable<Person> QueryPersonsThatAreConsigners();
    }

    public interface IPhoneNumberRepository : IRepository<PhoneNumber>
    {

    }

    public interface ISpaceRentalTransactionRepository : IRepository<SpaceRentalTransaction>
    {

    }

    public interface IStoreCreditTransactionRepository : IRepository<StoreCreditTransaction>
    {
        IEnumerable<StoreCreditTransaction> GetStoreCreditTransactionsByConsignorId(int id);
    }

    public interface IStoreCreditPmtRepository : IRepository<StoreCreditPmt>
    {
        IEnumerable<StoreCreditPmt> GetStoreCreditPmtsByConsignorId(int id);
    }

    public interface ITeachingAideRepository : IRepository<TeachingAide>, IGenericItemRepository<TeachingAide>
    {

    }

    public interface IVideoRepository : IRepository<Video>, IGenericItemRepository<Video>
    {
        //int AddNewItem(Video video);
        IEnumerable<Video> GetAllVideos();
    }

    public interface IVolunteerRepository : IRepository<Volunteer>
    {

    }
}
