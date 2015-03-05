using System;
using System.Collections;
using System.Collections.Generic;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public interface IItemController
    {
        string SetItemBarcode(Item item);
        int UpdateItem(Item updatedItem);
        void DeleteItemById(int id);

        IEnumerable<Item> GetAllItems();

        DataLayer.Entities.Item GetItemById(int id);
        IEnumerable<Item> SearchAllItems(string barcode, string status, string itemType, string consignorName,
            DateTime? listedDate);

        IEnumerable<Item> SearchListItems(IEnumerable<Item> list, string barcode, string status, string itemType,
            string consignorName, DateTime? listedDate);

        List<Item> GetItemsByConsignorName(string firstName, string lastName);
        List<Item> GetItemsByPartOfBarcode(string barcode);

        List<Item> QueryItemsThatAreBooks();
        List<Item> QueryItemsThatAreGames();
        List<Item> QueryItemsThatAreOthers();
        List<Item> QueryItemsThatAreTeachingAides();
        List<Item> QueryItemsThatAreVideos();
    }
}
