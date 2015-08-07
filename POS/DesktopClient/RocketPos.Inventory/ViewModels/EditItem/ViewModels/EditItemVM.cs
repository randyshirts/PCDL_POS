// -----------------------------------------------------------------------------
//  <copyright file="CustomerViewModel.cs" company="DCOM Engineering, LLC">
//      Copyright (c) DCOM Engineering, LLC.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.TransactionalLayer.Repositories;
using GalaSoft.MvvmLight.Messaging;
using Inventory.Controller.CustomClasses;
using Inventory.Controller.Elements;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;

namespace Inventory.ViewModels.EditItem.ViewModels
{
    //using Google.Apis.Books.v1;

    /// <summary>
    /// A View-Model that represents a customer and its state information.
    /// </summary>
    public class EditItemVm : ViewModel
    {
        
        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance
        
        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application


        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _itemTypeComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _subjectComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _statusComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _consignorNameComboValues = new ComboBoxListValues();

        public EditItemVm()
        {
            //Disable fields on view
            ItemTypeIsNotNull = false;

            //Initialize ComboBoxes
            _subjectComboValues.InitializeComboBox(EnumsAndLists.Subjects);

            _itemTypeComboValues.InitializeComboBox(EnumsAndLists.ItemTypes);

            var statusComboList = new List<string>(EnumsAndLists.Statuses);
            statusComboList.Insert(0, "All");
            _statusComboValues.InitializeComboBox(statusComboList);

            var controller = new ConsignorController();
            {
                List<string> consignorNameComboList = new List<string>(controller.GetConsignorNames());
                consignorNameComboList.Insert(0, "All");
                _consignorNameComboValues.InitializeComboBox(consignorNameComboList);
            }
           
            //Initialize AutoCompleteBox Available Items
            _queryCollection = InitializeBarcodeItems(); 
            
            //Register for messages
            Messenger.Default.Register<PropertySetter>(this, BookDataGridVm.Token, msg => SetBookProperty(msg.PropertyName, (string)msg.PropertyValue));
            Messenger.Default.Register<PropertySetter>(this, GameDataGridVm.Token, msg => SetGameProperty(msg.PropertyName, (string)msg.PropertyValue));
            Messenger.Default.Register<PropertySetter>(this, OtherDataGridVm.Token, msg => SetOtherProperty(msg.PropertyName, (string)msg.PropertyValue));
            Messenger.Default.Register<PropertySetter>(this, TeachingAideDataGridVm.Token, msg => SetTeachingAideProperty(msg.PropertyName, (string)msg.PropertyValue));
            Messenger.Default.Register<PropertySetter>(this, VideoDataGridVm.Token, msg => SetVideoProperty(msg.PropertyName, (string)msg.PropertyValue));
        }

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
            var consignorNameComboList = new List<string>(controller.GetConsignorNames());
            consignorNameComboList.Insert(0, "All");
            _consignorNameComboValues.InitializeComboBox(consignorNameComboList);
            
        }

        /// <summary>
        /// Sets a property of myGameGridView view model.
        /// </summary>
        private void SetGameProperty(string propertyName, string propertyValue)
        {
            if (propertyName != "GameImage") return;
          
            if (propertyValue != null) SelectedImage = propertyValue;    
        }

        ///// <summary>
        ///// Sets a property of myOtherGridView view model.
        ///// </summary>
        private void SetOtherProperty(string propertyName, string propertyValue)
        {
            if (propertyName != "OtherImage") return;

            if (propertyValue != null) SelectedImage = propertyValue;
        }

        ///// <summary>
        ///// Sets a property of myVideoGridView view model.
        ///// </summary>
        private void SetVideoProperty(string propertyName, string propertyValue)
        {
            if (propertyName != "VideoImage") return;

            if (propertyValue != null) SelectedImage = propertyValue;
        }

        ///// <summary>
        ///// Sets a property of myTeachingAideGridView view model.
        ///// </summary>
        private void SetTeachingAideProperty(string propertyName, string propertyValue)
        {
            if (propertyName != "teachingAideImage") return;

            if (propertyValue != null) SelectedImage = propertyValue;
        }


        ///// <summary>
        ///// Sets a property of myBookGridView view model.
        ///// </summary>
        private void SetBookProperty(string propertyName, string propertyValue)
        {

            if (propertyName != "BookImage") return;

            if (propertyValue != null) SelectedImage = propertyValue;
        
        }

