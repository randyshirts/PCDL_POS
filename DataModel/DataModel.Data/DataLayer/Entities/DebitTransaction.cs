using System;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class DebitTransaction : Entity
    {

        #region Define Members
        //Define members
        //public int Id { get; set; }
        public DateTime DebitTransactionDate { get; set; }
        public double DebitTotal { get; set; }
        public virtual ConsignorPmt ConsignorPmt { get; set; }
        public virtual StoreCreditPmt StoreCreditPmt { get; set; }
        #endregion




    }
}
