
using System.Linq;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class ItemAppService : PcdAppServiceBase, IItemAppService
    {
        private readonly IItemRepository _itemRepository;

        public ItemAppService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public SetItemBarcodeOutput SetItemBarcode(SetItemBarcodeInput input)
        {
            return new SetItemBarcodeOutput
            {
                Barcode = _itemRepository.SetItemBarcode(input.Item.ConvertToItem())
            };
        }

        public QueryItemsThatAreBooksOutput QueryItemsThatAreBooks()
        {
            var output = _itemRepository.QueryItemsThatAreBooks();
            var list = output.Select(i => (new ItemDto(i))).ToList();
            
            return new QueryItemsThatAreBooksOutput
            {
                Items = list
            };
        }

        public QueryItemsThatAreGamesOutput QueryItemsThatAreGames()
        {
            var output = _itemRepository.QueryItemsThatAreGames();
            var list = output.Select(i => (new ItemDto(i))).ToList();

            return new QueryItemsThatAreGamesOutput
            {
                Items = list
            };
        }

        public QueryItemsThatAreOthersOutput QueryItemsThatAreOthers()
        {
            var output = _itemRepository.QueryItemsThatAreOthers();
            var list = output.Select(i => (new ItemDto(i))).ToList();

            return new QueryItemsThatAreOthersOutput
            {
                Items = list
            };
        }

        public QueryItemsThatAreVideosOutput QueryItemsThatAreVideos()
        {
            var output = _itemRepository.QueryItemsThatAreVideos();
            var list = output.Select(i => (new ItemDto(i))).ToList();

            return new QueryItemsThatAreVideosOutput
            {
                Items = list
            };
        }

        public QueryItemsThatAreTeachingAidesOutput QueryItemsThatAreTeachingAides()
        {
            var output = _itemRepository.QueryItemsThatAreTeachingAides();
            var list = output.Select(i => (new ItemDto(i))).ToList();

            return new QueryItemsThatAreTeachingAidesOutput
            {
                Items = list
            };
        }

        public GetAllItemsOutput GetAllItems()
        {
            var output = _itemRepository.GetAllList();
            var list = output.Select(i => (new ItemDto(i))).ToList();

            return new GetAllItemsOutput
            {
                Items = list
            };
        }

        public UpdateItemOutput UpdateItem(UpdateItemInput input)
        {
            return new UpdateItemOutput
            {
                Id = _itemRepository.UpdateItem(input.Item.ConvertToItem())
            };
        }

        public void DeleteItemById(DeleteItemByIdInput input)
        {
             _itemRepository.DeleteItemById(input.Id);
        }

        public SearchAllItemsOutput SearchAllItems(SearchAllItemsInput input)
        {
            if (input.ItemType == "Teaching Aide")
                input.ItemType = "TeachingAide";

            return new SearchAllItemsOutput
            {
                Items = _itemRepository.SearchAllItems(input.Barcode, 
                                                        input.Status, 
                                                        input.ItemType, 
                                                        input.ConsignorName, 
                                                        input.ListedDate)
                                                .Select(i => (new ItemDto(i))).ToList()
            };
        }

        public SearchListItemsOutput SearchListItems(SearchListItemsInput input)
        {
            return new SearchListItemsOutput
            {
                Items = _itemRepository.SearchListItems(input.ItemList.Select(i => i.ConvertToItem()), 
                                                                                    input.Barcode, 
                                                                                    input.Status, 
                                                                                    input.ItemType, 
                                                                                    input.ConsignorName, 
                                                                                    input.ListedDate)
                                                                        .Select(i => (new ItemDto(i))).ToList()
            };
        }

        public GetItemsByConsignorNameOutput GetItemsByConsignorName(GetItemsByConsignorNameInput input)
        {
            return new GetItemsByConsignorNameOutput
            {
                Items = _itemRepository.GetItemsByConsignorName(input.FirstName, input.LastName).Select(i => (new ItemDto(i))).ToList()
            };
        }

        public GetItemsByPartOfBarcodeOutput GetItemsByPartOfBarcode(GetItemsByPartOfBarcodeInput input)
        {
            return new GetItemsByPartOfBarcodeOutput
            {
                Items = _itemRepository.GetItemsByPartOfBarcode(input.Barcode).Select(i => (new ItemDto(i))).ToList()
            };
        }

        public GetItemByIdOutput GetItemById(GetItemByIdInput input)
        {
            return new GetItemByIdOutput
            {
                Item = new ItemDto(_itemRepository.Get(input.Id))
            };
        }
    }
}
