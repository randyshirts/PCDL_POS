using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public interface IGenericItemRepository<T>
        where T : class, IGenericItem
    {
        int UpdateItem(T updatedTItem);
        int AddNewItem(T genericItem);
        T GetItemByTitle(string title);
    }
}
