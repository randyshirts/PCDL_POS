using System;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class Item : Entity
    {

        #region Define Members
        //public int Id { get; set; }
        //[Required]
        public string ItemType { get; set; }
        public double ListedPrice { get; set; }
        public double? SalePrice { get; set; }
        //public double? ConsignorPmt { get; set; }
        public bool? CashPayout { get; set; }
        public DateTime ListedDate { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string Condition { get; set; }
        public bool IsDiscountable { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public virtual int? BookId { get; set; }
        public virtual Book Book { get; set; }
        public virtual int? GameId { get; set; }
        public virtual Game Game { get; set; }
        public virtual int? TeachingAideId { get; set; }
        public virtual TeachingAide TeachingAide { get; set; }
        public virtual int? OtherId { get; set; }
        public virtual Other Other { get; set; }
        public virtual int? VideoId { get; set; }
        public virtual Video Video { get; set; }
        public virtual int ConsignorId { get; set; }
        public virtual Consignor Consignor { get; set; }
        public virtual int? ItemSaleTransactionId { get; set; }
        public virtual ItemSaleTransaction ItemSaleTransaction { get; set; }
        public virtual int? ConsignorPmtId { get; set; }
        public virtual ConsignorPmt ConsignorPmt { get; set; }
        #endregion
    }
}


    

