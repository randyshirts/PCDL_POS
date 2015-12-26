// -----------------------------------------------------------------------------
//  <copyright file="CustomerViewModel.cs" company="DCOM Engineering, LLC">
//      Copyright (c) DCOM Engineering, LLC.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Windows;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using GalaSoft.MvvmLight.Messaging;
using Inventory.Controller.Elements;
using Inventory.Controller.Elements.ItemElements;
using Inventory.Controller.Visitors.ItemVisitors;
using Inventory.ViewModels.MainView.ViewModels;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;

namespace Inventory.ViewModels.AddItem.ViewModels
{
    //using Google.Apis.Books.v1;

    /// <summary>
    /// A View-Model that represents a customer and its state information.
    /// </summary>
    public class AddItemVm : ViewModel
    {
        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application

        readonly ViewModel _myAddItemBarcodesVm;

        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _itemTypeComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _subjectComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _statusComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _conditionComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _consignorNameComboValues = new ComboBoxListValues();

        public AddItemVm()
        {
            Items = new ObservableCollection<Item>();
            Books = new ObservableCollection<Book>();
            Barcodes = new TrulyObservableCollection<BarcodeItem>();

            BookItem = new BookItem();
            GameItem = new GameItem();
            OtherItem = new OtherItem();
            VideoItem = new VideoItem();
            TeachingAideItem = new TeachingAideItem();

            IsBarcodesOpened = true;
            

            //Disable fields on view
            ItemTypeIsNotNull = false;

            ItemIsDiscountable = true;

            //Initialize ComboBoxes
            _itemTypeComboValues.InitializeComboBox(EnumsAndLists.ItemTypes);
            _subjectComboValues.InitializeComboBox(EnumsAndLists.Subjects);
            List<string> statusExceptList = new List<string>
            {
                "Sold",
                "Paid",
                "Pending"
            };
            _statusComboValues.InitializeComboBox(EnumsAndLists.Statuses.Except(statusExceptList).ToList());
            _conditionComboValues.InitializeComboBox(EnumsAndLists.Conditions);
            
            var consignor = new ConsignorController();

            _consignorNameComboValues.InitializeComboBox(consignor.GetConsignorNames());

            //Initialize locator for switching views
            Locator = new ViewModelLocator();
            _myAddItemBarcodesVm = Locator.AddItemBarcodesView;

            //Register for messages
            Messenger.Default.Register<PropertySetter>(this, AddGameVm.Token, msg => SetGameProperty(msg.PropertyName, (string)msg.PropertyValue));
            Messenger.Default.Register<PropertySetter>(this, AddBookVm.Token, msg => SetBookProperty(msg.PropertyName, msg.PropertyValue));
            Messenger.Default.Register<PropertySetter>(this, AddOtherVm.Token, msg => SetOtherProperty(msg.PropertyName, (string)msg.PropertyValue));
            Messenger.Default.Register<PropertySetter>(this, AddVideoVm.Token, msg => SetVideoProperty(msg.PropertyName, (string)msg.PropertyValue));
            Messenger.Default.Register<PropertySetter>(this, AddTeachingAideVm.Token, msg => SetTeachingAideProperty(msg.PropertyName, (string)msg.PropertyValue));


        }

        private ViewModelLocator Locator { get; set; }

        /// <summary>
        /// Sets a property of myAddGame view model.
        /// </summary>
        private void SetGameProperty(string propertyName, string propertyValue)
        {
            //Find the property
            if (propertyName == "Title")
            {
                GameItem.Title = propertyValue;
            }
            if (propertyName == "Manufacturer")
            {
                GameItem.Manufacturer = propertyValue;
            }
            if (propertyName == "GameImage")
            {
                GameItem.GameImage = propertyValue;
            }
            if (propertyName == "EAN")
            {
                GameItem.Ean = propertyValue;
            }
        }

