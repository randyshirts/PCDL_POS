using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class SpaceRentalTransaction : Entity
    {

        #region Define Members
        //Define members
        //public int Id { get; set; }
        public virtual CreditTransaction CreditTransaction_SpaceRental { get; set; }
        //public virtual int ConsignorId { get; set; }
        //public virtual Consignor Consignor_ConsignorPmt { get; set; }
        //public virtual ICollection<Item> Items_SpaceRentalTransaction { get; set; }

        #endregion

    }
}
