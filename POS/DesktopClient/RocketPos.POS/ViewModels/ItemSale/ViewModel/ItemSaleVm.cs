// -----------------------------------------------------------------------------
//  <copyright file="CustomerViewModel.cs" company="DCOM Engineering, LLC">
//      Copyright (c) DCOM Engineering, LLC.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Common.Helpers;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using POS.Controller.Elements;
using POS.ViewModels.PaymentWindow.ViewModels;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;

namespace POS.ViewModels.ItemSale.ViewModel
{
    /// <summary>
    /// A View-Model that represents a Consignor and its state information.
    /// </summary>
    public class ItemSaleVm : RocketPos.Common.Foundation.ViewModel
    {
        public static readonly Guid Token = Guid.NewGuid(); //So others know messages came from this instance

        public ItemSaleVm()
        {

            DataGridSaleItems = new TrulyObservableCollection<SaleItem>();
            //StoreCreditTrans = new StoreCreditTransaction();
            _queryCollection = InitializeBarcodeItems();

            IsViewFocused = true;
            _isBarcodeReading = false;
            IsPaymentComplete = false;
            _leftShiftDown = false;
            _scanShiftDown = false;
            IsMember = false;

            Messenger.Default.Register<PropertySetter>(this, PaymentWindowVm.Token, msg => GetPaymentWindowProperty(msg.PropertyName, msg.PropertyValue));

        }

        private void GetPaymentWindowProperty(string name, object value)
        {
            if (name == "IsPaymentComplete")
                SetPaymentComplete((bool)value);

            if (name == "StoreCreditTr")
                StoreCreditTrans = value as StoreCreditTransaction;
        }
        //// <summary>
        //// Gets the collection of Item entities.
        //// </summary>
        //public ICollection<Item> Items { get; set; }

        #region ItemSaleVM Stuff


        public RelayCommand<KeyEventArgs> FilterKeyDownCommand
        {
            get
            {
                return new RelayCommand<KeyEventArgs>(KeyDownHandler);
            }
        }

        private void KeyDownHandler(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    DeleteSelected();
                    return;

                case Key.LeftShift:
                    _leftShiftDown = true;
                    e.Handled = true;
                    break;

                default:
                    if (_isBarcodeReading)
                    {
                        // Handle all Keydowns while scanning to prevent other events from catching them
                        e.Handled = true;
                        if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                        {
                            _scanShiftDown = true;
                        }
                        else if (_leftShiftDown && e.Key == Key.D8)
                        {
                            _isBarcodeReading = false;
                            //DataGridSaleItems.Add(mScanData.ToString());
                        }
                        else
                        {
                            var xChar = new KeyConverter().ConvertToString(e.Key);
                            if (!_scanShiftDown)
                            {
                                if (xChar != null) xChar = xChar.ToLower();
                            }

                            CurrentBarcode += xChar;

                        }
                    }
                    else
                    {
                        if (_leftShiftDown && e.Key == Key.D8)
                        {
                            _isBarcodeReading = true;
                            CurrentBarcode = null;
                            _scanShiftDown = false;
                            e.Handled = true;
                        }
                    }
                    break;
            }
        }

        private bool _leftShiftDown;
        private bool _scanShiftDown;
        public bool IsViewFocused { get; set; }
        private bool IsPaymentComplete { get; set; }
        private bool _isBarcodeReading;

        private bool _isMember;
        public bool IsMember
        {
            get { return _isMember; }
            set
            {
                if (value != _isMember)
                {
                    _isMember = value;
                    OnPropertyChanged();

                    CalculateMemberDiscount();
                    TotalAmount = GetTotalAmount();
                }
            }
        }

        private void SetPaymentComplete(bool value)
        {
            IsPaymentComplete = value;


            if (!IsPaymentComplete) return;

            var transaction = AddTransaction();
            DataGridSaleItems.Clear();
            StoreCreditTrans = null;
            TotalAmount = 0;

        }

        public ActionCommand AddItemCommand
        {
            get
            {
                return new ActionCommand(f => AddItem());
            }
        }

        public void AddItem()
        {
        }

        public ActionCommand PmtCommand
        {
            get
            {
                return new ActionCommand(f => ProcessPayment());
            }
        }

