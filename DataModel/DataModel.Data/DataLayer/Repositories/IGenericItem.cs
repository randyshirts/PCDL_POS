using System.Collections;
using System.Collections.Generic;
using Abp.Domain.Entities;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.DataLayer.Repositories
{
    public interface IGenericItem : IEntity
    {
        string Title { get; set; }
        string Manufacturer { get; set; }
        string EAN { get; set; }
        string ItemImage { get; set; }
        ICollection<Item> Items_TItems { get; set; }
    }
}