        /// <summary>
        /// Sets a property of myAddOther view model.
        /// </summary>
        private void SetOtherProperty(string propertyName, string propertyValue)
        {
            //Find the property
            if (propertyName == "Title")
            {
                OtherItem.Title = propertyValue;
            }
            if (propertyName == "Manufacturer")
            {
                OtherItem.Manufacturer = propertyValue;
            }
            if (propertyName == "OtherImage")
            {
                OtherItem.OtherImage = propertyValue;
            }
            if (propertyName == "EAN")
            {
                OtherItem.Ean = propertyValue;
            }
        }

        /// <summary>
        /// Sets a property of myAddVideo view model.
        /// </summary>
        private void SetVideoProperty(string propertyName, string propertyValue)
        {
            //Find the property
            if (propertyName == "Title")
            {
                VideoItem.Title = propertyValue;
            }
            if (propertyName == "Publisher")
            {
                VideoItem.Publisher = propertyValue;
            }
            if (propertyName == "VideoImage")
            {
                VideoItem.VideoImage = propertyValue;
            }
            if (propertyName == "EAN")
            {
                VideoItem.Ean = propertyValue;
            }
            if (propertyName == "VideoFormat")
            {
                VideoItem.VideoFormat = propertyValue;
            }
            if (propertyName == "Rating")
            {
                VideoItem.AudienceRating = propertyValue;
            }
        }

        /// <summary>
        /// Sets a property of myAddTeachingAide view model.
        /// </summary>
        private void SetTeachingAideProperty(string propertyName, string propertyValue)
        {
            //Find the property
            if (propertyName == "Title")
            {
                TeachingAideItem.Title = propertyValue;
            }
            if (propertyName == "Manufacturer")
            {
                TeachingAideItem.Manufacturer = propertyValue;
            }
            if (propertyName == "TeachingAideImage")
            {
                TeachingAideItem.TeachingAideImage = propertyValue;
            }
            if (propertyName == "EAN")
            {
                TeachingAideItem.Ean = propertyValue;
            }
        }


        /// <summary>
        /// Sets a property of myAddBook view model.
        /// </summary>
        private void SetBookProperty(string propertyName, object propertyValue)
        {
            //Find the property
            if (propertyName == "Title")
            {
                BookItem.Title = propertyValue != null ? propertyValue.ToString() : null;
            }
            if (propertyName == "Author")
            {
                BookItem.Author = propertyValue != null ? propertyValue.ToString() : null;
            }
            if (propertyName == "BookImage")
            {
                BookItem.BookImage = propertyValue != null ? propertyValue.ToString() : null;
            }
            if (propertyName == "ISBN")
            {
                BookItem.Isbn = propertyValue != null ? propertyValue.ToString() : null;
            }
            if (propertyName == "Binding")
            {
                BookItem.Binding = propertyValue != null ? propertyValue.ToString() : null;
                if (BookItem.Binding == "Paperback")
                    BookItem.Binding = "Softcover";
            }
            if (propertyName == "NumberOfPages")
            {
                if (propertyValue != null)
                    BookItem.NumberOfPages = (int)propertyValue;
            }
            if (propertyName == "PublicationDate")
            {
                if (propertyValue != null) BookItem.PublicationDate = DateTime.Parse(propertyValue.ToString());
            }
            if (propertyName == "TradeInValue")
            {
                if (propertyValue != null) BookItem.TradeInValue = (double)propertyValue;
            }
        }

        private GameItem GameItem { get; set; }
        private OtherItem OtherItem { get; set; }
        private VideoItem VideoItem { get; set; }
        private TeachingAideItem TeachingAideItem { get; set; }
        private BookItem BookItem { get; set; }

        public ActionCommand WindowLoaded
        {
            get
            {
                return new ActionCommand(p => LoadWindow());
            }
        }

        private void LoadWindow()
        {
            //Load ConsignorNames
            var controller = new ConsignorController();
            _consignorNameComboValues.InitializeComboBox(controller.GetConsignorNames());

            OpenBarcodeView();
            
        }