        public void ProcessPayment()
        {
            var paymentWindowVm = new PaymentWindowVm(DataGridSaleItems);
            WindowService.ShowWindow(paymentWindowVm);
        }

        public ActionCommand CancelCommand
        {
            get
            {
                return new ActionCommand(f => CancelPayment());
            }
        }

        public void CancelPayment()
        {
        }

        public ActionCommand DeleteSelectedCommand
        {
            get
            {
                return new ActionCommand(f => DeleteSelected());
            }
        }

        public void DeleteSelected()
        {
            //Get selected item
            var saleItem = SelectedSaleItem;

            //Copy datagrid to temp
            var tempItems = DataGridSaleItems;

            //Get the index of the current item
            var gridIndex = tempItems.IndexOf(saleItem);

            //Adjust index if index is last index in grid
            if ((gridIndex + 1) == tempItems.Count)
                gridIndex--;

            //Insert back into querycollection
            var list = QueryCollection.ToList();
            list.Add(new BarcodeItem(saleItem.ConvertSaleItemToItem()));
            QueryCollection = list;

            //Remove from temp
            tempItems.Remove(saleItem);

            //Assign temp to datagrid
            DataGridSaleItems = tempItems;

            //Enable or disable Payment button
            IsEnabledPmt = DataGridSaleItems.FirstOrDefault() != null;

            //Assign new index
            SelectedSaleItem = DataGridSaleItems.ElementAtOrDefault(gridIndex);

            TotalAmount = GetTotalAmount();
        }

        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        {
            get
            {
                return new RelayCommand<DataGridCellEditEndingEventArgs>(CellEdit);
            }
        }

        private void CellEdit(DataGridCellEditEndingEventArgs e)
        {

        }

        // <summary>
        // Gets or sets the CurrentBarcode.
        // </summary>
        private string _currentBarcode;
        public string CurrentBarcode
        {
            get { return _currentBarcode; }
            set
            {
                _currentBarcode = value;

                //Add to datagrid if 15 characters long
                if (_currentBarcode != null)
                {
                    if (_currentBarcode.Length == 15)
                    {

                        var saleItem = new SaleItem(_currentBarcode, IsMember);
                        DataGridSaleItems.Add(saleItem);
                        IsEnabledPmt = DataGridSaleItems.FirstOrDefault() != null;
                        TotalAmount = GetTotalAmount();
                        _currentBarcode = null;
                    }
                }
                OnPropertyChanged();
            }
        }

