// -----------------------------------------------------------------------------
//  <copyright file="CustomerViewModel.cs" company="DCOM Engineering, LLC">
//      Copyright (c) DCOM Engineering, LLC.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using Inventory.Controller.CustomClasses;
using Inventory.Controller.Elements;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;

namespace Inventory.ViewModels.PrintBarcodesView.ViewModels
{
    //using Google.Apis.Books.v1;

    /// <summary>
    /// A View-Model that represents a customer and its state information.
    /// </summary>
    public class PrintBarcodesVm : ViewModel
    {

        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application


        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _itemTypeComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _statusComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _consignorNameComboValues = new ComboBoxListValues();

        public PrintBarcodesVm()
        {
            DataGridBarcodes = new TrulyObservableCollection<BarcodeItem>();

            //Initialize ComboBoxes
            List<string> itemTypeComboList = new List<string>(EnumsAndLists.ItemTypes);
            itemTypeComboList.Insert(0, "All");
            _itemTypeComboValues.InitializeComboBox(itemTypeComboList);

            List<string> statusComboList = new List<string>(EnumsAndLists.Statuses);
            statusComboList.Insert(0, "All");
            _statusComboValues.InitializeComboBox(statusComboList);

            var controller = new ConsignorController();
            List<string> consignorNameComboList = new List<string>(controller.GetConsignorNames());
            consignorNameComboList.Insert(0, "All");
            _consignorNameComboValues.InitializeComboBox(consignorNameComboList);
            
        }

        private void LoadWindow()
        {
            //Load ConsignorNames
            var controller = new ConsignorController();
            var consignorNameComboList = new List<string>(controller.GetConsignorNames());
            consignorNameComboList.Insert(0, "All");
            _consignorNameComboValues.InitializeComboBox(consignorNameComboList);
            
        }

        private readonly string _barcode = null;
        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        private string _consignorName;
        public string ConsignorName
        {
            get { return _consignorName; }
            set
            {
                _consignorName = value;

                var controller = new ItemController();

                DataGridBarcodes = CreateBarcodeItemsList(
                                        controller.SearchAllItems(_barcode, Status, _itemType, ConsignorName, ListedDate));

                OnPropertyChanged();
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

                var controller = new ItemController();
                DataGridBarcodes = CreateBarcodeItemsList(
                                         controller.SearchAllItems(_barcode, Status, _itemType, ConsignorName, ListedDate));
                
            }
        }

        private DateTime? _listedDate;
        public DateTime? ListedDate
        {
            get { return _listedDate; }
            set
            {
                
                if (value != null)
                {
                    _listedDate = value;
                    var d = _listedDate ?? default(DateTime);
                    _listedDateString = d.ToString("MM/dd/yy");
                   
                }
                else
                {
                    _listedDateString = null;
                    _listedDate = null;
                }
                var controller = new ItemController();
                DataGridBarcodes = CreateBarcodeItemsList(
                                        controller.SearchAllItems(_barcode, Status, _itemType, ConsignorName, ListedDate));
                
            }
        }

        /// <summary>
        /// Gets or sets the Status value.
        /// </summary>
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                var controller = new ItemController();
                DataGridBarcodes = CreateBarcodeItemsList(
                                        controller.SearchAllItems(_barcode, Status, _itemType, ConsignorName, ListedDate));

            }
        }

        private string _listedDateString;

        public ActionCommand PrintBarcodesButtonCommand
        {
            get
            {
                return new ActionCommand(p => PrintBarcodes.PrintBarcodesItems(DataGridBarcodes.ToList()));
            }
        }

        private void ClearBarcodesButtonGrid()
        {
            DataGridBarcodes.Clear();
        }


        /// <summary>
        /// Gets a value indicating whether the view model is valid.
        /// </summary>
        //public bool IsValid
        //{
        //    get
        //    {
        //        if (ItemType == "Book")
        //            return !String.IsNullOrWhiteSpace(ItemType);
        //              //!String.IsNullOrWhiteSpace(Status) ||
        //              //!String.IsNullOrWhiteSpace(myAddBookVM.ISBN) ||
        //              //!String.IsNullOrWhiteSpace(myAddBookVM.Title);
        //        else if (ItemType == "Video")
        //            return !String.IsNullOrWhiteSpace(ItemType) ||
        //              !String.IsNullOrWhiteSpace(Status);
        //        else
        //            return !String.IsNullOrWhiteSpace(ItemType) ||
        //               !String.IsNullOrWhiteSpace(Status);
        //    }
        //}

        //protected override string OnValidate(string propertyName)
        //{

        //    //if (propertyName == "ItemType")
        //    //{
        //    //    if (itemType == null)
        //    //        return "Item Type is Required";
        //    //}

        //    return base.OnValidate(propertyName);
        //}

        //Make a collection of BarcodeItems with info from a collection of Items 
        public static TrulyObservableCollection<BarcodeItem> CreateBarcodeItemsList(IEnumerable<Item> items)
        {
            //Temp Collection Definition
            var tempBarcodeItems = new TrulyObservableCollection<BarcodeItem>();

            if (items == null) return tempBarcodeItems;

            //Make the collection
            var i = items.GetEnumerator();
            while (i.MoveNext())
            {
                //Create an instance of a BarcodeItem with the current item
                var currentBarcodeItem = new BarcodeItem(i.Current);
                //Add the new instance to the collection
                tempBarcodeItems.Add(currentBarcodeItem);
            }

            return tempBarcodeItems;
        }

        #region DataGrid Stuff
        /// <summary>
        /// Gets the collection of BarcodeItem entities.
        /// </summary>
        private TrulyObservableCollection<BarcodeItem> _dataGridBarcodes;
        public TrulyObservableCollection<BarcodeItem> DataGridBarcodes
        {
            get { return _dataGridBarcodes; }
            set
            {
                _dataGridBarcodes = value;
                OnPropertyChanged("DataGridBarcodes");
            }
        }

        /// <summary>
        /// Gets the selectedItem row from the datagrid.
        /// </summary>
        private BarcodeItem _selectedBarcodeItem;
        public BarcodeItem SelectedBarcodeItem
        {
            get { return _selectedBarcodeItem; }
            set
            {
                _selectedBarcodeItem = value;
            }
        }
        #endregion
    }

    
}