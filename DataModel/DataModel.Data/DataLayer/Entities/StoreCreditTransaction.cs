using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class StoreCreditTransaction : Entity
    {

        #region Define Members
        //Define members
        //public int Id { get; set; }
        public virtual CreditTransaction CreditTransaction_StoreCredit { get; set; }
        public virtual int ConsignorId { get; set; }
        public virtual Consignor Consignor_StoreCreditTransaction { get; set; }
        public double StoreCreditTransactionAmount { get; set; }
        //public virtual ICollection<Item> Items_ConsignorPmt { get; set; }

        #endregion

    }
}
