﻿using DataModel.Data.DataLayer.Repositories;
using Newtonsoft.Json;

namespace DataModel.Data.DataLayer.Entities
{
    [JsonObject(IsReference = true)]
    public class Member : Entity
    {

        #region Define Members
        //Define members
        //public int Id { get; set; }
        public virtual Person Member_Person { get; set; }
        #endregion




    }
}
