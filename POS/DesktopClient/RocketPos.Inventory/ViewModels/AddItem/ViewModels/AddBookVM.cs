using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Castle.Core.Internal;
using DataModel.Data.DataLayer.Entities;
using GalaSoft.MvvmLight.Messaging;
using Inventory.Controller.CustomClasses;
using Inventory.Controller.Elements.ItemElements;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;

namespace Inventory.ViewModels.AddItem.ViewModels
{
    public class AddBookVm : ViewModel
    {
        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance
        
        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application

        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _bindingComboValues = new ComboBoxListValues();

        public AddBookVm()
        {
            Books = new ObservableCollection<Book>();

            _bookItem = new BookItem();

            IsAmazonEnabled = true;
            
            _bindingComboValues.InitializeComboBox(EnumsAndLists.Bindings);
        }

        /// <summary>
        /// Gets the collection of Book entities.
        /// </summary>
        public ICollection<Book> Books { get; private set; }
        
        /// <summary>
        /// Gets or sets the ISBN value.
        /// </summary>
        private string _isbn;
        [Required]
        [StringLength(13, MinimumLength = 10)]
        public string Isbn 
        { 
            get
            {
                return _isbn;
            }        
            set
            {
                _isbn = value;
                
                //Let everyone else know ISBN has changed
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("ISBN", Isbn), Token);
            }   
        }

        /// <summary>
        /// Gets the bindingComboValues list.
        /// </summary>
        public List<ComboBoxListValues> BindingComboValues
        {
            get { return _bindingComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets or sets the Title value.
        /// </summary>
        private string _title;
        [Required]
        [MaxLength(150)]
        public string Title 
        { 
            get
            {
                return _title;    
            } 
            set
            { 
                _title = value; 
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("Title", Title), Token);
            }
        }


        /// <summary>
        /// Gets or sets the Author value.
        /// </summary>
        
        private string _author;
        [MaxLength(100)]
        public string Author 
        { 
            get
            {
                return _author;
            }
            set
            {
                _author = value;
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("Author", Author), Token);
            }
        }

        private string _bookImage;
        public string BookImage
        {
            get
            { 
                return _bookImage;
            }
            set
            {
                _bookImage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Binding value.
        /// </summary>
        private string _binding;
        public string Binding
        {
            get
            {
                return _binding;
            }
            set
            {
                _binding = value;
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("Binding", Binding), Token);
            }
        }
        
        /// <summary>
        /// Gets or sets the NumberOfPages value.
        /// </summary>
        private int _numberOfPages;
        public int NumberOfPages
        {
            get
            {
                return _numberOfPages;
            }
            set
            {
                _numberOfPages = value;
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("NumberOfPages", NumberOfPages), Token);
            }
        }

        /// <summary>
        /// Gets or sets the PublicationDate value.
        /// </summary>
        private string _publicationDate;
        public string PublicationDate
        {
            get
            {
                return _publicationDate;
            }
            set
            {
                _publicationDate = value;
                OnPropertyChanged();
                Messenger.Default.Send(new PropertySetter("PublicationDate", PublicationDate), Token);
            }
        }

