using System.Collections.Generic;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class ConsignorPmt : Entity
    {
        #region Define Members
        //Define members
        //public int Id { get; set; }
        public virtual DebitTransaction DebitTransaction_ConsignorPmt { get; set; }
        public virtual int ConsignorId { get; set; }
        public virtual Consignor Consignor_ConsignorPmt { get; set; }
        public virtual ICollection<Item> Items_ConsignorPmt { get; set; }

        #endregion

    }
}
