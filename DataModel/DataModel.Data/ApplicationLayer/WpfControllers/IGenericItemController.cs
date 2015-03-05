using System.Collections.Generic;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public interface IGenericItemController<T>
        where T : class, IGenericItem
    {
        int UpdateItem(T updatedTItem);
        int AddNewItem(T genericItem);
        T GetItemByTitle(string title);
        IEnumerable<T> GetAllItems();
    }
}
