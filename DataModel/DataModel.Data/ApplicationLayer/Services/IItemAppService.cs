using Abp.Application.Services;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IItemAppService : IApplicationService
    {
        SetItemBarcodeOutput SetItemBarcode(SetItemBarcodeInput input);
        QueryItemsThatAreBooksOutput QueryItemsThatAreBooks();
        QueryItemsThatAreGamesOutput QueryItemsThatAreGames();
        QueryItemsThatAreOthersOutput QueryItemsThatAreOthers();
        QueryItemsThatAreVideosOutput QueryItemsThatAreVideos();
        QueryItemsThatAreTeachingAidesOutput QueryItemsThatAreTeachingAides();
        GetAllItemsOutput GetAllItems();
        UpdateItemOutput UpdateItem(UpdateItemInput input);
        void DeleteItemById(DeleteItemByIdInput input);
        SearchAllItemsOutput SearchAllItems(SearchAllItemsInput input);
        SearchListItemsOutput SearchListItems(SearchListItemsInput input);
        GetItemsByConsignorNameOutput GetItemsByConsignorName(GetItemsByConsignorNameInput input);
        GetItemsByPartOfBarcodeOutput GetItemsByPartOfBarcode(GetItemsByPartOfBarcodeInput input);
        GetItemByIdOutput GetItemById(GetItemByIdInput input);
    }
}
