using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Repositories;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class GenericItemController<T> : IGenericItemController<T>
        where T: class, IGenericItem
    {
        public virtual IEnumerable<T> GetAllItems()
        {
            using (var repo = new GenericItemRepositoryBase<T>())
            {
                var app = new GenericItemAppService<T>(repo);
                var output = app.GetAllTItems();
                return output.Items.Select(i => i.ConvertToTItem()).ToList();
            }
        }

        public virtual T GetItemByTitle(string title)
        {
            var input = new GetItemByTitleInput
            {
                Title = title
            };
            using (var repo = new GenericItemRepositoryBase<T>())
            {
                var app = new GenericItemAppService<T>(repo);
                var output = app.GetItemByTitle(input);
                return output.Item.ConvertToTItem();
            }
        }

        public virtual int UpdateItem(T updatedGenericItem)
        {
            var input = new UpdateTItemInput<T>
            {
                Item = new GenericItemDto<T>(updatedGenericItem)
            };
            using (var repo = new GenericItemRepositoryBase<T>())
            {
                var app = new GenericItemAppService<T>(repo);
                return app.UpdateTItem(input).Id;
            }
        }

        public virtual int AddNewItem(T genericItem)
        {

            var itemInput = new GetItemByIdInput();
            var barcodeInput = new SetItemBarcodeInput();
            var updateInput = new UpdateItemInput();

            var input = new AddNewItemInput<T>
            {
                Item = new GenericItemDto<T>(genericItem)
            };
            using (var repo = new GenericItemRepositoryBase<T>())
            {
                var app = new GenericItemAppService<T>(repo);
                itemInput.Id = app.AddNewItem(input).Id;
            }
            using (var itemRepo = new ItemRepository())
            {
                var app = new ItemAppService(itemRepo);
                var thisItem = app.GetItemById(itemInput);
                barcodeInput.Item = thisItem.Item;
                var barcodeOutput = app.SetItemBarcode(barcodeInput);
                thisItem.Item.Barcode = barcodeOutput.Barcode;
                updateInput.Item = thisItem.Item;
                app.UpdateItem(updateInput);
            }
            return itemInput.Id;
        }
    }
}
