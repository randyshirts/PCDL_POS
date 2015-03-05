using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataModel.Data.DataLayer;
using DataModel.Data.DataLayer.Entities;
using RocketPos.Data.DataLayer;

namespace RocketPos.Data.TransactionalLayer
{
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public sealed class BusinessContext : IDisposable
    {
        private readonly DataContext _context;
        private bool _disposed;

        public BusinessContext()
        {
            _context = new DataContext();
        }

        #region Add to Database Methods
        //[Obsolete]
        //public void AddNewItem(Item item)
        //{
        //    Check.StringRequire(item.ItemType);
        //    Check.StringRequire(item.Status);
        //    Check.StringRequire(item.Condition);
        //    Check.PriceRequire(item.ListedPrice);
        //    Check.TitleLength(item.Book.Title);

        //    _context.Items.Add(item);
        //    _context.SaveChanges();

        //}

        //public int AddNewBook(Book book)
        //{

        //    if (book.Items_Books.ElementAt(0).Book != null)
        //    {
        //        throw new NullReferenceException("Item member Book must be null when adding item or SQL will throw exception saying violated relationship property");
        //    }

        //    //Give dummy value for barcode
        //    if (book.Items_Books != null)
        //    {
        //        book.Items_Books.ElementAt(0).Barcode = "9999999999999";
        //    }

        //    //Check if a record already exists for this book
        //    var oldBook = GetBookByIsbn(book.ISBN);
        //    if (oldBook != null)
        //    {

        //        Check.ItemDuplicate(book, oldBook);

        //        //Update Items property
        //        if (book.Items_Books != null && book.Items_Books.Count == 1)
        //            oldBook.Items_Books.Add(book.Items_Books.ElementAt(0));     //Add new item to old book record
        //        else
        //            throw new ArgumentException("Can't add more than one item to a book at the same time, or Book.Items is null");
        //    }
        //    //Book doesn't exist so add it
        //    else
        //    {
        //        Check.StringRequire(book.Title);
        //        Check.StringRequire(book.ISBN);
        //        Check.ItemDuplicate(book);
        //        Check.TitleLength(book.Title);
        //        Check.IsbnLength(book.ISBN);
        //        Check.AuthorLength(book.Author);
        //        Check.ImageLength(book.BookImage);
        //        //Check.BoolRequire(book.IsAudioBook);

        //        _context.Books.Add(book);
        //    }
        //    _context.SaveChanges();

        //    //Set Barcode here because we need Id
        //    if (book.Items_Books != null)
        //    {
        //        var itemId = book.Items_Books.ElementAt(0).Id;
        //        var thisItem = GetItemById(itemId);
        //        thisItem.Barcode = SetItemBarcode(thisItem);


        //        _context.SaveChanges();
        //        return itemId;
        //    }
        //    return -1;
        //}

        //public int AddNewGame(Game game)
        //{
        //    if (game.Items_Games.ElementAt(0).Game != null)
        //    {
        //        throw new NullReferenceException("Item member Game must be null when adding item or SQL will throw exception saying violated relationship property");
        //    }

        //    //Give dummy value for barcode
        //    if (game.Items_Games != null)
        //    {
        //        game.Items_Games.ElementAt(0).Barcode = "9999999999999";
        //    }

        //    //Check if a record already exists for this game
        //    var oldGame = GetGameByTitle(game.Title);
        //    if (oldGame != null)
        //    {

        //        Check.ItemDuplicate(game, oldGame);

        //        //Update Items property
        //        if (game.Items_Games != null && game.Items_Games.Count == 1)
        //            oldGame.Items_Games.Add(game.Items_Games.ElementAt(0));     //Add new item to old game record
        //        else
        //            throw new ArgumentException("Can't add more than one item to a game at the same time, or Game.Items is null");
        //    }
        //    //Game doesn't exist so add it
        //    else
        //    {
        //        Check.StringRequire(game.Title);
        //        Check.TitleLength(game.Title);
        //        Check.ItemDuplicate(game);
        //        Check.EanLength(game.EAN);
        //        Check.ImageLength(game.GameImage);
        //        _context.Games.Add(game);
        //    }
        //    _context.SaveChanges();

        //    //Set Barcode here because we need Id
        //    if (game.Items_Games != null)
        //    {
        //        var itemId = game.Items_Games.ElementAt(0).Id;
        //        var thisItem = GetItemById(itemId);
        //        thisItem.Barcode = SetItemBarcode(thisItem);


        //        _context.SaveChanges();
        //        return itemId;
        //    }
        //    return -1;
        //}

        //public int AddNewTeachingAide(TeachingAide teachingAide)
        //{
        //    if (teachingAide.Items_TeachingAide.ElementAt(0).TeachingAide != null)
        //    {
        //        throw new NullReferenceException("Item member TeachingAide must be null when adding item or SQL will throw exception saying violated relationship property");
        //    }

        //    //Give dummy value for barcode
        //    if (teachingAide.Items_TeachingAide != null)
        //    {
        //        teachingAide.Items_TeachingAide.ElementAt(0).Barcode = "9999999999999";
        //    }

        //    //Check if a record already exists for this TeachingAide
        //    var oldTeachingAide = GetTeachingAideByTitle(teachingAide.Title);
        //    if (oldTeachingAide != null)
        //    {

        //        Check.ItemDuplicate(teachingAide, oldTeachingAide);

        //        //Update Items property
        //        if (teachingAide.Items_TeachingAide != null && teachingAide.Items_TeachingAide.Count == 1)
        //            oldTeachingAide.Items_TeachingAide.Add(teachingAide.Items_TeachingAide.ElementAt(0));     //Add new item to old teachingAide record
        //        else
        //            throw new ArgumentException("Can't add more than one item to a TeachingAide at the same time, or TeachingAide.Items is null");
        //    }
        //    //TeachingAide doesn't exist so add it
        //    else
        //    {
        //        Check.StringRequire(teachingAide.Title);
        //        Check.TitleLength(teachingAide.Title);
        //        Check.ItemDuplicate(teachingAide);
        //        Check.EanLength(teachingAide.EAN);
        //        Check.ImageLength(teachingAide.TeachingAideImage);
        //        _context.TeachingAides.Add(teachingAide);
        //    }
        //    _context.SaveChanges();

        //    //Set Barcode here because we need Id
        //    if (teachingAide.Items_TeachingAide != null)
        //    {
        //        var itemId = teachingAide.Items_TeachingAide.ElementAt(0).Id;
        //        var thisItem = GetItemById(itemId);
        //        thisItem.Barcode = SetItemBarcode(thisItem);


        //        _context.SaveChanges();
        //        return itemId;
        //    }
        //    return -1;
        //}

        //public int AddNewOther(Other other)
        //{
        //    if (other.Items_Other.ElementAt(0).Other != null)
        //    {
        //        throw new NullReferenceException("Item member Other must be null when adding item or SQL will throw exception saying violated relationship property");
        //    }

        //    //Give dummy value for barcode
        //    if (other.Items_Other != null)
        //    {
        //        other.Items_Other.ElementAt(0).Barcode = "9999999999999";
        //    }

        //    //Check if a record already exists for this Other
        //    var oldOther = GetOtherByTitle(other.Title);
        //    if (oldOther != null)
        //    {

        //        Check.ItemDuplicate(other, oldOther);

        //        //Update Items property
        //        if (other.Items_Other != null && other.Items_Other.Count == 1)
        //            oldOther.Items_Other.Add(other.Items_Other.ElementAt(0));     //Add new item to old other record
        //        else
        //            throw new ArgumentException("Can't add more than one item to an Other at the same time, or Audio.Items is null");
        //    }
        //    //Other doesn't exist so add it
        //    else
        //    {
        //        Check.StringRequire(other.Title);
        //        Check.TitleLength(other.Title);
        //        Check.ItemDuplicate(other);
        //        Check.EanLength(other.EAN);
        //        Check.ImageLength(other.OtherImage);
        //        _context.Others.Add(other);
        //    }
        //    _context.SaveChanges();

        //    //Set Barcode here because we need Id
        //    if (other.Items_Other != null)
        //    {
        //        var itemId = other.Items_Other.ElementAt(0).Id;
        //        var thisItem = GetItemById(itemId);
        //        thisItem.Barcode = SetItemBarcode(thisItem);


        //        _context.SaveChanges();
        //        return itemId;
        //    }
        //    return -1;
        //}

        //public int AddNewVideo(Video video)
        //{
        //    if (video.Items_Video.ElementAt(0).Video != null)
        //    {
        //        throw new NullReferenceException("Item member Video must be null when adding item or SQL will throw exception saying violated relationship property");
        //    }

        //    //Give dummy value for barcode
        //    if (video.Items_Video != null)
        //    {
        //        video.Items_Video.ElementAt(0).Barcode = "9999999999999";
        //    }

        //    //Check if a record already exists for this Video
        //    var oldVideo = GetVideoByTitle(video.Title);
        //    if (oldVideo != null)
        //    {

        //        Check.ItemDuplicate(video, oldVideo);

        //        //Update Items property
        //        if (video.Items_Video != null && video.Items_Video.Count == 1)
        //            oldVideo.Items_Video.Add(video.Items_Video.ElementAt(0));     //Add new item to old video record
        //        else
        //            throw new ArgumentException("Can't add more than one item to a Video at the same time, or Video.Items is null");
        //    }
        //    //Video doesn't exist so add it
        //    else
        //    {
        //        Check.StringRequire(video.Title);
        //        Check.TitleLength(video.Title);
        //        Check.ItemDuplicate(video);
        //        Check.EanLength(video.EAN);
        //        Check.VideoFormatLength(video.VideoFormat);
        //        Check.AudienceRatingLength(video.AudienceRating);
        //        _context.Videos.Add(video);
        //    }
        //    _context.SaveChanges();

        //    //Set Barcode here because we need Id
        //    if (video.Items_Video != null)
        //    {
        //        var itemId = video.Items_Video.ElementAt(0).Id;
        //        var thisItem = GetItemById(itemId);
        //        thisItem.Barcode = SetItemBarcode(thisItem);

        //        _context.SaveChanges();
        //        return itemId;
        //    }
        //    return -1;
        //}

        //private string SetItemBarcode(Item item)
        //{
        //    var barcode = item.ListedDate.Year.ToString().Substring(Math.Max(0, item.ListedDate.Year.ToString().Length - 2)) +
        //                     item.ListedDate.Month.ToString("D2") +
        //                     item.Consignor.Id.ToString("D4") + item.Id.ToString("D5");

        //    if (_context.Items.Any(i => i.Barcode == barcode))
        //        throw new ArgumentException("Barcode already exists in SetItemBarcode in BusinessContext");
            
        //    return barcode;
        //}

        //public void AddNewItemSaleTransaction(ItemSaleTransaction transaction)
        //{
        //    if (transaction.Items_ItemSaleTransaction.ElementAt(0).ItemSaleTransaction != null)
        //    {
        //        throw new ArgumentNullException("transaction", "ItemSaleTransaction member Transaction must be null when adding item or SQL will throw exception saying violated relationship property");
        //    }

        //    //Check if a record already exists for this Transaction
        //    var oldTransaction = GetItemSaleTransactionById(transaction.Id);
        //    if (oldTransaction != null)
        //    {
        //        //Update Items property
        //        if (transaction.Items_ItemSaleTransaction.Count == 1)
        //            oldTransaction.Items_ItemSaleTransaction.Add(transaction.Items_ItemSaleTransaction.ElementAt(0));     //Add new item to old transaction record
        //        else
        //            throw new ArgumentException("AddItemSaleTransaction - Can't add more than one item to a Transaction at the same time, or Transaction.Items is null");
        //    }
        //    //Transaction doesn't exist so add it
        //    else
        //    {
        //        Check.PriceRequire(transaction.CreditTransaction_ItemSale.StateSalesTaxTotal);
        //        Check.PriceRequire(transaction.CreditTransaction_ItemSale.LocalSalesTaxTotal);
        //        Check.PriceRequire(transaction.CreditTransaction_ItemSale.CountySalesTaxTotal);
        //        Check.PriceRequire(transaction.CreditTransaction_ItemSale.TransactionTotal);
        //        Check.DateRequire(transaction.CreditTransaction_ItemSale.TransactionDate);
        //        _context.ItemSaleTransactions.Add(transaction);
        //    }

        //    _context.SaveChanges();

        //    //Change all items' status to sold
        //    if (transaction.Items_ItemSaleTransaction != null)
        //    {
        //        foreach (var thisItem in transaction.Items_ItemSaleTransaction.Select(item => item.Id).Select(GetItemById))
        //        {
        //            thisItem.Status = "Sold";
        //        }
               
        //    }

        //    //Get TransactionId
        //    if (transaction.CreditTransaction_ItemSale != null)
        //    {
        //        transaction.CreditTransaction_ItemSale.ItemSaleTransactionId = transaction.Id;
        //    } 
            
        //    _context.SaveChanges();
        //}

        //[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        //public void AddNewConsignor(Consignor consignor)
        //{
        //    //if (person.Consignor != null)
        //    //{
        //    //    throw new ArgumentNullException("Consignor", "Person member Consignor must be null when adding item or SQL will throw exception saying violated relationship property");
        //    //}
        //    Consignor oldConsignor = null;

        //    //Check if a record already exists for this consignor
        //    if (consignor != null)
        //    {
        //        oldConsignor = GetConsignorById(consignor.Id);
        //    }
        //    if (oldConsignor != null)
        //    {
        //        Check.ConsignorPersonDuplicate(consignor, oldConsignor);
        //    }
        //    //Consignor doesn't exist so add it
        //    else
        //    {
        //        Check.StringRequire(consignor.Consignor_Person.FirstName);
        //        Check.StringRequire(consignor.Consignor_Person.LastName);
        //        Check.NameLength(consignor.Consignor_Person.FirstName);
        //        Check.NameLength(consignor.Consignor_Person.LastName);
        //        Check.DateRequire(consignor.DateAdded);
        //        Check.StringRequire(consignor.Consignor_Person.EmailAddresses.FirstOrDefault().EmailAddress);
        //        Check.EmailLength(consignor.Consignor_Person.EmailAddresses.FirstOrDefault().EmailAddress);
        //        Check.StringRequire(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().MailingAddress1);
        //        Check.AddressLength(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().MailingAddress1);
        //        Check.AddressLength(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().MailingAddress2);
        //        Check.StringRequire(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().City);
        //        Check.CityLength(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().City);
        //        Check.StringRequire(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().State);
        //        Check.StateLength(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().State);
        //        Check.StringRequire(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().ZipCode);
        //        Check.ZipLength(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().ZipCode);
        //        Check.StringRequire(consignor.Consignor_Person.PhoneNumbers.CellPhoneNumber);
        //        Check.PhoneLength(consignor.Consignor_Person.PhoneNumbers.CellPhoneNumber);
        //        Check.PhoneLength(consignor.Consignor_Person.PhoneNumbers.HomePhoneNumber);
        //        Check.PhoneLength(consignor.Consignor_Person.PhoneNumbers.AltPhoneNumber);
        //        _context.Consignors.Add(consignor);
        //    }
        //    _context.SaveChanges();

        //    //Link Consignor to Person
        //    consignor.Consignor_Person.Consignor = consignor;
        //    UpdatePerson(consignor.Consignor_Person);

        //}


        //public void AddNewConsignorPmt(ConsignorPmt consignorPmt)
        //{
        //    //if (person.Consignor != null)
        //    //{
        //    //    throw new ArgumentNullException("Consignor", "Person member Consignor must be null when adding item or SQL will throw exception saying violated relationship property");
        //    //}
        //    ConsignorPmt oldConsignorPmt = null;

        //    //Check if a record already exists for this consignor
        //    if (consignorPmt != null)
        //    {
        //        oldConsignorPmt = GetConsignorPmtById(consignorPmt.Id);
        //    }
        //    if (oldConsignorPmt != null)
        //    {
        //        Check.ConsignorPmtDuplicate(consignorPmt, oldConsignorPmt);
        //    }
        //    //Consignor doesn't exist so add it
        //    else
        //    {
        //        _context.ConsignorPmts.Add(consignorPmt);
        //    }
        //    _context.SaveChanges();

        //    //Link ConsignorPmt to Consignor
        //    //consignorPmt.Consignor_ConsignorPmt.ConsignorPmts_Consignor = consignorPmt;
        //    //UpdatePerson(consignor.Consignor_Person);

        //}
#endregion

        #region Update/Delete Methods
        
        //public int UpdateItem(Item updatedItem)
        //{
        //    var original = _context.Items.Find(updatedItem.Id);

        //    if (original == null) return -1;
        //    _context.Entry(original).CurrentValues.SetValues(updatedItem);
        //    _context.SaveChanges();
        //    return updatedItem.Id;
        //}

        //public void DeleteItemById(int id)
        //{
        //    var item = _context.Items.Find(id);
        //    if (item != null)
        //    {
        //        _context.Entry(item).State = EntityState.Deleted;
        //        _context.SaveChanges();
        //    }
        //}

        //public void DeleteTransactionById(int id)
        //{
        //    var transaction = _context.ItemSaleTransactions.Find(id);
        //    if (transaction == null) return;
        //    _context.Entry(transaction).State = EntityState.Deleted;
        //    _context.SaveChanges();
        //}

        //public void DeleteConsignerById(int id)
        //{

        //    var tempPerson = new Person();
        //    var consigner = _context.Consignors.Find(id);
        //    if (consigner != null)
        //    {
        //        tempPerson = consigner.Consignor_Person;
        //        _context.Entry(consigner).State = EntityState.Deleted;
        //        _context.SaveChanges();
        //    }
        //    var person = _context.Persons.Find(tempPerson.Id);
        //    if (person != null)
        //    {
        //        var phoneNumber = person.PhoneNumbers;
        //        _context.Entry(phoneNumber).State = EntityState.Deleted;
        //        _context.SaveChanges();
        //        _context.Entry(person).State = EntityState.Deleted;
        //        _context.SaveChanges();
        //    }
        //}

        //public int UpdateBook(Book updatedBook)
        //{
        //    var original = _context.Books.Find(updatedBook.Id);

        //    if (original != null)
        //    {
        //        _context.Entry(original).CurrentValues.SetValues(updatedBook);
        //        _context.SaveChanges();
        //        return updatedBook.Id;
        //    }

        //    return -1;
        //}

        //public int UpdateGame(Game updatedGame)
        //{

        //    var original = _context.Games.Find(updatedGame.Id);

        //    if (original != null)
        //    {
        //        _context.Entry(original).CurrentValues.SetValues(updatedGame);
        //        _context.SaveChanges();
        //        return updatedGame.Id;
        //    }

        //    return -1;
        //}

        //public int UpdateOther(Other updatedOther)
        //{

        //    var original = _context.Others.Find(updatedOther.Id);

        //    if (original != null)
        //    {
        //        _context.Entry(original).CurrentValues.SetValues(updatedOther);
        //        _context.SaveChanges();
        //        return updatedOther.Id;
        //    }

        //    return -1;
        //}

        //public int UpdateTeachingAide(TeachingAide updatedTeachingAide)
        //{

        //    var original = _context.TeachingAides.Find(updatedTeachingAide.Id);

        //    if (original != null)
        //    {
        //        _context.Entry(original).CurrentValues.SetValues(updatedTeachingAide);
        //        _context.SaveChanges();
        //        return updatedTeachingAide.Id;
        //    }

        //    return -1;
        //}

        //public int UpdateVideo(Video updatedVideo)
        //{

        //    var original = _context.Videos.Find(updatedVideo.Id);

        //    if (original != null)
        //    {
        //        _context.Entry(original).CurrentValues.SetValues(updatedVideo);
        //        _context.SaveChanges();
        //        return updatedVideo.Id;
        //    }

        //    return -1;
        //}

        //public void UpdateItemSaleTransaction(ItemSaleTransaction updatedTransaction)
        //{

        //    var original = _context.ItemSaleTransactions.Find(updatedTransaction.Id);

        //    if (original == null) return;
        //    _context.Entry(original).CurrentValues.SetValues(updatedTransaction);
        //    _context.SaveChanges();
        //}

        //public void UpdateConsignor(Consignor updatedConsignor)
        //{

        //    var original = _context.Consignors.Find(updatedConsignor.Id);

        //    if (original != null)
        //    {
        //        _context.Entry(original).CurrentValues.SetValues(updatedConsignor);
        //        _context.SaveChanges();
        //    }
        //}

        //public void UpdatePerson(Person updatedPerson)
        //{
        //    var original = _context.Persons.Find(updatedPerson.Id);

        //    if (original != null)
        //    {
        //        int i = 0;
        //        foreach (var email in original.EmailAddresses) 
        //        { 
        //            _context.Entry(email).CurrentValues.SetValues(updatedPerson.EmailAddresses.ElementAtOrDefault(i));
        //            _context.SaveChanges();
        //            i++;
        //        }
        //        i = 0;
        //        foreach (var address in original.MailingAddresses)
        //        {
        //            _context.Entry(address).CurrentValues.SetValues(updatedPerson.MailingAddresses.ElementAtOrDefault(i));
        //            _context.SaveChanges();
        //            i++;
        //        }
 
        //        _context.Entry(original.PhoneNumbers).CurrentValues.SetValues(updatedPerson.PhoneNumbers);
        //        _context.SaveChanges();
                
        //        _context.Entry(original).CurrentValues.SetValues(updatedPerson);
        //        _context.SaveChanges();
        //    }
        //}

        #endregion

        #region Queries

        //public IQueryable<Item> QueryItemsThatAreBooks()
        //{
        //    var items = _context.Items
        //                   .Where(x => x.BookId.HasValue);
        //    return items;
        //}

        //public IQueryable<Item> QueryItemsThatAreGames()
        //{
        //    var items = _context.Items
        //                   .Where(x => x.GameId.HasValue);
        //    return items;
        //}

        //public IQueryable<Item> QueryItemsThatAreOthers()
        //{
        //    var items = _context.Items
        //                   .Where(x => x.OtherId.HasValue);
        //    return items;
        //}

        //public IQueryable<Item> QueryItemsThatAreTeachingAides()
        //{
        //    var items = _context.Items
        //                   .Where(x => x.TeachingAideId.HasValue);
        //    return items;
        //}

        //public IQueryable<Item> QueryItemsThatAreVideos()
        //{
        //    var items = _context.Items
        //                   .Where(x => x.VideoId.HasValue);
        //    return items;
        //}

        //public IQueryable<Person> QueryPersonsThatAreConsigners()
        //{
        //    var persons = _context.Persons
        //                   .Where(x => x.Consignor != null);
        //    return persons;
        //}

        #endregion

        #region SQL Linq Methods
        //public List<Item> GetAllItems()
        //{
        //    var query = from i in _context.Items
        //                orderby i.ItemType
        //                select i;

        //    return query.ToList();
        //}

        //public List<Book> GetAllBooks()
        //{
        //    var query = from b in _context.Books
        //                orderby b.Title
        //                select b;

        //    return query.ToList();
        //}

        //public List<Game> GetAllGames()
        //{
        //    var query = from b in _context.Games
        //                orderby b.Title
        //                select b;

        //    return query.ToList();
        //}

        //public List<Other> GetAllOthers()
        //{
        //    var query = from b in _context.Others
        //                orderby b.Title
        //                select b;

        //    return query.ToList();
        //}

        //public List<Video> GetAllVideos()
        //{
        //    var query = from b in _context.Videos
        //                orderby b.Title
        //                select b;

        //    return query.ToList();
        //}


        //public List<Consignor> GetAllConsignors()
        //{
        //    var query = from c in _context.Consignors
        //                orderby c.Consignor_Person.LastName
        //                select c;

        //    return query.ToList();
        //}

        //public List<TeachingAide> GetAllTeachingAides()
        //{
        //    var query = from b in _context.TeachingAides
        //                orderby b.Title
        //                select b;

        //    return query.ToList();
        //}

        //public List<ItemSaleTransaction> GetAllItemSaleTransactions()
        //{
        //    var query = from t in _context.ItemSaleTransactions
        //                orderby t.CreditTransaction_ItemSale.TransactionDate  
        //                select t;

        //    return query.ToList();
        //}

        //public Item GetItemById(int id)
        //{
        //    Item item = (from s in _context.Items
        //                 where s.Id == id
        //                 select s).FirstOrDefault<Item>();

        //    return item;
        //}

        //public Book GetBookByIsbn(string isbn)
        //{
        //    var book = (from s in _context.Books
        //                where s.ISBN == isbn
        //                select s).FirstOrDefault<Book>();

        //    return book;
        //}

        //public Game GetGameByTitle(string title)
        //{
        //    var game = (from s in _context.Games
        //                where s.Title == title
        //                select s).FirstOrDefault<Game>();

        //    return game;
        //}

        //public TeachingAide GetTeachingAideByTitle(string title)
        //{
        //    var teachingAide = (from s in _context.TeachingAides
        //                        where s.Title == title
        //                        select s).FirstOrDefault<TeachingAide>();

        //    return teachingAide;
        //}

        //public Other GetOtherByTitle(string title)
        //{
        //    var other = (from s in _context.Others
        //                 where s.Title == title
        //                 select s).FirstOrDefault<Other>();

        //    return other;
        //}

        //public Video GetVideoByTitle(string title)
        //{
        //    var video = (from s in _context.Videos
        //                 where s.Title == title
        //                 select s).FirstOrDefault<Video>();

        //    return video;
        //}

        //public ItemSaleTransaction GetItemSaleTransactionById(int id)
        //{
        //    var transaction = (from t in _context.ItemSaleTransactions
        //                       where t.Id == id
        //                       select t).FirstOrDefault<ItemSaleTransaction>();

        //    return transaction;
        //}

        //public Consignor GetConsignorById(int id)
        //{
        //    Consignor consignor = (from c in _context.Consignors
        //                           where c.Id == id
        //                 select c).FirstOrDefault<Consignor>();

        //    return consignor;
        //}

        //public Consignor GetConsignorByName(string firstName, string lastName)
        //{
        //    Consignor consignor = (from c in _context.Consignors
        //                           where ((c.Consignor_Person.FirstName == firstName) &&
        //                                    (c.Consignor_Person.LastName == lastName))
        //                 select c).FirstOrDefault<Consignor>();

        //    return consignor;
        //}

        //public List<Item> GetItemsByConsignorName(string firstName, string lastName)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName))
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByBarcodeConsignorName(string barcode, string firstName, string lastName)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                         (i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                         (i.Consignor.Consignor_Person.LastName == lastName))
        //                 select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByBarcodeConsignorNameAndStatus(string barcode, string firstName, string lastName, string status)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.Status == status))
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByBarcodeConsignornameStatusListeddateItemtype(string barcode, string firstName, string lastName, string status, DateTime listedDate, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.Status == status) &&
        //                            (i.ItemType == itemType) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByBarcodeConsignornameStatusListeddate(string barcode, string firstName, string lastName, string status, DateTime listedDate)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.Status == status) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year))
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByBarcodeStatusListeddateItemtype(string barcode, string status, DateTime listedDate, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.Status == status) &&
        //                            (i.ItemType == itemType) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByBarcodeConsignornameStatusItemtype(string barcode, string firstName, string lastName, string status, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.Status == status) &&
        //                            (i.ItemType == itemType)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByBarcodeStatusItemtype(string barcode, string status, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.Status == status) &&
        //                            (i.ItemType == itemType)) 
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByBarcodeConsignornameListeddateItemtype(string barcode, string firstName, string lastName, DateTime listedDate, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.ItemType == itemType) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByBarcodeConsignornameListeddate(string barcode, string firstName, string lastName, DateTime listedDate)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByBarcodeListeddateItemtype(string barcode, DateTime listedDate, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.ItemType == itemType) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByBarcodeListeddate(string barcode, DateTime listedDate)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByBarcodeConsignornameItemtype(string barcode, string firstName, string lastName, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.ItemType == itemType)) 
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByBarcodeStatusListeddate(string barcode, string status, DateTime listedDate)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                            (i.Status == status) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByBarcodeStatus(string barcode, string status)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                         (i.Status == status))
        //                 select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByBarcodeItemType(string barcode, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())) &&
        //                         (i.ItemType == itemType))
        //                 select i);
        //    return query.ToList();
        //}
        
        //public List<Item> GetItemsByConsignorNameAndStatus(string firstName, string lastName, string status)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.Status == status))
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByConsignornameStatusListeddateItemtype(string firstName, string lastName, string status, DateTime listedDate, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.Status == status) &&
        //                            (i.ItemType == itemType) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByConsignornameStatusListeddate(string firstName, string lastName, string status, DateTime listedDate)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.Status == status) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year))
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByStatusListeddateItemtype(string status, DateTime listedDate, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Status == status) &&
        //                            (i.ItemType == itemType) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByConsignornameStatusItemtype(string firstName, string lastName, string status, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.Status == status) &&
        //                            (i.ItemType == itemType)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByStatusItemtype(string status, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Status == status) &&
        //                            (i.ItemType == itemType)) 
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByConsignornameListeddateItemtype(string firstName, string lastName, DateTime listedDate, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.ItemType == itemType) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByConsignornameListeddate(string firstName, string lastName, DateTime listedDate)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByListeddateItemtype(DateTime listedDate, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.ItemType == itemType) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByListeddate(DateTime listedDate)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByConsignornameItemtype(string firstName, string lastName, string itemType)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Consignor.Consignor_Person.FirstName == firstName) &&
        //                            (i.Consignor.Consignor_Person.LastName == lastName) &&
        //                            (i.ItemType == itemType)) 
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByStatusListeddate(string status, DateTime listedDate)
        //{
        //    var query = (from i in _context.Items
        //                    where ((i.Status == status) &&
        //                            (i.ListedDate.Day == listedDate.Day) &&
        //                            (i.ListedDate.Month == listedDate.Month) &&
        //                            (i.ListedDate.Year == listedDate.Year)) 
        //                    select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByStatus(string status)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Status == status))
        //                 select i);
        //    return query.ToList();
        //}


        //public List<Item> GetItemsByItemType(string itemType)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.ItemType == itemType))
        //                 select i);
        //    return query.ToList();
        //}

        //public List<Item> GetItemsByPartOfBarcode(string barcode)
        //{
        //    var query = (from i in _context.Items
        //                 where ((i.Barcode.Contains(barcode.Trim())))
        //                 select i);
        //    return query.ToList();
        //}

        //public List<string> GetConsignorNames()
        //{
        //    using (var bc = new BusinessContext())
        //    {
        //        var consignors = bc.QueryPersonsThatAreConsigners().ToList();
        //        return consignors.Select(person => person.FirstName + " " + person.LastName).ToList();
        //    }
        //}

        //public List<string> GetConsignorNamesLastnameFirst()
        //{
        //    using (var bc = new BusinessContext())
        //    {
        //        var consignors = bc.QueryPersonsThatAreConsigners().ToList();
        //        return consignors.Select(person => person.LastName + " " + person.FirstName).ToList();
        //    }
        //}

        //public Consignor GetConsignorByFullName(string fullName)
        //{
        //    var names = fullName.Split(' ');
        //    var firstName = names[0];
        //    var lastName = names[1];
        //    var consignor = GetConsignorByName(firstName, lastName);

        //    return consignor;
        //}

        //public ConsignorPmt GetConsignorPmtById(int id)
        //{
        //    ConsignorPmt consignorPmt = (from c in _context.ConsignorPmts
        //                                 where c.Id == id
        //                           select c).FirstOrDefault<ConsignorPmt>();

        //    return consignorPmt;
        //}

        //public IEnumerable<ConsignorPmt> GetConsignorPmtByConsignorId(int id)
        //{
        //    var consignorPmts = (from c in _context.ConsignorPmts
        //                                 where c.ConsignorId == id
        //                                 select c);

        //    return consignorPmts;
        //}

        //public IEnumerable<StoreCreditTransaction> GetStoreCreditTransactionsByConsignorId(int id)
        //{
        //    var storeCreditTrans = (from c in _context.StoreCreditTransactions
        //                         where c.ConsignorId == id
        //                         select c);

        //    return storeCreditTrans;
        //}

        //public IEnumerable<StoreCreditPmt> GetStoreCreditPmtsByConsignorId(int id)
        //{
        //    var storeCreditPmts = (from c in _context.StoreCreditPmts
        //                            where c.ConsignorId == id
        //                            select c);

        //    return storeCreditPmts;
        //}

        //public double GetConsignorCreditBalance(string name)
        //{
        //    //Split full name
        //    if (String.IsNullOrEmpty(name)) return 0;
        //    var names = name.Split(' ');
        //    var lastName = names[0];
        //    var firstName = names[1];

        //    //Find consignor
        //    var consignor = GetConsignorByName(firstName, lastName);

        //    //Sum StoreCreditPmts which are store credit then subtract the sum of StoreCreditTransactions
        //    var pmts = GetStoreCreditPmtsByConsignorId(consignor.Id); 
        //    if(pmts == null) return 0;

        //    var purchases = GetStoreCreditTransactionsByConsignorId(consignor.Id);
        //    if (purchases == null) return pmts.Sum(f => f.StoreCreditPmtAmount);

        //    return pmts.Sum(f => f.StoreCreditPmtAmount) -
        //                purchases.Sum(f => f.StoreCreditTransactionAmount);
        //}

        #endregion

        //static class Check
        //{
        //    public static void StringRequire(string value)
        //    {
        //        if (value == null)
        //            throw new NullReferenceException("Item string member must not be null");
        //        if (value.Trim().Length == 0)
        //            throw new ArgumentException("Item string member must not be empty or null");
        //    }

        //    public static void DateRequire(DateTime value)
        //    {
        //        if (value == null)
        //            throw new NullReferenceException("DateTime must not be null");
        //        if (value.ToString().Trim().Length == 0)
        //            throw new ArgumentException("DateTime member must not be empty or null");
        //    }
            

        //    public static void PriceRequire(double value)
        //    {
        //        if (value <= 0)
        //            throw new NullReferenceException("price member must be greater than zero");

        //        if (Double.IsNaN(value))
        //            throw new ArgumentException("Price member must be a number");

        //    }

        //    //public static void BoolRequire(bool value)
        //    //{
        //    //    if (value == null)
        //    //        throw new ArgumentNullException("Book", "Book boolean member must not be null");
        //    //}

        //    internal static void NameLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 20)
        //                value.Remove(20);
        //    }

        //    internal static void EmailLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 40)
        //                value.Remove(40);
        //    }

        //    internal static void AddressLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 40)
        //                value.Remove(40);
        //    }

        //    internal static void CityLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 30)
        //                value.Remove(30);
        //    }

        //    internal static void StateLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 2)
        //                value.Remove(2);
        //    }

        //    internal static void ZipLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 10)
        //                value.Remove(10);
        //    }

        //    internal static void PhoneLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 10)
        //                value.Remove(10);
        //    }

        //    internal static void TitleLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 150)
        //                value.Remove(150);
        //    }

        //    internal static void AuthorLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 100)
        //                value.Remove(100);
        //    }

        //    internal static void ImageLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 300)
        //                throw new ArgumentException("Image URL is longer than 200 characters");
        //    }

        //    internal static void IsbnLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 13)
        //                throw new ArgumentException("ISBN in Book object must be 13 characters or less");
        //    }

        //    internal static void EanLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 13)
        //                throw new ArgumentException("EAN in object must be 13 characters or less");
        //    }


        //    internal static void VideoFormatLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 7)
        //                throw new ArgumentException("VideoFormat in video must be 7 characters or less");
        //    }

        //    internal static void AudienceRatingLength(string value)
        //    {
        //        if (value != null)
        //            if (value.Length > 40)
        //                throw new ArgumentException("AudienceRating in video must be 20 characters or less");
        //    }


        //    internal static void BooksDuplicate(List<Book> books, Book book)
        //    {

        //        //Check for duplicate ISBN
        //        if (books.Any(b => b.ISBN == book.ISBN))
        //            throw new ArgumentException("ISBN in Book object already exists");
        //    }
            
        //    #region ItemDuplicate Methods
        //    internal static void ItemDuplicate(Book book)
        //    {
        //        if (book.Items_Books != null)
        //            if (book.Items_Books.Count > 1)
        //                throw new ArgumentException("Cannot add more than one Item to book at a time");
        //            else
        //            {
        //                if (book.Items_Books.Count != book.Items_Books.Distinct().Count())
        //                    throw new ArgumentException("Book already contains specified Id, cannot add duplicate Id to Book");
        //            }
        //    }

        //    internal static void ItemDuplicate(Book book, Book oldBook)
        //    {
        //        if (book.Items_Books != null)
        //        {
        //            if (book.Items_Books.Count > 1)
        //                throw new ArgumentException("Cannot add more than one Item to book at a time");
        //            else
        //            {
        //                if (book.Items_Books.Count != book.Items_Books.Distinct().Count())
        //                    throw new ArgumentException("Book already contains specified Id, cannot add duplicate Id to Book");
        //                if (oldBook.Items_Books.Any(b => b.Id == book.Items_Books.ElementAt(0).Id))
        //                    throw new ArgumentException("A Book exists that already contains specified Id, cannot add duplicate Id to Book");
        //            }
        //        }
        //    }

        //    internal static void ItemDuplicate(Game game)
        //    {
        //        if (game.Items_Games != null)
        //            if (game.Items_Games.Count > 1)
        //                throw new ArgumentException("Cannot add more than one Item to game at a time");
        //            else
        //            {
        //                if (game.Items_Games.Count != game.Items_Games.Distinct().Count())
        //                    throw new ArgumentException("Game already contains specified Id, cannot add duplicate Id to Game");
        //            }
        //    }

        //    internal static void ItemDuplicate(Game game, Game oldGame)
        //    {
        //        if (game.Items_Games != null)
        //        {
        //            if (game.Items_Games.Count > 1)
        //                throw new ArgumentException("Cannot add more than one Item to game at a time");
        //            else
        //            {
        //                if (game.Items_Games.Count != game.Items_Games.Distinct().Count())
        //                    throw new ArgumentException("Game already contains specified Id, cannot add duplicate Id to Game");
        //                if (oldGame.Items_Games.Any(b =>
        //                {
        //                    var firstOrDefault = game.Items_Games.FirstOrDefault();
        //                    return firstOrDefault != null && b.Id == firstOrDefault.Id;
        //                }))
        //                    throw new ArgumentException("A Game exists that already contains specified Id, cannot add duplicate Id to Game");
        //            }
        //        }
        //    }

        //    internal static void ItemDuplicate(TeachingAide teachingAide)
        //    {
        //        if (teachingAide.Items_TeachingAide != null)
        //            if (teachingAide.Items_TeachingAide.Count > 1)
        //                throw new ArgumentException("Cannot add more than one Item to teachingAide at a time");
        //            else
        //            {
        //                if (teachingAide.Items_TeachingAide.Count != teachingAide.Items_TeachingAide.Distinct().Count())
        //                    throw new ArgumentException("Game already contains specified Id, cannot add duplicate Id to TeachingAide");
        //            }
        //    }

        //    internal static void ItemDuplicate(TeachingAide teachingAide, TeachingAide oldTeachingAide)
        //    {
        //        if (teachingAide.Items_TeachingAide != null)
        //        {
        //            if (teachingAide.Items_TeachingAide.Count > 1)
        //                throw new ArgumentException("Cannot add more than one Item to teachingAide at a time");
        //            if (teachingAide.Items_TeachingAide.Count != teachingAide.Items_TeachingAide.Distinct().Count())
        //                throw new ArgumentException("Game already contains specified Id, cannot add duplicate Id to TeachingAide");
        //            if (oldTeachingAide.Items_TeachingAide.Any(b =>
        //            {
        //                var firstOrDefault = teachingAide.Items_TeachingAide.FirstOrDefault();
        //                return firstOrDefault != null && b.Id == firstOrDefault.Id;
        //            }))
        //                throw new ArgumentException("A TeachingAide exists that already contains specified Id, cannot add duplicate Id to TeachingAide");
        //        }
        //    }

        //    internal static void ItemDuplicate(Other other)
        //    {
        //        if (other.Items_Other != null)
        //            if (other.Items_Other.Count > 1)
        //                throw new ArgumentException("Cannot add more than one Item to other at a time");
        //            else
        //            {
        //                if (other.Items_Other.Count != other.Items_Other.Distinct().Count())
        //                    throw new ArgumentException("Other already contains specified Id, cannot add duplicate Id to Other");
        //            }
        //    }

        //    internal static void ItemDuplicate(Other other, Other oldOther)
        //    {
        //        if (other.Items_Other != null)
        //        {
        //            if (other.Items_Other.Count > 1)
        //                throw new ArgumentException("Cannot add more than one Item to other at a time");
        //            else
        //            {
        //                if (other.Items_Other.Count != other.Items_Other.Distinct().Count())
        //                    throw new ArgumentException("Other already contains specified Id, cannot add duplicate Id to Other");
        //                if (oldOther.Items_Other.Any(b =>
        //                {
        //                    var firstOrDefault = other.Items_Other.FirstOrDefault();
        //                    return firstOrDefault != null && b.Id == firstOrDefault.Id;
        //                }))
        //                    throw new ArgumentException("A Other exists that already contains specified Id, cannot add duplicate Id to Other");
        //            }
        //        }
        //    }

        //    internal static void ItemDuplicate(Video video)
        //    {
        //        if (video.Items_Video != null)
        //            if (video.Items_Video.Count > 1)
        //                throw new ArgumentException("Cannot add more than one Item to video at a time");
        //            else
        //            {
        //                if (video.Items_Video.Count != video.Items_Video.Distinct().Count())
        //                    throw new ArgumentException("Video already contains specified Id, cannot add duplicate Id to Video");
        //            }
        //    }

        //    internal static void ItemDuplicate(Video video, Video oldVideo)
        //    {
        //        if (video.Items_Video != null)
        //        {
        //            if (video.Items_Video.Count > 1)
        //                throw new ArgumentException("Cannot add more than one Item to video at a time");
        //            if (video.Items_Video.Count != video.Items_Video.Distinct().Count())
        //                throw new ArgumentException("Video already contains specified Id, cannot add duplicate Id to Video");
        //            if (oldVideo.Items_Video.Any(b =>
        //            {
        //                var firstOrDefault = video.Items_Video.FirstOrDefault();
        //                return firstOrDefault != null && b.Id == firstOrDefault.Id;
        //            }))
        //                throw new ArgumentException("A Video exists that already contains specified Id, cannot add duplicate Id to Video");
        //        }
        //    } 
        //    # endregion

        //    internal static void ConsignorPersonDuplicate(Consignor consignor, Consignor oldConsignor)
        //    {
        //        if (consignor.Consignor_Person != null)
        //        {
        //            if (oldConsignor.Consignor_Person != null)
        //                throw new ArgumentException("Consignor already contains Id, cannot change Id of Consignor");
        //            if (oldConsignor.Consignor_Person != null && oldConsignor.Consignor_Person.Id == consignor.Consignor_Person.Id)
        //                throw new ArgumentException("A Consignor exists that already contains specified Id, cannot add duplicate Id to Consignor");
                    
        //        }
        //    }


        //    internal static void ConsignorPmtDuplicate(ConsignorPmt consignorPmt, ConsignorPmt oldConsignorPmt)
        //    {
        //        if (consignorPmt.Items_ConsignorPmt.Count > 1)
        //            throw new ArgumentException("Cannot add more than one Item to ConsignorPmt at a time");
        //        else
        //        {
        //            if (consignorPmt.Items_ConsignorPmt.Count != consignorPmt.Items_ConsignorPmt.Distinct().Count())
        //                throw new ArgumentException("ConsignorPmt already contains specified Id, cannot add duplicate Id to ConsignorPmt");
        //            if (oldConsignorPmt.Items_ConsignorPmt.Any(b =>
        //            {
        //                var firstOrDefault = consignorPmt.Items_ConsignorPmt.FirstOrDefault();
        //                return firstOrDefault != null && b.Id == firstOrDefault.Id;
        //            }))
        //                throw new ArgumentException("A ConsignorPmt exists that already contains specified Id, cannot add duplicate Id to ConsignorPmt");
        //        }

        //        if (consignorPmt.Consignor_ConsignorPmt != null)
        //        {
        //            if (oldConsignorPmt.Consignor_ConsignorPmt != null)
        //                throw new ArgumentException("ConsignorPmt already contains Consignor, cannot change Id of Consignor");
        //            if (oldConsignorPmt.Consignor_ConsignorPmt != null && oldConsignorPmt.Consignor_ConsignorPmt.Id == consignorPmt.Consignor_ConsignorPmt.Id)
        //                throw new ArgumentException("A ConsignorPmt exists that already contains specified Id, cannot add duplicate Id to Consignor");
        //        }

        //        if (consignorPmt.DebitTransaction_ConsignorPmt != null)
        //        {
        //            if (oldConsignorPmt.DebitTransaction_ConsignorPmt != null)
        //                throw new ArgumentException("ConsignorPmt already contains DebitTransaction");
        //            if (oldConsignorPmt.DebitTransaction_ConsignorPmt != null && oldConsignorPmt.DebitTransaction_ConsignorPmt.Id == consignorPmt.DebitTransaction_ConsignorPmt.Id)
        //                throw new ArgumentException("A ConsignorPmt exists that already contains specified Id, cannot add duplicate Id to ConsignorPmt");
        //        }
        //    } 
        //}
           

        public DataContext DataContext
        {
            get { return _context; }
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            if (_context != null)
                _context.Dispose();

            _disposed = true;
        }
        #endregion


    }
}
