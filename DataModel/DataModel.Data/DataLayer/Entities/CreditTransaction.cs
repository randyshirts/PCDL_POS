using System;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class CreditTransaction : Entity
    {

        #region Define Members
        //Define members
        //public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionTotal { get; set; }
        public double StateSalesTaxTotal { get; set; }
        public double LocalSalesTaxTotal { get; set; }
        public double CountySalesTaxTotal { get; set; }
        public double DiscountTotal { get; set; }
        //public virtual ICollection<Item> Items_Transaction { get; set; } -- goes in ItemSaleTransaction, ClassPmt, etc
        //Relationships - keys
        public int? ItemSaleTransactionId { get; set; }
        public virtual ItemSaleTransaction ItemSaleTransaction { get; set; }
        public int? ClassPmtTransactionId { get; set; }
        public virtual ClassPmtTransaction ClassPmtTransaction { get; set; }
        public int? SpaceRentalTransactionId { get; set; }
        public virtual SpaceRentalTransaction SpaceRentalTransaction { get; set; }
        public int? StoreId { get; set; }
        public virtual StoreCreditTransaction StoreCreditTransaction { get; set; }
        #endregion




    }
}
