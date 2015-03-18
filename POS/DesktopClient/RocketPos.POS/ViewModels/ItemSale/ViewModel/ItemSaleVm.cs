// -----------------------------------------------------------------------------
//  <copyright file="CustomerViewModel.cs" company="DCOM Engineering, LLC">
//      Copyright (c) DCOM Engineering, LLC.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

            Messenger.Default.Register<PropertySetter>(this, PaymentWindowVm.Token, msg => GetPaymentWindowProperty(msg.PropertyName, msg.PropertyValue)); 
            
        }

        private void GetPaymentWindowProperty(string name, object value)
        {
            if(name == "IsPaymentComplete")
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

                //Add to datagrid if 13 characters long
                if (_currentBarcode != null)
                {
                    if (_currentBarcode.Length == 13)
                    {
                        
                        var saleItem = new SaleItem(_currentBarcode);
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
        private StoreCreditTransaction StoreCreditTrans {
            get { return _storeCreditTrans; }
            set
            {
                if(value != null)
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
                        if (_selectedBarcodeItem.BarcodeItemBc.Length != 13) return;
                        CurrentBarcode = _selectedBarcodeItem.BarcodeItemBc;
                        _selectedBarcodeItem.BarcodeItemBc = String.Empty;
                        
                    }
                    else
                    {
                        CurrentBarcode = null;
                    }
                }
            }
            #endregion
        //    private void OnSelected()
        //    {
        //        //if (SelectedConsignor != null)
        //        //    Messenger.Default.Send(new PropertySetter("GameImage", SelectedGameItem.GameImage), Token);
        //    }

        //    /// <summary>
        //    /// Gets the command that allows a cell to be updated.
        //    /// </summary>
        //    public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        //    {
        //        get
        //        {
        //            return new RelayCommand<DataGridCellEditEndingEventArgs>(UpdateSaleItem);
        //        }
        //    }

        //    /// <summary>
        //    /// Gets the command that allows a row to be deleted.
        //    /// </summary>
        //    public RelayCommand DeleteSelectedCommand
        //    {
        //        get
        //        {
        //            return new RelayCommand(DeleteSaleItem);
        //        }
        //    }

        //    private void DeleteSaleItem()
        //    {

        //        if (MessageBox.Show("Deletions are permanent. Are you sure you want to delete this item? ", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
        //            return;

        //        if (null == SelectedSaleItem) return;
        //        var consignorItem = DataGridSaleItems.FirstOrDefault(ci => ci.Consignor_Item.Id == SelectedSaleItem.Consignor_Item.Id);
        //        if (consignorItem == null) return;
        //        var tempConsignors = DataGridSaleItems;
        //        var gridIndex = tempConsignors.IndexOf(consignorItem);
        //        if ((gridIndex + 1) == tempConsignors.Count)
        //            gridIndex--;
        //        tempConsignors.Remove(consignorItem);
        //        DataGridSaleItems = tempConsignors;
        //        SelectedSaleItem = DataGridSaleItems.ElementAtOrDefault(gridIndex);

        //        using (var bc = new BusinessContext())
        //        {
        //            bc.DeleteItemById(consignorItem.Consignor_Item.Id);
        //        }
        //    }

        //    private void UpdateSaleItem(DataGridCellEditEndingEventArgs e)
        //    {


        //            var text = WpfHelpers.GetTextFromCellEditingEventArgs(e);

        //            //Get the Column header
        //            var column = e.Column.Header.ToString();

        //            //Add info to new ConsignorInfo
        //            var consignorItem = e.Row.Item as SaleItem;
        //            var success = consignorItem != null && consignorItem.SetProperty(column, text);

        //            if (SaleItemIsValid(consignorItem) && success)
        //            {
        //                var updateVisitor = new ItemUpdateVisitor();
        //                consignorItem.Accept(updateVisitor);
        //            }
        //            else
        //            {
        //                e.Cancel = true;
        //                if (consignorItem != null) consignorItem.ReportError(column, text);
        //            }

        //    }

        //    private static bool SaleItemIsValid(SaleItem consignorItem)
        //    {
        //        if (consignorItem.ItemType == "Book")
        //            return !String.IsNullOrWhiteSpace(consignorItem.Title) &&
        //                    (consignorItem.Title.Length <= 150) &&
        //                    !String.IsNullOrWhiteSpace(consignorItem.IsbnEan) &&
        //                    ((consignorItem.IsbnEan.Length == 13) || (consignorItem.IsbnEan.Length == 10)) &&
        //                    !String.IsNullOrWhiteSpace(consignorItem.ItemStatus) &&
        //                    !String.IsNullOrWhiteSpace(consignorItem.ListedPrice) &&
        //                    Validation_Helper.ParseCurrency(consignorItem.ListedPrice) &&
        //                    Validation_Helper.ParseCurrency(consignorItem.SoldPrice);
        //        if (!String.IsNullOrWhiteSpace(consignorItem.IsbnEan))
        //            return !String.IsNullOrWhiteSpace(consignorItem.Title) &&
        //                   (consignorItem.Title.Length <= 150) &&
        //                   (consignorItem.IsbnEan.Length == 13) &&
        //                   !String.IsNullOrWhiteSpace(consignorItem.ItemStatus) &&
        //                   !String.IsNullOrWhiteSpace(consignorItem.ListedPrice) &&
        //                   Validation_Helper.ParseCurrency(consignorItem.ListedPrice) &&
        //                   Validation_Helper.ParseCurrency(consignorItem.SoldPrice);
        //        return !String.IsNullOrWhiteSpace(consignorItem.Title) &&
        //               (consignorItem.Title.Length <= 150) &&
        //               !String.IsNullOrWhiteSpace(consignorItem.ItemStatus) &&
        //               !String.IsNullOrWhiteSpace(consignorItem.ListedPrice) &&
        //               Validation_Helper.ParseCurrency(consignorItem.ListedPrice) &&
        //               Validation_Helper.ParseCurrency(consignorItem.SoldPrice);
        //    }

        //    private void InitializeDataGrid(ObservableCollection<Item> items)
        //    {

        //            //Clear DataGridSaleItems
        //            DataGridSaleItems.Clear();

        //            DataGridSaleItems = CreateSaleItemsList(items);

        //    }

        //    //Make a collection of SaleItems with info from the Items Table
        //    private TrulyObservableCollection<SaleItem> CreateSaleItemsList(IEnumerable<Item> items)
        //    {
        //        //Temp Collection Definition
        //        var tempSaleItems = new TrulyObservableCollection<SaleItem>();

        //        //Clear DataGridSaleItems
        //        DataGridSaleItems.Clear();

        //        //Make the collection
        //        var i = items.GetEnumerator();
        //        while (i.MoveNext())
        //        {
        //            //Create an instance of a ConsignorInfo with the current consignor and person info
        //            var currentSaleItem = new SaleItem(i.Current);
        //            //Add the new instance to the collection
        //            tempSaleItems.Add(currentSaleItem);
        //        }

        //        return tempSaleItems;
        //    }

        //    public TrulyObservableCollection<SaleItem> UpdateConsignorPortion(bool cash)
        //    {
        //        var consignorItems = DataGridSaleItems;

        //        double totalPayment = 0;

        //        foreach (var ci in consignorItems)
        //        {
        //            double currentPmt;
        //            if ((ci.SoldPrice != null) && (ci.ItemStatus == "Sold"))
        //            {
        //                if (cash)
        //                {
        //                    currentPmt = (double.Parse(ci.SoldPrice, NumberStyles.Currency) * ConfigSettings.CONS_CASH_PAYOUT_PCT);
        //                }
        //                else
        //                {
        //                    currentPmt = (double.Parse(ci.SoldPrice, NumberStyles.Currency) * ConfigSettings.CONS_CREDIT_PAYOUT_PCT);
        //                }

        //                ci.ConsignorPortion = currentPmt.ToString("C2");
        //                totalPayment += currentPmt;
        //            }
        //            else if ((ci.SoldPrice == null) && (ci.ItemStatus == "Sold"))
        //            {
        //                currentPmt = 0;
        //                ci.ConsignorPortion = currentPmt.ToString("C2");
        //                totalPayment += currentPmt;
        //            }
        //        }

        //        PayoutAmountText = totalPayment.ToString("C2");

        //        return consignorItems;
        //    }

        //    public TrulyObservableCollection<SaleItem> ClearConsignorPortion()
        //    {
        //        var consignorItems = DataGridSaleItems;

        //        foreach (var ci in consignorItems)
        //        {
        //            ci.ConsignorPortion = null;
        //        }

        //        PayoutAmountText = "$0";

        //        return consignorItems;
        //    }
            #endregion
        //}
    }
}