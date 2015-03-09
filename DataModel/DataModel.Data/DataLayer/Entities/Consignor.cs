using System;
using System.Collections.Generic;
using DataModel.Data.DataLayer.Repositories;
using Newtonsoft.Json;

namespace DataModel.Data.DataLayer.Entities
{
    [JsonObject(IsReference = true)]
    public class Consignor : Entity
    {
        #region Define Members
        //Define members
        //public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public virtual Person Consignor_Person { get; set; }
        public virtual ICollection<ConsignorPmt> ConsignorPmts_Consignor { get; set; }
        public virtual ICollection<StoreCreditTransaction> StoreCreditTransactions_Consignor { get; set; }
        public virtual ICollection<StoreCreditPmt> StoreCreditPmts_Consignor { get; set; } 
        public virtual ICollection<Item> Items_Consignor { get; set; }

        #endregion




    }
}
