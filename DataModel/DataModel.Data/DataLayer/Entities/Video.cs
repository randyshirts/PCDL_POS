using System.Collections.Generic;
using DataModel.Data.DataLayer.Repositories;
using Newtonsoft.Json;

namespace DataModel.Data.DataLayer.Entities
{
    [JsonObject(IsReference = true)]
    public class Video : Entity, IGenericItem
    {

        #region Define Members
        //Define members
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string EAN { get; set; }
        public string VideoFormat { get; set; }
        public string AudienceRating { get; set; }
        public string ItemImage { get; set; }
        public string Manufacturer { get; set; }
        public virtual ICollection<Item> Items_TItems { get; set; }
        #endregion




    }
}