        private string _selectedImage;
        public string SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        private string _consignorName;
        [StringLength(32, MinimumLength = 2)]
        public string ConsignorName
        {
            get { return _consignorName; }
            set
            {
                _consignorName = value;
                OnPropertyChanged();
                ItemType = _itemType; //Update datagrid
            }
        }

        
        /// <summary>
        /// Gets the itemTypeComboValues list.
        /// </summary>
        public List<ComboBoxListValues> ItemTypeComboValues
        {
            get { return _itemTypeComboValues.ComboValues; }
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
        public List<ComboBoxListValues> ConsignorNameComboValues
        {
            get { return _consignorNameComboValues.ComboValues; }
        }

          
        /// <summary>
        /// Gets or sets the ItemType value.
        /// </summary>
        private string _itemType;
        public string ItemType
        {
            get { return _itemType; }
            set
            {
                _itemType = value;
                OnPropertyChanged();
                ItemTypeIsNotNull = !String.IsNullOrEmpty(_itemType);



                switch (_itemType)
                {
                    case "Book":
                        {
                            //Send message to display BookDataGrid 
                            var myBookDataGridVm = new BookDataGridVm();
                            Messenger.Default.Send(new SwitchView(myBookDataGridVm), Token);

                            //Get search results
                            var controller = new ItemController();
                            
                            var items = controller.SearchAllItems(Barcode, Status, _itemType, ConsignorName, null, SearchTitle);
                            var dataGridBooks = CreateElementLists.CreateBookItemsList(items);
                            //Update Datagrid
                            Messenger.Default.Send(new PropertySetter("DataGridBooks", dataGridBooks), Token);
                            
                            break;
                        }
                    case "Game":
                        {
                            //Send message to display GameDataGrid 
                            var myGameDataGridVm = new GameDataGridVm();
                            Messenger.Default.Send(new SwitchView(myGameDataGridVm), Token);
                            
                            //Get search results
                            var controller = new ItemController();
                            var items = controller.SearchAllItems(Barcode, Status, _itemType, ConsignorName, null, SearchTitle);
                            var dataGridGames = CreateElementLists.CreateGameItemsList(items);

                            //Update Datagrid
                            Messenger.Default.Send(new PropertySetter("DataGridGames", dataGridGames), Token);
                            
                            break;
                        }
                    case "Video":
                        {
                            //Send message to display VideoDataGrid 
                            var myVideoDataGridVm = new VideoDataGridVm();
                            Messenger.Default.Send(new SwitchView(myVideoDataGridVm), Token);

                            //Get search results
                            var controller = new ItemController();
                            var items = controller.SearchAllItems(Barcode, Status, _itemType, ConsignorName, null, SearchTitle);
                            var dataGridVideos = CreateElementLists.CreateVideoItemsList(items);
                            
                            //Update Datagrid
                            Messenger.Default.Send(new PropertySetter("DataGridVideos", dataGridVideos), Token);
                            
                            break;
                        }
                    case "Teaching Aide":
                        {
                            //Send message to display TeachingAideDataGrid
                            var myTeachingAideDataGridVm = new TeachingAideDataGridVm();
                            Messenger.Default.Send(new SwitchView(myTeachingAideDataGridVm), Token);

                            //Get search results
                            var controller = new ItemController();
                            var items = controller.SearchAllItems(Barcode, Status, _itemType, ConsignorName, null, SearchTitle);
                            var dataGridTeachingAides = CreateElementLists.CreateTeachingAideItemsList(items);

                            //Update Datagrid
                            Messenger.Default.Send(
                            new PropertySetter("DataGridTeachingAides", dataGridTeachingAides), Token);
                            
                            break;
                        }
                    case "Other":
                        {
                            //Send message to display OtherDataGrid
                            var myOtherDataGridVm = new OtherDataGridVm();
                            Messenger.Default.Send(new SwitchView(myOtherDataGridVm), Token);

                            //Get search results
                            var controller = new ItemController();
                            var items = controller.SearchAllItems(Barcode, Status, _itemType, ConsignorName, null, SearchTitle);
                            var dataGridOthers = CreateElementLists.CreateOtherItemsList(items);

                            //Update Datagrid
                            Messenger.Default.Send(new PropertySetter("DataGridOthers", dataGridOthers), Token);
                            
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Gets or sets the Barcode value.
        /// </summary>
        private string _barcode;
        public string Barcode 
        {
            get { return _barcode;}
            set
            {
                _barcode = value;
                ItemType = _itemType; //Update datagrid
            }
        }

        /// <summary>
        /// Gets or sets the BarcodeItem value.
        /// </summary>
        private BarcodeItem _selectedBarcodeItem;
        public BarcodeItem SelectedBarcodeItem 
        {
            get { return _selectedBarcodeItem; }
            set 
            {
                _barcodeResultCount = 0;
                
                _selectedBarcodeItem = value;
                
                //If textcompletion is enabled this part will break the functionality of the autocomplete
                if (_selectedBarcodeItem != null)
                {
                    if (_selectedBarcodeItem.BarcodeItemBc.Length == 15)
                    {
                        Barcode = _selectedBarcodeItem.BarcodeItemBc;
                        BarcodeId = _selectedBarcodeItem.Id;
                        ItemType = _selectedBarcodeItem.ItemType;
                    }
                }
                else
                {
                    Barcode = null;
                    BarcodeId = null;
                }
            } 
        }

        public int? BarcodeId { get; set; }
        /// <summary>
        /// Gets or sets the ListedDate value.
        /// </summary>
        public DateTime ListedDate { get; set; }

        /// <summary>
        /// Gets or sets the Subject value.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the Status value.
        /// </summary>
        private string _status;
        public string Status 
        {
            get { return _status;}
            set
            {
                _status = value;
                ItemType = _itemType; //Update datagrid

            }
        }

        /// <summary>
        /// Gets or sets the Title value.
        /// </summary>
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                SearchTitle = _title.Length > 3 ? _title : null;
            }
        }

        private string _searchTitle;
        public string SearchTitle
        {
            get { return _searchTitle; }
            set
            {
                _searchTitle = value;
                if (_searchTitle.Length > 3)
                    ItemType = _itemType; //Update datagrid

            }
        }

        /// <summary>
        /// Gets or sets the ItemTypeIsNotNull value. This value signifies whether certain 
        ///     fields are enabled on the view
        /// </summary>
        private bool _itemTypeIsNotNull;
        public bool ItemTypeIsNotNull 
        { 
            get {return _itemTypeIsNotNull;}
            set
            {
                _itemTypeIsNotNull = value;
                OnPropertyChanged();
            } 
        }

        /// <summary>
        /// Gets a value indicating whether the view model is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (ItemType == "Book")
                    return !String.IsNullOrWhiteSpace(ItemType);
                      //!String.IsNullOrWhiteSpace(Status) ||
                      //!String.IsNullOrWhiteSpace(myAddBookVM.ISBN) ||
                      //!String.IsNullOrWhiteSpace(myAddBookVM.Title);
                else if (ItemType == "Video")
                    return !String.IsNullOrWhiteSpace(ItemType) ||
                      !String.IsNullOrWhiteSpace(Status);
                else
                    return !String.IsNullOrWhiteSpace(ItemType) ||
                       !String.IsNullOrWhiteSpace(Status);
            }
        }

        #region AutoComplete TextBox

        private readonly IEnumerable<BarcodeItem> _queryCollection;
        public IEnumerable<BarcodeItem> QueryCollection
        {
            get
            {
                //if ((barcode != null) && (barcode.Length > 0))
                //    SearchItemsByBarcode(barcode);
                //if (queryCollection == null)
                    //queryCollection = new List<string>();
                return _queryCollection;
            }
        }

        private IEnumerable<BarcodeItem> InitializeBarcodeItems()
        {
            try
            {
                var barcodes = new List<BarcodeItem>();
                var controller = new ItemController();
                var results = controller.GetAllItems();
                barcodes.AddRange(results.Select(item => new BarcodeItem(item)));
                
                return barcodes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "InitializeBarcodeItemsException");
                return null;
            }

        }

        private int _barcodeResultCount;
        public AutoCompleteFilterPredicate<object> BarcodeFilter
        {
            get
            {
                return DoOnBarcodeFilter;
            }
        }

        private bool DoOnBarcodeFilter(string search, object data)
        {
            var obj = data as BarcodeItem;

            var result = true;

            if ((obj != null) && obj.BarcodeItemBc.Contains(search))
            {
                if (_barcodeResultCount++ >= 10)
                {
                    result = false;
                }
            }
            else
                result = false;

            if(String.IsNullOrEmpty(search))
            {
                SelectedBarcodeItem = null;
            }

            return (result);

        }
        #endregion

    }
}