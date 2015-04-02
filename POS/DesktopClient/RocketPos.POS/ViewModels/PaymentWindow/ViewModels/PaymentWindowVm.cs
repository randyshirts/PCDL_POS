// -----------------------------------------------------------------------------
//  <copyright file="CustomerViewModel.cs" company="DCOM Engineering, LLC">
//      Copyright (c) DCOM Engineering, LLC.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RocketPos.Common.Foundation;
using RocketPos.Common.Helpers;
using POS.Controller.CustomClasses;
using POS.Controller.Elements;
using POS.Controller.Visitors;
using MessageBox = System.Windows.MessageBox;


namespace POS.ViewModels.PaymentWindow.ViewModels
{
    /// <summary>
    /// A View-Model that represents a Consignor and its state information.
    /// </summary>
    public class PaymentWindowVm : ViewModel
    {
        #region ctors and guid
        public static readonly Guid Token = Guid.NewGuid(); //So others know messages came from this instance
        private readonly Collection<SaleItem> _saleItems = new Collection<SaleItem>();
        

        public PaymentWindowVm(IEnumerable<SaleItem> items)
        {
            _saleItems = items as Collection<SaleItem>;
            
            TotalAmount = _saleItems.Sum(f => f.LinePrice);

            IsCreditDebitVisible = Visibility.Hidden.ToString();
            IsStoreCreditVisible = Visibility.Hidden.ToString();
            IsCashVisible = Visibility.Hidden.ToString();
            IsPrintingVisible = Visibility.Hidden.ToString();
            IsSplitVisible = Visibility.Hidden.ToString();
            IsCashFocused = false;

            var controller = new ConsignorController();
            var creditNameComboList = new List<string>(controller.GetConsignorNamesLastnameFirst());
            _creditNameComboValues.InitializeComboBox(creditNameComboList);
            
            
        }
        #endregion

        #region left-hand side menu
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
        
        public ActionCommand CashCommand
        {
            get
            {
                return new ActionCommand(f => PayWithCash());
            }
        }

        public void PayWithCash()
        {
            IsCashVisible = Visibility.Visible.ToString();
            IsCreditDebitVisible = Visibility.Hidden.ToString();
            IsStoreCreditVisible = Visibility.Hidden.ToString();
            IsSplitVisible = Visibility.Hidden.ToString();
        }

        public ActionCommand CreditCommand
        {
            get
            {
                return new ActionCommand(f => PayWithCredit());
            }
        }

        public void PayWithCredit()
        {
            IsCreditDebitVisible = Visibility.Visible.ToString();
            IsCashVisible = Visibility.Hidden.ToString();
            IsStoreCreditVisible = Visibility.Hidden.ToString();
            IsSplitVisible = Visibility.Hidden.ToString();

            CreditDebitTransactionText = "Credit Transaction";
        }

        public ActionCommand DebitCommand
        {
            get
            {
                return new ActionCommand(f => PayWithDebit());
            }
        }

        public void PayWithDebit()
        {
            IsCreditDebitVisible = Visibility.Visible.ToString();
            IsCashVisible = Visibility.Hidden.ToString();
            IsStoreCreditVisible = Visibility.Hidden.ToString();
            IsSplitVisible = Visibility.Hidden.ToString();

            CreditDebitTransactionText = "Debit Transaction";
        }

        public ActionCommand StoreCreditCommand
        {
            get
            {
                return new ActionCommand(f => PayWithStoreCredit());
            }
        }

        public void PayWithStoreCredit()
        {
            IsCreditDebitVisible = Visibility.Hidden.ToString();
            IsCashVisible = Visibility.Hidden.ToString();
            IsSplitVisible = Visibility.Hidden.ToString();
            IsStoreCreditVisible = Visibility.Visible.ToString();
        }

        public ActionCommand SplitTransactionCommand
        {
            get
            {
                return new ActionCommand(f => PayWithSplit());
            }
        }