        // <summary>
        // Gets or sets the TotalAmountText.
        // </summary>
        private double _totalAmount;
        public double TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value;
                OnPropertyChanged();

            }
        }

        // <summary>
        // Gets or sets the CityTax for a transaction.
        // </summary>
        public double CityTax { get; set; }

        // <summary>
        // Gets or sets the StateTax for a transaction.
        // </summary>
        public double StateTax { get; set; }

        // <summary>
        // Gets or sets the CountyTax for a transaction.
        // </summary>
        public double CountyTax { get; set; }

        // <summary>
        // Gets or sets the Discount for a transaction.
        // </summary>
        public double Discount { get; set; }

        private bool _isEnabledPmt;
        public bool IsEnabledPmt
        {
            get
            {
                return _isEnabledPmt;
            }
            set
            {
                _isEnabledPmt = value;
                OnPropertyChanged();
            }
        }

        private StoreCreditTransaction _storeCreditTrans;
        private StoreCreditTransaction StoreCreditTrans
        {
            get { return _storeCreditTrans; }
            set
            {
                if (value != null)
                    _storeCreditTrans = new StoreCreditTransaction();
                _storeCreditTrans = value;
            }
        }

        private double GetTotalAmount()
        {
            return DataGridSaleItems.Sum(item => item.LinePrice);
        }

        private ItemSaleTransaction AddTransaction()
        {

            //Convert DataGridSaleItems to Item list
            var icontroller = new ItemController();
            var itemList = DataGridSaleItems.Select(item => icontroller.GetItemById(item.Id)).ToList();


            //Get city, state, county tax amount from DataGridSaleItems
            CityTax = DataGridSaleItems.Sum(item => item.CityTaxAmount);
            StateTax = DataGridSaleItems.Sum(item => item.StateTaxAmount);
            CountyTax = DataGridSaleItems.Sum(item => item.CountyTaxAmount);

            //Get discount total from DataGridSaleItems
            Discount = DataGridSaleItems.Sum(item => item.DiscountAmount);

            var transaction = new ItemSaleTransaction()
            {
                CreditTransaction_ItemSale = new CreditTransaction()
                {
                    TransactionDate = DateTime.Now,
                    TransactionTotal = TotalAmount,
                    LocalSalesTaxTotal = CityTax,
                    StateSalesTaxTotal = StateTax,
                    CountySalesTaxTotal = CountyTax,
                    DiscountTotal = Discount,
                    StoreCreditTransaction = StoreCreditTrans
                },

                Items_ItemSaleTransaction = itemList
            };

            try
            {
                var iscontroller = new ItemSaleTransactionController();
                iscontroller.AddNewItemSaleTransaction(transaction);
                ProcessConsignorPayment(itemList);

                foreach (var saleItem in DataGridSaleItems)
                {
                    var tempItem = saleItem.ConvertSaleItemToItem();
                    var item = icontroller.GetItemById(tempItem.Id);
                    item.SalePrice = saleItem.SaleAmount;
                    icontroller.UpdateItem(item);
                }

                return transaction;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AddTransaction - ItemSaleVm");
                return null;
            }


        }

        private void ProcessConsignorPayment(List<Item> itemsList)
        {
            //Create a PaymentTransaction which includes date, amount, consignor, items

            foreach (var item in itemsList)
            {
                //var ccontroller = new ConsignorController();
                var consignor = item.Consignor;
                //Create StoreCreditPmt if cash is not checked
                var storeCredit = new StoreCreditPmt();

                //Get Member Info
                var c = new MemberController();
                var member = c.GetMemberByName(consignor.Consignor_Person.FirstName, consignor.Consignor_Person.LastName);


                var ndFee = 0.0;
                var gridItem = DataGridSaleItems.FirstOrDefault(i => i.Id == item.Id);
                double thisItemsSoldPrice = 0;
                if (gridItem != null)
                {
                    var dateSpan = DateTimeSpan.CompareDates(gridItem.ListedDate, DateTime.Now);
                    if (member == null) //No Member Discount
                    {
                        if (gridItem.IsDiscountable || (dateSpan.Months < 3))
                            thisItemsSoldPrice = (gridItem.UnitPrice - (gridItem.DiscountAmount))*
                                                 ConfigSettings.CONS_CREDIT_PAYOUT_PCT;
                        else
                        {
                            thisItemsSoldPrice = (gridItem.UnitPrice - (gridItem.DiscountAmount))*
                                                 ConfigSettings.CONS_CREDIT_PAYOUT_PCT - ConfigSettings.ND_FEE;
                            ndFee = ConfigSettings.ND_FEE;
                        }
                    }
                    else if (member != null) 
                    {
                        if ((member.StartDate <= DateTime.Now) &&
                            (member.RenewDate >= DateTime.Now)) //Give Member Discount
                        {
                            if (gridItem.IsDiscountable || (dateSpan.Months < 3))
                                thisItemsSoldPrice = (gridItem.UnitPrice - (gridItem.DiscountAmount)) *
                                                     ConfigSettings.MEMBER_CONS_CREDIT_PAYOUT_PCT;
                            else
                            {
                                thisItemsSoldPrice = (gridItem.UnitPrice - (gridItem.DiscountAmount)) *
                                                     ConfigSettings.MEMBER_CONS_CREDIT_PAYOUT_PCT - ConfigSettings.ND_FEE;
                                ndFee = ConfigSettings.ND_FEE;
                            }
                        }
                        else //No Member Discount
                        {
                            if (gridItem.IsDiscountable || (dateSpan.Months < 3))
                                thisItemsSoldPrice = (gridItem.UnitPrice - (gridItem.DiscountAmount)) *
                                                     ConfigSettings.CONS_CREDIT_PAYOUT_PCT;
                            else
                            {
                                thisItemsSoldPrice = (gridItem.UnitPrice - (gridItem.DiscountAmount)) *
                                                     ConfigSettings.CONS_CREDIT_PAYOUT_PCT - ConfigSettings.ND_FEE;
                                ndFee = ConfigSettings.ND_FEE;
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Item not found in DataSalesGrid - can't continue");
                }
                storeCredit.StoreCreditPmtAmount = thisItemsSoldPrice;
                storeCredit.ConsignorId = consignor.Id;


                var consignorPmt = new ConsignorPmt
                {
                    //Consignor Info
                    ConsignorId = consignor.Id,

                    NoDiscountFee = ndFee,

                    //DebitTransaction Info
                    DebitTransaction_ConsignorPmt = new DebitTransaction
                    {
                        DebitTotal = thisItemsSoldPrice,
                        DebitTransactionDate = DateTime.Now,
                        StoreCreditPmt = storeCredit,
                    },

                    //Items info
                    Items_ConsignorPmt = new List<Item>(){ item },
                };

                try
                {
                    var cpcontroller = new ConsignorPmtController();
                    cpcontroller.AddNewConsignorPmt(consignorPmt);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Exception adding Consignor Payment in ProcessConsignorPayment method in ItemSaleVm class - " +
                        ex.Message);
                }
            }





            // Display status of transaction on screen
            // Send message to open cash drawer

        }
        #endregion

        #region DataGrid Stuff
        /// <summary>
        /// Gets the collection of SaleItem entities.
        /// </summary>
        private TrulyObservableCollection<SaleItem> _dataGridSaleItems;
        public TrulyObservableCollection<SaleItem> DataGridSaleItems
        {
            get { return _dataGridSaleItems; }

            set
            {
                _dataGridSaleItems = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the selectedItem row from the datagrid.
        /// </summary>
        private SaleItem _selectedSaleItem;
        public SaleItem SelectedSaleItem
        {
            get { return _selectedSaleItem; }
            set
            {
                _selectedSaleItem = value;
                //OnSelected();
            }
        }
        #endregion

        #region AutoComplete TextBox

        private IEnumerable<BarcodeItem> _queryCollection;
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
            set
            {
                _queryCollection = value;
                OnPropertyChanged();
            }
        }

        private static IEnumerable<BarcodeItem> InitializeBarcodeItems()
        {
            try
            {
                var barcodes = new List<BarcodeItem>();
                var controller = new ItemController();
                //var results = controller.GetAllItems();
                var results = controller.SearchAllItems(null, "Shelved", null, null, null).ToList();
                results.AddRange((controller.SearchAllItems(null, "Lost", null, null, null)));
                results.AddRange((controller.SearchAllItems(null, "Arrived but not shelved", null, null, null)));
                results.AddRange((controller.SearchAllItems(null, "Not yet arrived in store", null, null, null)));

                barcodes.AddRange(results.Select(item => new BarcodeItem(item)));

                return barcodes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "InitializeBarcodeItemsException - POS.ItemSaleVm");
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

            if (String.IsNullOrEmpty(search))
            {
                SelectedBarcodeItem = null;
            }

            return (result);

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
                if ((_selectedBarcodeItem != null) && !_isBarcodeReading)
                {
                    if (_selectedBarcodeItem.BarcodeItemBc.Length != 15) return;
                    CurrentBarcode = _selectedBarcodeItem.BarcodeItemBc;
                    if ((_selectedBarcodeItem.Id != 915) && (_selectedBarcodeItem.Id != 916) && (_selectedBarcodeItem.Id != 79)) //Keep dollar items
                    {
                        _selectedBarcodeItem.BarcodeItemBc = String.Empty;
                    }
                    else
                    {
                        _selectedBarcodeItem = null;
                    }

                }
                else
                {
                    CurrentBarcode = null;
                }
            }
        }

        public void CalculateMemberDiscount()
        {
            //DataGridSaleItems.Clear();
            var tempGrid = new TrulyObservableCollection<SaleItem>();
            
            foreach (var bi in DataGridSaleItems)
            {
                //Get member, volunteer, owner discount
                bi.ComputeMemberDiscount(IsMember);

                //Compute taxes and linePrice
                bi.ComputeTaxesAndLinePrice();

                tempGrid.Add(bi);
            }

            DataGridSaleItems = tempGrid;
        }

        #endregion
        
    }
}