using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IGenericItemAppService<T>
        where T : class, IGenericItem
    {
        GetAllTItemsOutput<T> GetAllTItems();
        GetItemByTitleOutput<T> GetItemByTitle(GetItemByTitleInput input);
        AddNewItemOutput AddNewItem(AddNewItemInput<T> input);
        UpdateTItemOutput UpdateTItem(UpdateTItemInput<T> input);
        void DeleteItemById(DeleteItemByIdInput<T> input);
    }
}
