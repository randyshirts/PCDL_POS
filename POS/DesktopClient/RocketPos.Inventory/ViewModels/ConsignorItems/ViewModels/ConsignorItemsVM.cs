// -----------------------------------------------------------------------------
//  <copyright file="CustomerViewModel.cs" company="DCOM Engineering, LLC">
//      Copyright (c) DCOM Engineering, LLC.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using GalaSoft.MvvmLight.Command;
using Inventory.Controller.CustomClasses;
using Inventory.Controller.Elements.ItemElements;
using Inventory.Controller.Visitors.ItemVisitors;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;
using RocketPos.Data.TransactionalLayer;

namespace Inventory.ViewModels.ConsignorItems.ViewModels
{
    /// <summary>
    /// A View-Model that represents a Consignor and its state information.
    /// </summary>
    public class ConsignorItemsVm : ViewModel
    {
        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        //private EnumsAndLists myEnumsLists = new EnumsAndLists();   //Various lists and enums used by the application

        //List for sources of ComboBoxes
        private readonly ComboBoxListValues _statusComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _subjectComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _itemTypeComboValues = new ComboBoxListValues();
        private readonly ComboBoxListValues _consignorNameComboValues = new ComboBoxListValues();

        public ConsignorItemsVm()
        {
            DataGridConsignorItems = new TrulyObservableCollection<ConsignorItem>();

            _subjectComboValues.InitializeComboBox(EnumsAndLists.Subjects);

            //Initialize ComboBoxes
            var statusComboList = new List<string>(EnumsAndLists.Statuses);
            statusComboList.Insert(0, "All");
            _statusComboValues.InitializeComboBox(statusComboList);

            var itemTypeComboList = new List<string>(EnumsAndLists.ItemTypes);
            itemTypeComboList.Insert(0, "All");
            _itemTypeComboValues.InitializeComboBox(itemTypeComboList);

            var controller = new ConsignorController();
            var consignorNameComboList = new List<string>(controller.GetConsignorNames());
            consignorNameComboList.Insert(0, "All");
            _consignorNameComboValues.InitializeComboBox(consignorNameComboList);


            //Initialize checkboxes
            CashPayoutEnabled = false;
            PayoutEnabled = false;


            //Register for messages
            //Messenger.Default.Register<PropertySetter>(this, AddGameVM.Token, msg => SetGameProperty(msg.PropertyName, (string)msg.PropertyValue));         

            //Fill grid with all games on record
            //InitializeDataGrid();
        }

        /// <summary>
        /// Gets the collection of Item entities.
        /// </summary>
        public ICollection<Item> Items { get; set; }