        public void PayWithSplit()
        {
            IsCreditDebitVisible = Visibility.Hidden.ToString();
            IsCashVisible = Visibility.Hidden.ToString();
            IsStoreCreditVisible = Visibility.Hidden.ToString();
            IsSplitVisible = Visibility.Visible.ToString();
        }

        public RelayCommand<Object> CancelTransactionCommand
        {
            get
            {
                return new RelayCommand<Object>(CancelTransaction);
            }
        }

        public void CancelTransaction(Object control)
        {
            WindowService.CloseWindow((Window)control);
        }      
        #endregion

        #region Credit/Debit Properties and Methods
        private string _isCreditDebitVisible;
        public string IsCreditDebitVisible
        {
            get
            {
                return _isCreditDebitVisible;
            }
            set
            {
                _isCreditDebitVisible = value;
                OnPropertyChanged();
            }
        }

        private double _creditDebitAmount;
        public double CreditDebitAmount
        {
            get { return _creditDebitAmount; }
            set
            {
                _creditDebitAmount = value;
                OnPropertyChanged();
            }
        }

        private string _creditDebitTransactionText;
        public string CreditDebitTransactionText
        {
            get { return _creditDebitTransactionText; }
            set
            {
                _creditDebitTransactionText = value;
                OnPropertyChanged();
            }
        }

        public ActionCommand DoneCreditDebitCommand
        {
            get
            {
                return new ActionCommand(f => DoneCreditDebit());
            }
        }

        public void DoneCreditDebit()
        {
            IsCreditDebitVisible = Visibility.Hidden.ToString();
            IsPrintingVisible = Visibility.Visible.ToString();

            CreditDebitAmount = TotalAmount;

            PrintReceipt();
        }

        public ActionCommand CancelCreditDebitCommand
        {
            get
            {
                return new ActionCommand(f => CancelCreditDebit());
            }
        }

        public void CancelCreditDebit()
        {
            IsCreditDebitVisible = Visibility.Hidden.ToString();
        }
        #endregion

        #region Printer Properties and Methods
        private string _isPrintingVisible;
        public string IsPrintingVisible
        {
            get { return _isPrintingVisible; }
            set
            {
                _isPrintingVisible = value;
                OnPropertyChanged();
            }
        }
        
        public RelayCommand<object> PrintDoneCommand
        {
            get
            {
                return new RelayCommand<object>(PrintDone);
            }
        }

        public void PrintDone(object obj)
        {
            if(StoreCreditTr != null)
                Messenger.Default.Send(new PropertySetter("StoreCreditTr", StoreCreditTr), Token);
            
            Messenger.Default.Send(new PropertySetter("IsPaymentComplete", true), Token);
            WindowService.CloseWindow(obj as Window);
        }

        public ActionCommand PrintCommand
        {
            get
            {
                return new ActionCommand(f => PrintReceipt());
            }
        }

        public void PrintReceipt()
        {
            const string printerAddress = "\\\\Randy-PC\\POS-58";
            const string printerName = "POS-58";
            
            //Set Printer to Receipt Printer
            //ReceiptPrinterHelper.FindPrinter(printerName);

            //Format Receipt
            var sb = new StringBuilder();

            //Header
            sb.Append(CreateReceipt.CreateHeader());

            //Body
            var receiptVisitor = new CreateReceiptVisitor();
            foreach (var item in _saleItems)
            {
                sb.Append(item.Accept(receiptVisitor));
            }

            //Footer
            sb.Append(CreateReceipt.CreateFooter(_saleItems.Sum(f => (f.UnitPrice - f.DiscountAmount)), 
                                                 _saleItems.FirstOrDefault().Tax, 
                                                 _saleItems.Sum(f => f.TaxAmount),
                                                 CashReceived,
                                                 Change,
                                                 StoreCreditCharged,
                                                 CreditBalanceAmount - StoreCreditCharged,
                                                 CreditDebitAmount));

            //Print document
            ReceiptPrinterHelper.PrintText(sb.ToString());
        }

        #endregion

