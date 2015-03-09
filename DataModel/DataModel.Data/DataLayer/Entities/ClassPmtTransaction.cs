﻿using DataModel.Data.DataLayer.Repositories;
using Newtonsoft.Json;

namespace DataModel.Data.DataLayer.Entities

{
    [JsonObject(IsReference = true)]
    public class ClassPmtTransaction : Entity
    {

        #region Define Members
        //Define members
        //public int Id { get; set; }
        public virtual CreditTransaction CreditTransaction_ClassPmt { get; set; }
        //public virtual int ConsignorId { get; set; }
        //public virtual Consignor Consignor_ConsignorPmt { get; set; }
        //public virtual ICollection<Item> Items_ItemSaleTransaction { get; set; }

        #endregion

    }
}
