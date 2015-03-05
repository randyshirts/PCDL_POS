using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class ItemController : IItemController
    {
        public string SetItemBarcode(Item item)
        {
            var input = new SetItemBarcodeInput
            {
                Item = new ItemDto(item)
            };
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.SetItemBarcode(input);
                return output.Barcode;
            }
        }

        public int UpdateItem(Item updatedItem)
        {
            var input = new UpdateItemInput
            {
                Item = new ItemDto(updatedItem)
            };
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.UpdateItem(input);
                return output.Id;
            }
        }

        public void DeleteItemById(int id)
        {
            var input = new DeleteItemByIdInput
            {
                Id = id
            };
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                app.DeleteItemById(input);
            }
        }

        public IEnumerable<Item> GetAllItems()
        {
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.GetAllItems();

                return output.Items.Select(i => i.ConvertToItem()).ToList();
            }
        }

        public Item GetItemById(int id)
        {
            var input = new GetItemByIdInput
            {
                Id = id
            };
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.GetItemById(input);
                return output.Item.ConvertToItem();
            }
        }

        public IEnumerable<Item> SearchAllItems(string barcode, string status, string itemType, string consignorName, DateTime? listedDate)
        {
            var input = new SearchAllItemsInput
            {
                Barcode = barcode,
                Status = status,
                ItemType = itemType,
                ConsignorName = consignorName,
                ListedDate = listedDate
            };
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.SearchAllItems(input);

                return output.Items.Select(i => i.ConvertToItem()).ToList();
            }
        }

        public IEnumerable<Item> SearchListItems(IEnumerable<Item> list, string barcode, string status, string itemType, string consignorName,
            DateTime? listedDate)
        {
            var input = new SearchListItemsInput
            {
                Barcode = barcode,
                Status = status,
                ItemType = itemType,
                ConsignorName = consignorName,
                ListedDate = listedDate
            };
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.SearchListItems(input);

                return output.Items.Select(i => i.ConvertToItem()).ToList();
            }
        }

        public List<Item> GetItemsByConsignorName(string firstName, string lastName)
        {
            var input = new GetItemsByConsignorNameInput
            {
                FirstName = firstName,
                LastName = lastName
            };
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.GetItemsByConsignorName(input);
                return output.Items.Select(i => i.ConvertToItem()).ToList();
            }          
        }

        public List<Item> GetItemsByPartOfBarcode(string barcode)
        {
            var input = new GetItemsByPartOfBarcodeInput
            {
                Barcode = barcode
            };
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.GetItemsByPartOfBarcode(input);
                return output.Items.Select(i => i.ConvertToItem()).ToList();
            }   
        }

        public List<Item> QueryItemsThatAreBooks()
        {
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.QueryItemsThatAreBooks();
                return output.Items.Select(i => i.ConvertToItem()).ToList();
            }  
        }

        public List<Item> QueryItemsThatAreGames()
        {
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.QueryItemsThatAreGames();
                return output.Items.Select(i => i.ConvertToItem()).ToList();
            }  
        }

        public List<Item> QueryItemsThatAreOthers()
        {
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.QueryItemsThatAreOthers();
                return output.Items.Select(i => i.ConvertToItem()).ToList();
            }  
        }

        public List<Item> QueryItemsThatAreTeachingAides()
        {
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.QueryItemsThatAreTeachingAides();
                return output.Items.Select(i => i.ConvertToItem()).ToList();
            }  
        }

        public List<Item> QueryItemsThatAreVideos()
        {
            using (var repo = new ItemRepository())
            {
                var app = new ItemAppService(repo);
                var output = app.QueryItemsThatAreVideos();
                return output.Items.Select(i => i.ConvertToItem()).ToList();
            }  
        }
    }
}
