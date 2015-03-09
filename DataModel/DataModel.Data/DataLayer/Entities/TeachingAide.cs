using System.Collections.Generic;
using DataModel.Data.DataLayer.Repositories;
using Newtonsoft.Json;

namespace DataModel.Data.DataLayer.Entities
{
    [JsonObject(IsReference = true)]
    public class TeachingAide : Entity, IGenericItem
    {
        #region Ctors
        public TeachingAide()
        {

        }

        //public TeachingAide(string title, string manufacturer, string ean, string teachingAideImage, Item item)
        //{
        //    Title = title;
        //    Manufacturer = manufacturer;
        //    Items_TeachingAide.Add(item);
        //    EAN = ean;
        //    TeachingAideImage = teachingAideImage;
        //}


        #endregion


        #region Define Members
        //Define members
        //public int Id { get; set; }
        public string Title { get; set; }
        public string Manufacturer { get; set; }
        public string EAN { get; set; }
        public string ItemImage { get; set; }
        public virtual ICollection<Item> Items_TItems { get; set; }
        #endregion

    }
}