        #region ConsignorItemVM Stuff
        /// <summary>
        /// Gets the statusComboValues list.
        /// </summary>
        public List<ComboBoxListValues> StatusComboValues
        {
            get { return _statusComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets the statusComboValues list.
        /// </summary>
        public List<ComboBoxListValues> SubjectComboValues
        {
            get { return _subjectComboValues.ComboValues; }
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
        public List<ComboBoxListValues> ConsignorNameComboValues
        {
            get { return _consignorNameComboValues.ComboValues; }
        }

        public ICommand WindowLoaded
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
        /// Gets or sets the consignor name.
        /// </summary>
        private string _consignorName;
        [Required]
        [StringLength(41, MinimumLength = 2)]
        public string ConsignorName
        {
            get { return _consignorName; }
            set
            {
                _consignorName = value;
                SearchConsignor();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                SearchConsignor();
                if (_status == "Sold")
                {
                    PayoutEnabled = true;
                }
                else
                {
                    PayoutEnabled = false;
                    PayoutIsSelected = false;
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the ItemType.
        /// </summary>
        private string _itemType;
        public string ItemType
        {
            get { return _itemType; }
            set
            {
                _itemType = value;
                SearchConsignor();
                OnPropertyChanged();

            }
        }

        private bool _payoutEnabled;
        public bool PayoutEnabled
        {
            get
            {
                return _payoutEnabled;
            }
            set
            {
                _payoutEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _payoutIsSelected;
        public bool PayoutIsSelected
        {
            get
            {
                return _payoutIsSelected;
            }
            set
            {
                _payoutIsSelected = value;

                if (_payoutIsSelected)
                {
                    CashPayoutEnabled = true;
                    DataGridConsignorItems = UpdateConsignorPortion(CashIsSelected);
                    IsVisiblePayoutAmount = "Visible";
                }
                else
                {
                    CashPayoutEnabled = false;
                    DataGridConsignorItems = ClearConsignorPortion();
                    IsVisiblePayoutAmount = "Hidden";
                }

                OnPropertyChanged();
            }
        }


        private bool _cashIsSelected;
        public bool CashIsSelected
        {
            get
            {
                return _cashIsSelected;
            }
            set
            {
                _cashIsSelected = value;

                DataGridConsignorItems = UpdateConsignorPortion(_cashIsSelected);
                //PayoutAmountText = UpdatePayoutAmount();

                //CollectionChanged;
                OnPropertyChanged();
            }
        }

        private bool _cashPayoutEnabled;
        public bool CashPayoutEnabled
        {
            get
            {
                return _cashPayoutEnabled;
            }
            set
            {
                _cashPayoutEnabled = value;
                if (!_cashPayoutEnabled)
                    CashIsSelected = false;
                OnPropertyChanged();
            }
        }


        private string _payoutAmountText;
        public string PayoutAmountText
        {
            get
            {
                return _payoutAmountText;
            }
            set
            {
                _payoutAmountText = value;
                OnPropertyChanged();
            }
        }

        private string _isVisiblePayoutAmount;
        public string IsVisiblePayoutAmount
        {
            get
            {
                return _isVisiblePayoutAmount;
            }
            set
            {
                _isVisiblePayoutAmount = value;
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
                if (!String.IsNullOrWhiteSpace(ConsignorName))
                {
                    if (ConsignorName.Length > 42)
                        return false;
                }
                else
                    return false;

                return true;

            }
        }

        /// <summary>
        /// Gets the command that allows a consignor's items to be searched.
        /// </summary>
        public void SearchConsignor()
        {

            //if (Status != null)
            //    SearchConsignorItems(ConsignorName, Status);
            //else
            //    SearchConsignorItems(ConsignorName);

            //Populate DataGridConsignorItems
            var controller = new ItemRepository();
            var items = controller.SearchAllItems(null, Status, ItemType, ConsignorName, null);
            DataGridConsignorItems = CreateElementLists.CreateConsignorItemsList(items);

        }


        /// <summary>
        /// Gets the command that allows a consignor's items to be paid.
        /// </summary>
        public ActionCommand ProcessPaymentCommand
        {
            get
            {
                return new ActionCommand(p => ProcessConsignorPayment());
            }
        }

        protected override string OnValidate(string propertyName)
        {

            if (propertyName == "CustomerName")
            {
                if (ConsignorName == null)
                    return "Consignor Name is Required";
            }

            if (propertyName == "Status")
            {

            }

            return base.OnValidate(propertyName);
        }

        //private void SearchConsignorItems(string consignorName)
        //{
        //    using (var api = new BusinessContext())
        //    {
        //        if (ConsignorName != null)
        //        {
        //            if (ConsignorName != "All")
        //            {
        //                var names = ConsignorName.Split();

        //                var items = new ObservableCollection<Item>(api.GetItemsByConsignorName(names[0], names[1]));

        //                //Populate DataGridConsignorItems
        //                DataGridConsignorItems = CreateConsignorItemsList(items);
        //            }
        //            else
        //            {
        //                var items = new ObservableCollection<Item>(api.GetAllItems());

        //                //Populate DataGridConsignorItems
        //                DataGridConsignorItems = CreateConsignorItemsList(items);
        //            }
        //        }
        //    }
        //}

        //private void SearchConsignorItems(string consignorName, string status)
        //{
        //    using (var api = new BusinessContext())
        //    {
        //        if (ConsignorName != null)
        //        {
        //            if (ConsignorName != "All")
        //            {
        //                var names = ConsignorName.Split();

        //                if (Status != "All")
        //                {
        //                    var items = new ObservableCollection<Item>(api.GetItemsByConsignorNameAndStatus(names[0], names[1], Status));
        //                    //Populate DataGridConsignorItems
        //                    DataGridConsignorItems = CreateConsignorItemsList(items);
        //                }
        //                else
        //                {
        //                    var items = new ObservableCollection<Item>(api.GetItemsByConsignorName(names[0], names[1]));
        //                    //Populate DataGridConsignorItems
        //                    DataGridConsignorItems = CreateConsignorItemsList(items);
        //                }
        //            }
        //            else
        //            {
        //                if (Status != "All")
        //                {
        //                    var items = new ObservableCollection<Item>(api.GetItemsByStatus(Status));
        //                    //Populate DataGridConsignorItems
        //                    DataGridConsignorItems = CreateConsignorItemsList(items);
        //                }
        //                else
        //                {
        //                    var items = new ObservableCollection<Item>(api.GetAllItems());
        //                    //Populate DataGridConsignorItems
        //                    DataGridConsignorItems = CreateConsignorItemsList(items);

        //                }
        //            }

        //        }
        //    }
        //}

        private void ProcessConsignorPayment()
        {
            //Create a PaymentTransaction which includes date, amount, consignor, items



            //Create list of items to be paid
            var itemsList = new List<Item>();
            foreach (var consignorItem in DataGridConsignorItems)
            {
                if (consignorItem.ItemStatus == "Sold")
                {
                    var icontroller = new ItemController();
                    var item = icontroller.GetItemById(consignorItem.Id);
                    itemsList.Add(item);
                }
            }

            var ccontroller = new ConsignorController();
            var consignor = ccontroller.GetConsignorByFullName(ConsignorName);
            //Create StoreCreditPmt if cash is not checked
            var storeCredit = new StoreCreditPmt();

            if (!_cashIsSelected)
            {
                storeCredit.StoreCreditPmtAmount = double.Parse(PayoutAmountText, NumberStyles.Currency);
                //storeCredit.Consignor_StoreCreditPmt = consignor;
                //storeCredit.Consignor_StoreCreditPmt.Consignor_Person = null;
                //storeCredit.Consignor_StoreCreditPmt.Items_Consignor = null;
                storeCredit.ConsignorId = consignor.Id;
            }
            else
            {
                storeCredit = null;
            }

            var consignorPmt = new ConsignorPmt
            {
                //Consignor Info
                //Consignor_ConsignorPmt = consignor,
                ConsignorId = consignor.Id,


                //DebitTransaction Info
                DebitTransaction_ConsignorPmt = new DebitTransaction
                {
                    DebitTotal = double.Parse(PayoutAmountText, NumberStyles.Currency),
                    DebitTransactionDate = DateTime.Now,
                    StoreCreditPmt = storeCredit,
                },



                //Items info
                Items_ConsignorPmt = itemsList,

            };

            try
            {
                var cpcontroller = new ConsignorPmtController();
                cpcontroller.AddNewConsignorPmt(consignorPmt);

                ConsignorName = consignor.Consignor_Person.FirstName + " " + consignor.Consignor_Person.LastName;
                Status = "All";

            }
            catch (Exception ex)
            {
                foreach (var item in DataGridConsignorItems.Where(item => item.ItemStatus == "Paid"))
                {
                    item.ItemStatus = "Sold";
                }
                MessageBox.Show("Exception adding Consignor Payment in ProcessConsignorPayment method in ConsignorItems class - " + ex.Message);
            }






            // Display status of transaction on screen
            // Send message to open cash drawer

        }


        #endregion

        #region DataGrid Stuff
        /// <summary>
        /// Gets the collection of ConsignorItem entities.
        /// </summary>
        private TrulyObservableCollection<ConsignorItem> _dataGridConsignorItems;
        public TrulyObservableCollection<ConsignorItem> DataGridConsignorItems
        {
            get { return _dataGridConsignorItems; }

            set
            {
                _dataGridConsignorItems = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the selectedItem row from the datagrid.
        /// </summary>
        private ConsignorItem _selectedConsignorItem;
        public ConsignorItem SelectedConsignorItem
        {
            get { return _selectedConsignorItem; }
            set
            {
                _selectedConsignorItem = value;
                OnSelected();
            }
        }

        private static void OnSelected()
        {
            //if (SelectedConsignor != null)
            //    Messenger.Default.Send(new PropertySetter("GameImage", SelectedGameItem.GameImage), Token);
        }

        /// <summary>
        /// Gets the command that allows a cell to be updated.
        /// </summary>
        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        {
            get
            {
                return new RelayCommand<DataGridCellEditEndingEventArgs>(UpdateConsignorItem);
            }
        }

        /// <summary>
        /// Gets the command that allows a row to be deleted.
        /// </summary>
        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return new RelayCommand(DeleteConsignorItem);
            }
        }

        private void DeleteConsignorItem()
        {

            if (MessageBox.Show("Deletions are permanent. Are you sure you want to delete this item? ", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            if (null == SelectedConsignorItem) return;
            var consignorItem = DataGridConsignorItems.FirstOrDefault(ci => ci.Consignor_Item.Id == SelectedConsignorItem.Consignor_Item.Id);
            if (consignorItem == null) return;
            var tempConsignors = DataGridConsignorItems;
            var gridIndex = tempConsignors.IndexOf(consignorItem);
            if ((gridIndex + 1) == tempConsignors.Count)
                gridIndex--;
            tempConsignors.Remove(consignorItem);
            DataGridConsignorItems = tempConsignors;
            SelectedConsignorItem = DataGridConsignorItems.ElementAtOrDefault(gridIndex);

            var icontroller = new ItemController();
            icontroller.DeleteItemById(consignorItem.Consignor_Item.Id);

        }

        private void UpdateConsignorItem(DataGridCellEditEndingEventArgs e)
        {


            var text = WpfHelpers.GetTextFromCellEditingEventArgs(e);

            //Get the Column header
            var column = e.Column.Header.ToString();

            //Add info to new ConsignorInfo
            var consignorItem = e.Row.Item as ConsignorItem;
            var success = consignorItem != null && consignorItem.SetProperty(column, text);

            if (ConsignorItemIsValid(consignorItem) && success)
            {
                var updateVisitor = new ItemUpdateVisitor();
                consignorItem.Accept(updateVisitor);
            }
            else
            {
                e.Cancel = true;
                if (consignorItem != null) consignorItem.ReportError(column, text);
            }

        }

        private static bool ConsignorItemIsValid(ConsignorItem consignorItem)
        {
            if (consignorItem.ItemType == "Book")
                return !String.IsNullOrWhiteSpace(consignorItem.Title) &&
                        (consignorItem.Title.Length <= 150) &&
                        !String.IsNullOrWhiteSpace(consignorItem.IsbnEan) &&
                        ((consignorItem.IsbnEan.Length == 13) || (consignorItem.IsbnEan.Length == 10)) &&
                        !String.IsNullOrWhiteSpace(consignorItem.ItemStatus) &&
                        !String.IsNullOrWhiteSpace(consignorItem.ListedPrice) &&
                        Validation_Helper.ParseCurrency(consignorItem.ListedPrice) &&
                        Validation_Helper.ParseCurrency(consignorItem.SoldPrice);
            if (!String.IsNullOrWhiteSpace(consignorItem.IsbnEan))
                return !String.IsNullOrWhiteSpace(consignorItem.Title) &&
                       (consignorItem.Title.Length <= 150) &&
                       (consignorItem.IsbnEan.Length == 13) &&
                       !String.IsNullOrWhiteSpace(consignorItem.ItemStatus) &&
                       !String.IsNullOrWhiteSpace(consignorItem.ListedPrice) &&
                       Validation_Helper.ParseCurrency(consignorItem.ListedPrice) &&
                       Validation_Helper.ParseCurrency(consignorItem.SoldPrice);
            return !String.IsNullOrWhiteSpace(consignorItem.Title) &&
                   (consignorItem.Title.Length <= 150) &&
                   !String.IsNullOrWhiteSpace(consignorItem.ItemStatus) &&
                   !String.IsNullOrWhiteSpace(consignorItem.ListedPrice) &&
                   Validation_Helper.ParseCurrency(consignorItem.ListedPrice) &&
                   Validation_Helper.ParseCurrency(consignorItem.SoldPrice);
        }

        //private void InitializeDataGrid(ObservableCollection<Item> items)
        //{

        //        //Clear DataGridConsignorItems
        //        DataGridConsignorItems.Clear();

        //        DataGridConsignorItems = CreateConsignorItemsList(items);

        //}

        ////Make a collection of ConsignorItems with info from the Items Table
        //private TrulyObservableCollection<ConsignorItem> CreateConsignorItemsList(IEnumerable<Item> items)
        //{
        //    //Temp Collection Definition
        //    var tempConsignorItems = new TrulyObservableCollection<ConsignorItem>();

        //    //Clear DataGridConsignorItems
        //    DataGridConsignorItems.Clear();

        //    //Make the collection
        //    var i = items.GetEnumerator();
        //    while (i.MoveNext())
        //    {
        //        //Create an instance of a ConsignorInfo with the current consignor and person info
        //        var currentConsignorItem = new ConsignorItem(i.Current);
        //        //Add the new instance to the collection
        //        tempConsignorItems.Add(currentConsignorItem);
        //    }

        //    return tempConsignorItems;
        //}

        public TrulyObservableCollection<ConsignorItem> UpdateConsignorPortion(bool cash)
        {
            var consignorItems = DataGridConsignorItems;

            double totalPayment = 0;

            foreach (var ci in consignorItems)
            {
                double currentPmt;
                if ((ci.SoldPrice != null) && (ci.ItemStatus == "Sold"))
                {
                    if (cash)
                    {
                        currentPmt = (double.Parse(ci.SoldPrice, NumberStyles.Currency) * ConfigSettings.CONS_CASH_PAYOUT_PCT);
                    }
                    else
                    {
                        currentPmt = (double.Parse(ci.SoldPrice, NumberStyles.Currency) * ConfigSettings.CONS_CREDIT_PAYOUT_PCT);
                    }

                    ci.ConsignorPortion = currentPmt.ToString("C2");
                    totalPayment += currentPmt;
                }
                else if ((ci.SoldPrice == null) && (ci.ItemStatus == "Sold"))
                {
                    currentPmt = 0;
                    ci.ConsignorPortion = currentPmt.ToString("C2");
                    totalPayment += currentPmt;
                }
            }

            PayoutAmountText = totalPayment.ToString("C2");

            return consignorItems;
        }

        public TrulyObservableCollection<ConsignorItem> ClearConsignorPortion()
        {
            var consignorItems = DataGridConsignorItems;

            foreach (var ci in consignorItems)
            {
                ci.ConsignorPortion = null;
            }

            PayoutAmountText = "$0";

            return consignorItems;
        }
        #endregion
    }
}