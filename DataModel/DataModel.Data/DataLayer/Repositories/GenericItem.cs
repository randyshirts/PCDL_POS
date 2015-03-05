using System.Collections.Generic;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.DataLayer.Repositories
{
    public abstract class GenericItem : Entity, IGenericItem
    {
        public string Title { get; set; }
        public string Manufacturer { get; set; }
        public string EAN { get; set; }
        public string ItemImage { get; set; }

        public ICollection<Item> Items_TItems { get; set; }
    }
}