        #region Store Credit Properties and Methods
        private string _isStoreCreditVisible;
        public string IsStoreCreditVisible
        {
            get
            {
                return _isStoreCreditVisible;
            }
            set
            {
                _isStoreCreditVisible = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets the itemTypeComboValues list.
        /// </summary>
        private readonly ComboBoxListValues _creditNameComboValues = new ComboBoxListValues();
        public List<ComboBoxListValues> CreditNameComboValues
        {
            get { return _creditNameComboValues.ComboValues; }
        }

        /// <summary>
        /// Gets or sets the CreditName - the consignor name for store credit.
        /// </summary>
        private string _creditName;
        public string CreditName
        {
            get { return _creditName; }
            set
            {
                _creditName = value;
                SearchConsignorCredit();
                OnPropertyChanged();
            }
        }

        private double _creditBalanceAmount;
        public double CreditBalanceAmount
        {
            get { return _creditBalanceAmount; }
            set
            {
                _creditBalanceAmount = Math.Ceiling(value*100)/100;
                OnPropertyChanged();

            }
        }

        public StoreCreditTransaction StoreCreditTr { get; set; }
    
        public ActionCommand SubmitStoreCreditCommand
        {
            get{return new ActionCommand(f => ProcessStoreCreditPayment());}
        }

        private void ProcessStoreCreditPayment()
        {
            if (CreditBalanceAmount >= TotalAmount)
            {
                IsPrintingVisible = Visibility.Visible.ToString();
                IsStoreCreditVisible = Visibility.Hidden.ToString();

                StoreCreditCharged = TotalAmount;
                CreateStoreConsignorTr(TotalAmount);

                PrintReceipt();
            }
            else
            {
                MessageBox.Show("Insufficient payment - the amount due is greater than the amount available in Store Credit. Choose 'Split Transaction' instead.");
            }
        }

        public ActionCommand CancelStoreCreditCommand
        {
            get { return new ActionCommand(f => CancelStoreCreditPayment()); }
        }

        private void CancelStoreCreditPayment()
        {
            CreditName = null;
            CreditBalanceAmount = 0;
            IsStoreCreditVisible = Visibility.Hidden.ToString();
        }
        /// <summary>
        /// Gets the command that allows a consignor's items to be searched.
        /// </summary>
        private void SearchConsignorCredit()
        {
            var controller = new ConsignorController();
            CreditBalanceAmount = controller.GetConsignorCreditBalance(CreditName);
        }

        private void CreateStoreConsignorTr(double amount)
        {
            int id;
            
                //Split full name
                if (String.IsNullOrEmpty(CreditName)) return;
                var names = CreditName.Split(' ');
                var lastName = names[0];
                var firstName = names[1];

            var controller = new ConsignorController();
            id = controller.GetConsignorByName(firstName, lastName).Id;
            
            StoreCreditTr = new StoreCreditTransaction()
            {
                ConsignorId = id,
                StoreCreditTransactionAmount = amount
            };
        }
        #endregion
        
        #region Cash Payment Properties 
        private string _isCashVisible;
        public string IsCashVisible
        {
            get
            {
                return _isCashVisible;
            }
            set
            {
                _isCashVisible = value;
                OnPropertyChanged();
                IsCashFocused = _isCashVisible == Visibility.Visible.ToString();
            }
        }

        private bool _isCashFocused;
        public bool IsCashFocused
        {
            get
            {
                return _isCashFocused;
            }
            set
            {
                _isCashFocused = value;
                OnPropertyChanged();
            }
        }
        
        private double _cashReceived;
        public double CashReceived
        {
            get { return _cashReceived;}
            set
            {
                _cashReceived = value;
                if (_isCashVisible == Visibility.Visible.ToString())
                    Change = _cashReceived - TotalAmount;
                if (_isSplitVisible == Visibility.Visible.ToString())
                {
                    CreditDebitAmount = TotalAmount - _cashReceived - _storeCreditCharged;
                    if (CreditDebitAmount < 0)
                    {
                        SplitChangeDue = CreditDebitAmount*-1;
                        CreditDebitAmount = 0;
                    }
                
                }
                OnPropertyChanged();
            }
        }

        private double _change;
        public double Change
        {
            get { return _change; }
            set
            {
                _change = value;
                OnPropertyChanged();
            }
        }

        public ActionCommand CancelCashCommand
        {
            get
            {
                return new ActionCommand(f => CancelCashTransaction());
            }
        }

        public void CancelCashTransaction()
        {
            IsCashVisible = Visibility.Hidden.ToString();
        }

        public ActionCommand SubmitCashCommand
        {
            get
            {
                return new ActionCommand(f => SubmitCashTransaction());
            }
        }

        public void SubmitCashTransaction()
        {
            if (Change >= 0)
            {
                IsPrintingVisible = Visibility.Visible.ToString();
                IsCashVisible = Visibility.Hidden.ToString();
                IsCreditDebitVisible = Visibility.Hidden.ToString();

                PrintReceipt();
            }
            else
            {
                MessageBox.Show("Insufficient payment - the amount due is greater than the amount received");
            }
        }
#endregion
        
        #region Split Transaction Properties

        private string _isSplitVisible;
        public string IsSplitVisible
        {
            get { return _isSplitVisible; }
            set
            {
                _isSplitVisible = value;
                OnPropertyChanged();
                IsSplitCashFocused = _isSplitVisible == Visibility.Visible.ToString();
            }
        }

        private bool _isSplitCashFocused;
        public bool IsSplitCashFocused
        {
            get
            {
                return _isSplitCashFocused;
            }
            set
            {
                _isSplitCashFocused = value;
                OnPropertyChanged();
            }
        }

        private double _storeCreditCharged;
        public double StoreCreditCharged
        {
            get { return _storeCreditCharged; }
            set
            {
                if (value <= CreditBalanceAmount)
                {
                    _storeCreditCharged = value;
                    CreditDebitAmount = TotalAmount - CashReceived - _storeCreditCharged;
                    if (CreditDebitAmount < 0)
                    {
                        SplitChangeDue = -1*CreditDebitAmount;
                        CreditDebitAmount = 0;
                    }
                    OnPropertyChanged();
                }
                else
                {
                    MessageBox.Show("Enter an amount less than or equal to the available store credit");
                }
            }
        }

        private double _splitChangeDue;
        public double SplitChangeDue
        {
            get { return _splitChangeDue; }
            set
            {
                _splitChangeDue = value;
                OnPropertyChanged();
            }
        }

        public ActionCommand SubmitSplitCommand
        {
            get { return new ActionCommand(f => ProcessSplitCommand());}
        }

        private void ProcessSplitCommand()
        {
            

            if (CreditDebitAmount > 0)
            {
                if (MessageBox.Show("Process Credit Card Transaction Now: " + CreditDebitAmount.ToString("C2")
                                    + "\n\n Click 'Yes' if card transaction completed successfully."
                                    + "\n Click 'No' to cancel.", "Process Credit/Debit Card", MessageBoxButton.YesNo) ==
                    MessageBoxResult.Yes)
                {
                    IsSplitVisible = Visibility.Hidden.ToString();
                    IsPrintingVisible = Visibility.Visible.ToString();

                    if (StoreCreditCharged > 0)
                        CreateStoreConsignorTr(StoreCreditCharged);
                }
                else
                {
                    return;
                }
            }

            if (SplitChangeDue > 0)
            {
                Change = SplitChangeDue;
            }

            IsSplitVisible = Visibility.Hidden.ToString();
            IsPrintingVisible = Visibility.Visible.ToString();

            if(StoreCreditCharged > 0)
                CreateStoreConsignorTr(StoreCreditCharged);

            PrintReceipt();
        }

        public ActionCommand CancelSplitCommand
        {
            get { return new ActionCommand(f => CancelSplit()); }
        }

        private void CancelSplit()
        {
            CashReceived = 0;
            SplitChangeDue = 0;
            Change = 0;
            CreditBalanceAmount = 0;
            CreditDebitAmount = 0;
            CreditName = null;
            IsSplitVisible = Visibility.Hidden.ToString();
        }
        #endregion
    }
}