        /// <summary>
        /// Gets or sets the TradeInValue value.
        /// </summary>
        private double _tradeInValue;
        public double TradeInValue
        {
            get
            {
                return _tradeInValue;
            }
            set
            {
                _tradeInValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the BookItem value.
        /// </summary>
        private BookItem _bookItem;
        public BookItem BookItem
        {
            get
            {
                return _bookItem;
            }
            set
            {
                _bookItem = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the LowestNewPrice value.
        /// </summary>
        private double _lowestNewPrice;
        public double LowestNewPrice
        {
            get
            {
                return _lowestNewPrice;
            }
            set
            {
                _lowestNewPrice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the LowestUsedPrice value.
        /// </summary>
        private double _lowestUsedPrice;
        public double LowestUsedPrice
        {
            get
            {
                return _lowestUsedPrice;
            }
            set
            {
                _lowestUsedPrice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the IsAmazonEnabled checkbox value.
        /// </summary>
        private bool _isAmazonEnabled;
        public bool IsAmazonEnabled
        {
            get
            {
                return _isAmazonEnabled;
            }
            set
            {
                _isAmazonEnabled = value;
                OnPropertyChanged();
            }
        }

        //Does ItemSearch - returns many results
        private IEnumerable SearchAmazonByIsbn(string isbn)
        {
            try
            {
                var search = SearchAmazon.SearchAmazonByIsbn(isbn);

                if (search != null)
                {
                    var selectedIndex = search.Isbn.FindIndex(g => g == Isbn);
                    if (selectedIndex >= 0)
                    {
                        if (!search.Title.IsNullOrEmpty()) Title = search.Title[selectedIndex];
                        if (!search.Author.IsNullOrEmpty()) Author = search.Author[selectedIndex];
                        if (!search.Image.IsNullOrEmpty()) BookImage = search.Image[selectedIndex].URL;
                        if (!search.Binding.IsNullOrEmpty()) Binding = search.Binding[selectedIndex];
                        if (!search.NumberOfPages.IsNullOrEmpty()) NumberOfPages = search.NumberOfPages[selectedIndex];
                        if (!search.PublicationDate.IsNullOrEmpty()) PublicationDate = search.PublicationDate[selectedIndex].ToShortDateString();
                        if (!search.TradeInValue.IsNullOrEmpty()) TradeInValue = search.TradeInValue[selectedIndex];

                        //Get Lowest Price Info
                        BookItem = (BookItem)SearchAmazon.QueryAmazonByIsbn(BookItem, Isbn);
                        if (BookItem != null)
                        {
                            LowestNewPrice = BookItem.LowestNewPrice;
                            LowestUsedPrice = BookItem.LowestUsedPrice;
                        }

                        //Send notice to AddItemVM
                        Messenger.Default.Send(new PropertySetter("Author", Author), Token);
                        Messenger.Default.Send(new PropertySetter("Title", Title), Token);
                        Messenger.Default.Send(new PropertySetter("BookImage", BookImage), Token);
                        Messenger.Default.Send(new PropertySetter("ISBN", Isbn), Token);
                        Messenger.Default.Send(new PropertySetter("Binding", Binding), Token);
                        Messenger.Default.Send(new PropertySetter("NumberOfPages", NumberOfPages), Token);
                        Messenger.Default.Send(new PropertySetter("PublicationDate", PublicationDate), Token);
                        Messenger.Default.Send(new PropertySetter("TradeInValue", TradeInValue), Token);
                    }
                    else
                    {
                        Author = null;
                        Title = null;
                        BookImage = null;
                        Binding = null;
                        NumberOfPages = 0;
                        PublicationDate = null;
                        TradeInValue = 0;
                        //Send message to change property so that addItem works correctly
                        Messenger.Default.Send(new PropertySetter("Author", null), Token);
                        Messenger.Default.Send(new PropertySetter("Title", null), Token);
                        Messenger.Default.Send(new PropertySetter("BookImage", null), Token);
                    }

                    return search.Isbn;
                    }
                    Author = null;
                    Title = null;
                    BookImage = null;
                    //Send message to change property so that addItem works correctly
                    Messenger.Default.Send(new PropertySetter("Author", null), Token);
                    Messenger.Default.Send(new PropertySetter("Title", null), Token);
                    Messenger.Default.Send(new PropertySetter("BookImage", null), Token);                   
                
                return null;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BookSearchAmazonByISBNException");
                return null;
            }
        }

        #region AutoComplete TextBox
        private readonly List<string> _waitMessage = new List<string>() { "Please Wait..." };
        public IEnumerable WaitMessage { get { return _waitMessage; } }

        private string _isbnQueryText;
        public string IsbnQueryText
        {
            get { return _isbnQueryText; }
            set
            {
                if (_isbnQueryText == value) 
                    return;
                _isbnQueryText = value;
                OnPropertyChanged();
                Isbn = value;
                
                //If ISBN is at least 10 characters search amazon 
                //  and change title and author fields if found
                if (!string.IsNullOrEmpty(_isbnQueryText) && IsAmazonEnabled)
                    QueryCollection = SearchAmazonByIsbn(_isbn);
            }
        }

        private IEnumerable _queryCollection;
        public IEnumerable QueryCollection
        {
            get
            {
                //QueryGoogle(IsbnQueryText);
                return _queryCollection ?? (_queryCollection = new List<string>());
            }
            set
            {

                _queryCollection = value; 
                OnPropertyChanged();
            }
        }


        //Example of how to query google
        //private void QueryGoogle(string SearchTerm)
        //{
        //    try
        //    {
        //        if (SearchTerm != null)
        //        {
        //            string sanitized = HttpUtility.HtmlEncode(SearchTerm);
        //            string url = @"http://google.com/complete/search?output=toolbar&q=" + sanitized;
        //            WebRequest httpWebRequest = HttpWebRequest.Create(url);
        //            var webResponse = httpWebRequest.GetResponse();

        //            XmlDocument xmlDoc = new XmlDocument();
        //            xmlDoc.Load(webResponse.GetResponseStream());
        //            var result = xmlDoc.SelectNodes("//CompleteSuggestion");
        //            QueryCollection = result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("QueryGoogleException", ex.Message);
        //        return;
        //    }

        //}
        #endregion
    }
}
