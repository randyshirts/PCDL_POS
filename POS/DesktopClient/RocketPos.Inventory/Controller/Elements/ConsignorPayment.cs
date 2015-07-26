using System;
using DataModel.Data.DataLayer.Entities;
using RocketPos.Common.Foundation;


namespace Inventory.Controller.Elements
{
    public class ConsignorPayment : ObservableObjects
    {
        public ConsignorPayment()
        { }

        public ConsignorPayment(StoreCreditTransaction transaction)
        {
            Consignor = transaction.Consignor_StoreCreditTransaction;

            //Item info
            Id = transaction.Id;
            ConsignorId = transaction.ConsignorId;
            Transaction = transaction;
            TransactionAmount = transaction.StoreCreditTransactionAmount;
            TransactionType = transaction.TransactionType;
            TransactionDate = transaction.CreditTransaction_StoreCredit.TransactionDate;
        }


        //Define members
        public int Id { get; set; }
        public int ConsignorId { get; set; }
        public DateTime TransactionDate { get; set; }
        public Consignor Consignor { get; set; }
        public StoreCreditTransaction Transaction { get; set; }
        public string TransactionType { get; set; }
        public double TransactionAmount { get; set; }
        
        private string _consignorPortion;
        public string ConsignorPortion 
        {
            get { return _consignorPortion; } 
            set
            {
                _consignorPortion = value;
                OnPropertyChanged();
            } 
        }    
    
    }
}
