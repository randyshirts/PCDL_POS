
using System;
using System.Linq;
using Common.Helpers;
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


            var items = _itemRepository.SearchAllItems(input.Barcode,
                input.Status,
                input.ItemType,
                input.ConsignorName,
                input.ListedDate,
                input.Title);
            
            if(items != null)                        
            return new SearchAllItemsOutput
            {
                Items = items.Select(i => (new ItemDto(i))).ToList()
            };

            return new SearchAllItemsOutput
            {
                Items = null
            };

        }

        public SearchItemsDateRangeOutput SearchItemsDateRange(SearchItemsDateRangeInput input)
        {

            var items = _itemRepository.SearchItemsDateRange(
                input.FromDate, 
                input.EndDate, 
                input.ConsignorName, 
                input.Status
                );
            
            if(items != null)
                return new SearchItemsDateRangeOutput
            {
                Items = items.Select(i => (new ItemDto(i))).ToList()
            };

            return new SearchItemsDateRangeOutput
            {
                Items = null
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

        public GetItemTitleOutput GetItemTitle(GetItemTitleInput input)
        {
            string title;

            if (input.Item.ItemType == "Book")
            {
                title = input.Item.Book.Title;
            }

            else if (input.Item.ItemType == "Game")
            {
                title = input.Item.Game.Title;
            }

            else if (input.Item.ItemType == "Other")
            {
                title = input.Item.Other.Title;
            }

            else if (input.Item.ItemType == "TeachingAide")
            {
                title = input.Item.TeachingAide.Title;
            }

            else if (input.Item.ItemType == "Video")
            {
                title = input.Item.Video.Title;
            }

            else
            {
                title = null;
            }

            return new GetItemTitleOutput
            {
                Title = title
            };
        }

        public GetCurrentPriceOutput GetCurrentPrice(GetCurrentPriceInput input)
        {
            var price = input.Item.ListedPrice;
            var dateSpan = DateTimeSpan.CompareDates(input.Item.ListedDate, DateTime.Now);
            if ((input.Item.IsDiscountable) && (dateSpan.Months > 0))
            {
                price = dateSpan.Months < 6 ? input.Item.ListedPrice * dateSpan.Months * .10 : input.Item.ListedPrice * .50;
            }

            return new GetCurrentPriceOutput
            {
                Price = price
            };

        }
    }
}