        /// <summary>
        /// Gets the itemTypeComboValues list.
        /// </summary>
        public List<ComboBoxListValues> SubjectComboValues
        {
            get { return _subjectComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the itemTypeComboValues list.
        /// </summary>
        public List<ComboBoxListValues> StatusComboValues
        {
            get { return _statusComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the itemTypeComboValues list.
        /// </summary>
        public List<ComboBoxListValues> ConditionComboValues
        {
            get { return _conditionComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the itemTypeComboValues list.
        /// </summary>
        public List<ComboBoxListValues> ConsignorNameComboValues
        {
            get { return _consignorNameComboValues.ComboValues; }
        }



        /// <summary>
        /// Gets the collection of Item entities.
        /// </summary>
        public ICollection<Item> Items { get; private set; }

        /// <summary>
        /// Gets the collection of Book entities.
        /// </summary>
        public ICollection<Book> Books { get; private set; }

        /// <summary>
        /// Gets the collection of BarcodeItem entities.
        /// </summary>
        public ICollection<BarcodeItem> Barcodes { get; private set; }

        /// <summary>
        /// Gets or sets the ItemType value.
        /// </summary>
        private string _itemType;
        [Required]
        public string ItemType
        {
            get { return _itemType; }
            set
            {
                _itemType = value;
                ItemTypeIsNotNull = !String.IsNullOrEmpty(_itemType);

                switch (value)
                {
                    case "Book":
                        {
                            //Send message to display AddBookView 
                            var myAddBookVm = Locator.AddBookView;
                            Messenger.Default.Send(new SwitchView(myAddBookVm), Token);
                            break;
                        }
                    case "Game":
                        {
                            Subject = "Games";

                            //Send message to display AddBookView 
                            var myAddGameVm = Locator.AddGameView;
                            Messenger.Default.Send(new SwitchView(myAddGameVm), Token);
                            break;
                        }
                    case "Video":
                        {
                            Subject = "Videos";

                            //Send message to display AddVideoView 
                            var myAddVideoVm = Locator.AddVideoView;
                            Messenger.Default.Send(new SwitchView(myAddVideoVm), Token);
                            break;
                        }
                    case "Teaching Aide":
                        {
                            Subject = "Teaching Aides";

                            //Send message to display AddTeachingAideView 
                            var myAddTeachingAideVm = Locator.AddTeachingAideView;
                            Messenger.Default.Send(new SwitchView(myAddTeachingAideVm), Token);
                            break;
                        }
                    case "Other":
                        {
                            Subject = "Other";

                            //Send message to display AddOtherView
                            var myAddOtherVm = Locator.AddOtherView;
                            Messenger.Default.Send(new SwitchView(myAddOtherVm), Token);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        private string _customerName;
        [Required]
        [StringLength(41, MinimumLength = 2)]
        public string ConsignorName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                OnPropertyChanged();
            }
        }

        private bool IsBarcodesOpened { get; set; }

        /// <summary>
        /// Gets the itemTypeComboValues list.
        /// </summary>
        public List<ComboBoxListValues> ItemTypeComboValues
        {
            get { return _itemTypeComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets or sets the ListedPrice value.
        /// </summary>
        public double ListedPrice { get; set; }

        /// <summary>
        /// Gets or sets the ListedDate value.
        /// </summary>
        public DateTime ListedDate { get; set; }

        /// <summary>
        /// Gets or sets the Subject value.
        /// </summary>
        private string _subject;

        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Status value.
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the Condition value.
        /// </summary>
        [Required]
        public string Condition { get; set; }

        /// <summary>
        /// Gets or sets the Description value.
        /// </summary>
        public string Description { get; set; }

        public double MinListedPrice
        {
            get { return ConfigSettings.MIN_LISTED_PRICE; }
        }

        /// <summary>
        /// Gets or sets the ItemTypeIsNotNull value. This value signifies whether certain 
        ///     fields are enabled on the view
        /// </summary>
        private bool _itemTypeIsNotNull;
        public bool ItemTypeIsNotNull
        {
            get { return _itemTypeIsNotNull; }
            set
            {
                _itemTypeIsNotNull = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the ItemIsDiscountable value. This value signifies whether the item 
        ///    will reduce in price over time
        /// </summary>
        public bool ItemIsDiscountable { get; set; }

        /// <summary>
        /// Gets a value indicating whether the view model is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (ItemType == "Book")
                    return !String.IsNullOrWhiteSpace(ItemType) &&
                          !String.IsNullOrWhiteSpace(ConsignorName) &&
                          !String.IsNullOrWhiteSpace(Status) &&
                          !String.IsNullOrWhiteSpace(Condition) &&
                          !Double.IsNaN(ListedPrice) &&
                          !(ListedPrice < ConfigSettings.MIN_LISTED_PRICE) &&
                          !String.IsNullOrWhiteSpace(BookItem.Isbn) &&
                          !String.IsNullOrWhiteSpace(BookItem.Title);
                else if (ItemType == "Video")
                    return !String.IsNullOrWhiteSpace(ItemType) &&
                          !String.IsNullOrWhiteSpace(ConsignorName) &&
                          !String.IsNullOrWhiteSpace(Status) &&
                          !String.IsNullOrWhiteSpace(Condition) &&
                          !Double.IsNaN(ListedPrice) &&
                          !(ListedPrice < ConfigSettings.MIN_LISTED_PRICE) &&
                          !String.IsNullOrWhiteSpace(VideoItem.Title) &&
                          !String.IsNullOrWhiteSpace(VideoItem.VideoFormat);
                else
                    return !String.IsNullOrWhiteSpace(ItemType) &&
                           !String.IsNullOrWhiteSpace(ConsignorName) &&
                           !String.IsNullOrWhiteSpace(Status) &&
                           !String.IsNullOrWhiteSpace(Condition) &&
                           !Double.IsNaN(ListedPrice) &&
                           !(ListedPrice < ConfigSettings.MIN_LISTED_PRICE);
            }
        }

        /// <summary>
        /// Gets the command that allows a customer to be added.
        /// </summary>
        public ActionCommand AddItemCommand
        {
            get
            {
                return new ActionCommand(p => AddItem(),
                                         p => IsValid);
            }
        }

        protected override string OnValidate(string propertyName)
        {
            if (propertyName == "ListedPrice")
            {
                if (ListedPrice < ConfigSettings.MIN_LISTED_PRICE)
                    return "Listed Price must be $" + ConfigSettings.MIN_LISTED_PRICE + " or more";
            }

            //if (propertyName == "ItemType")
            //{
            //    if (itemType == null)
            //        return "Item Type is Required";
            //}

            return base.OnValidate(propertyName);
        }

        private void OpenBarcodeView()
        {
            //if (IsBarcodesOpened == false)
            {
                //Open Barcode DataGrid
                Messenger.Default.Send(new SwitchView(_myAddItemBarcodesVm), Token);

                IsBarcodesOpened = true;
            }
        }

        private void AddItem()
        {

            var listedDate = DateTime.Now;
            var itemVisitor = new ItemAddVisitor();

            switch (ItemType)
            {
                case "Book":
                    {

                        BookItem.ListedPrice = ListedPrice;
                        BookItem.ListedDate = listedDate;
                        BookItem.Subject = Subject;
                        BookItem.ItemStatus = Status;
                        BookItem.Condition = Condition;
                        BookItem.Description = Description;
                        BookItem.ConsignorName = ConsignorName;
                        BookItem.IsDiscountable = ItemIsDiscountable;

                        try
                        {
                            var id = BookItem.Accept(itemVisitor);
                            var controller = new ItemController();
                            var newItem = controller.GetItemById(id);
                            if (newItem == null) throw new NullReferenceException("ItemAddVisitor Cannot find added item - BookItem");

                            Items.Add(newItem);
                            Barcodes.Add(new BarcodeItem(newItem));
                            //OpenBarcodeView();
                            Messenger.Default.Send(new PropertySetter("DataGridBarcodes", Barcodes), Token);

                            BookItem.TradeInValue = 0;
                            BookItem.LowestUsedPrice = 0;
                            BookItem.LowestNewPrice = 0;
                            BookItem.BookImage = null;
                            Messenger.Default.Send(new PropertySetterString("TradeInValue", BookItem.TradeInValue.ToString()), Token);
                            Messenger.Default.Send(new PropertySetterString("BookLowestUsedPrice", BookItem.LowestUsedPrice.ToString(CultureInfo.InvariantCulture)), Token);
                            Messenger.Default.Send(new PropertySetterString("BookLowestNewPrice", BookItem.LowestNewPrice.ToString(CultureInfo.InvariantCulture)), Token);
                            Messenger.Default.Send(new PropertySetterString("BookImage", BookItem.BookImage), Token);
                            break;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "AddItem-BookException");
                            return;
                        }

                    }

                case "Game":
                    {
                        GameItem.ListedPrice = ListedPrice;
                        GameItem.ListedDate = listedDate;
                        GameItem.Subject = Subject;
                        GameItem.ItemStatus = Status;
                        GameItem.Condition = Condition;
                        GameItem.Description = Description;
                        GameItem.ConsignorName = ConsignorName;
                        GameItem.IsDiscountable = ItemIsDiscountable;

                        try
                        {
                            var id = GameItem.Accept(itemVisitor);
                            Item newItem;
                            var controller = new ItemController();
                            newItem = controller.GetItemById(id);
                            if (newItem == null) throw new NullReferenceException("ItemAddVisitor Cannot find added item - GameItem");

                            Items.Add(newItem);
                            Barcodes.Add(new BarcodeItem(newItem));
                            //OpenBarcodeView();
                            Messenger.Default.Send(new PropertySetter("DataGridBarcodes", Barcodes), Token);

                            
                            GameItem.LowestUsedPrice = 0;
                            GameItem.LowestNewPrice = 0;
                            GameItem.GameImage = null;
                            Messenger.Default.Send(new PropertySetterString("GameLowestUsedPrice", GameItem.LowestUsedPrice.ToString(CultureInfo.InvariantCulture)), Token);
                            Messenger.Default.Send(new PropertySetterString("GameLowestNewPrice", GameItem.LowestNewPrice.ToString(CultureInfo.InvariantCulture)), Token);
                            Messenger.Default.Send(new PropertySetterString("GameImage", GameItem.GameImage), Token);
                            break;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "AddItem-GameException");
                            return;
                        }

                    }

                case "Other":
                    {
                        OtherItem.ListedPrice = ListedPrice;
                        OtherItem.ListedDate = listedDate;
                        OtherItem.Subject = Subject;
                        OtherItem.ItemStatus = Status;
                        OtherItem.Condition = Condition;
                        OtherItem.Description = Description;
                        OtherItem.ConsignorName = ConsignorName;
                        OtherItem.IsDiscountable = ItemIsDiscountable;

                        try
                        {
                            var id = OtherItem.Accept(itemVisitor);
                            Item newItem;
                            var controller = new ItemController();
                            newItem = controller.GetItemById(id);
                            if (newItem == null) throw new NullReferenceException("ItemAddVisitor Cannot find added item - OtherItem");

                            Items.Add(newItem);
                            Barcodes.Add(new BarcodeItem(newItem));
                            //OpenBarcodeView();
                            Messenger.Default.Send(new PropertySetter("DataGridBarcodes", Barcodes), Token);

                            OtherItem.LowestUsedPrice = 0;
                            OtherItem.LowestNewPrice = 0;
                            OtherItem.OtherImage = null;
                            Messenger.Default.Send(new PropertySetterString("OtherLowestUsedPrice", OtherItem.LowestUsedPrice.ToString(CultureInfo.InvariantCulture)), Token);
                            Messenger.Default.Send(new PropertySetterString("OtherLowestNewPrice", OtherItem.LowestNewPrice.ToString(CultureInfo.InvariantCulture)), Token);
                            Messenger.Default.Send(new PropertySetterString("OtherImage", OtherItem.OtherImage), Token);
                            break;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "AddItem-OtherException");
                            return;
                        }

                    }


                case "Teaching Aide":
                    {
                        TeachingAideItem.ListedPrice = ListedPrice;
                        TeachingAideItem.ListedDate = listedDate;
                        TeachingAideItem.Subject = Subject;
                        TeachingAideItem.ItemStatus = Status;
                        TeachingAideItem.Condition = Condition;
                        TeachingAideItem.Description = Description;
                        TeachingAideItem.ConsignorName = ConsignorName;
                        TeachingAideItem.IsDiscountable = ItemIsDiscountable;

                        try
                        {
                            var id = TeachingAideItem.Accept(itemVisitor);
                            Item newItem;
                            var controller = new ItemController();
                            newItem = controller.GetItemById(id);
                            if (newItem == null) throw new NullReferenceException("ItemAddVisitor Cannot find added item - TeachingAideItem");

                            Items.Add(newItem);
                            Barcodes.Add(new BarcodeItem(newItem));
                            //OpenBarcodeView();
                            Messenger.Default.Send(new PropertySetter("DataGridBarcodes", Barcodes), Token);

                            TeachingAideItem.LowestUsedPrice = 0;
                            TeachingAideItem.LowestNewPrice = 0;
                            TeachingAideItem.TeachingAideImage = null;
                            Messenger.Default.Send(new PropertySetterString("TeachingAideLowestUsedPrice", TeachingAideItem.LowestUsedPrice.ToString(CultureInfo.InvariantCulture)), Token);
                            Messenger.Default.Send(new PropertySetterString("TeachingAideLowestNewPrice", TeachingAideItem.LowestNewPrice.ToString(CultureInfo.InvariantCulture)), Token);
                            Messenger.Default.Send(new PropertySetterString("TeachingAideImage", TeachingAideItem.TeachingAideImage), Token);
                            break;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "AddItem-TeachingAideException");
                            return;
                        }
                    }

                case "Video":
                    {
                        VideoItem.ListedPrice = ListedPrice;
                        VideoItem.ListedDate = listedDate;
                        VideoItem.Subject = Subject;
                        VideoItem.ItemStatus = Status;
                        VideoItem.Condition = Condition;
                        VideoItem.Description = Description;
                        VideoItem.ConsignorName = ConsignorName;
                        VideoItem.IsDiscountable = ItemIsDiscountable;

                        try
                        {
                            var id = VideoItem.Accept(itemVisitor);
                            Item newItem;
                            var controller = new ItemController();
                            newItem = controller.GetItemById(id);
                            if (newItem == null) throw new NullReferenceException("ItemAddVisitor Cannot find added item - VideoItem");

                            Items.Add(newItem);
                            Barcodes.Add(new BarcodeItem(newItem));
                            //OpenBarcodeView();
                            Messenger.Default.Send(new PropertySetter("DataGridBarcodes", Barcodes), Token);

                            VideoItem.LowestUsedPrice = 0;
                            VideoItem.LowestNewPrice = 0;
                            VideoItem.VideoImage = null;
                            Messenger.Default.Send(new PropertySetterString("VideoLowestUsedPrice", VideoItem.LowestUsedPrice.ToString(CultureInfo.InvariantCulture)), Token);
                            Messenger.Default.Send(new PropertySetterString("VideoLowestNewPrice", VideoItem.LowestNewPrice.ToString(CultureInfo.InvariantCulture)), Token);
                            Messenger.Default.Send(new PropertySetterString("VideoImage", VideoItem.VideoImage), Token);
                            break;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "AddItem-VideoException");
                            return;
                        }

                    }

            }
        }
    }
}