using System.Collections.Generic;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class ItemSaleTransaction : Entity
    {

        #region Define Members
        //Define members
        //public int Id { get; set; }
        public virtual CreditTransaction CreditTransaction_ItemSale { get; set; }
        //public virtual int ConsignorId { get; set; }
        //public virtual Consignor Consignor_ConsignorPmt { get; set; }
        public virtual ICollection<Item> Items_ItemSaleTransaction { get; set; }

        #endregion

    }
}
