//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using DataModel.Data.DataLayer.Entities;
//using RocketPos.Common.Foundation;
//using RocketPos.Data.TransactionalLayer;

//namespace RocketPos.Common.Helpers
//{
//    public static class SearchItems
//    {

//        public static Collection<Item> SearchItemTable(string barcode, string status, string listedDateString, string consignorName, string itemType)
//        {
//            var isStatusNull = String.IsNullOrEmpty(status);
//            var isListedDateStringNull = String.IsNullOrEmpty(listedDateString);
//            var isConsignorNameNull = String.IsNullOrEmpty(consignorName);
//            var isItemTypeNull = String.IsNullOrEmpty(itemType);
//            var isBarcodeNull = String.IsNullOrEmpty(barcode);

//            if (status == "All")
//                isStatusNull = true;
//            if (listedDateString == "All")
//                isListedDateStringNull = true;
//            if (consignorName == "All")
//                isConsignorNameNull = true;
//            if (itemType == "All")
//                isItemTypeNull = true;

//            if ((!isBarcodeNull) && isStatusNull && (isListedDateStringNull) && isConsignorNameNull && isItemTypeNull)
//                return SearchItems31(barcode);
//            if ((!isBarcodeNull) && (!isStatusNull) && (!isListedDateStringNull) && (!isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems30(barcode, status, listedDateString, consignorName, itemType);
//            if ((!isBarcodeNull) && (!isStatusNull) && (!isListedDateStringNull) && (!isConsignorNameNull) && isItemTypeNull)
//                return SearchItems29(barcode, status, listedDateString, consignorName);
//            if ((!isBarcodeNull) && (!isStatusNull) && (!isListedDateStringNull) && (isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems28(barcode, status, listedDateString, itemType);
//            if ((!isBarcodeNull) && (!isStatusNull) && (!isListedDateStringNull) && (isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems27(barcode, status, listedDateString);
//            if ((!isBarcodeNull) && (!isStatusNull) && (isListedDateStringNull) && (!isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems26(barcode, status, consignorName, itemType);
//            if ((!isBarcodeNull) && (!isStatusNull) && (isListedDateStringNull) && (!isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems25(barcode, status, consignorName);
//            if ((!isBarcodeNull) && (!isStatusNull) && (isListedDateStringNull) && (isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems24(barcode, status, itemType);
//            if ((!isBarcodeNull) && (!isStatusNull) && (isListedDateStringNull) && (isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems23(barcode, status);
//            if ((!isBarcodeNull) && (isStatusNull) && (!isListedDateStringNull) && (!isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems22(barcode, listedDateString, consignorName, itemType);
//            if ((!isBarcodeNull) && (isStatusNull) && (!isListedDateStringNull) && (!isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems21(barcode, listedDateString, consignorName);
//            if ((!isBarcodeNull) && (isStatusNull) && (!isListedDateStringNull) && (isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems20(barcode, listedDateString, itemType);
//            if ((!isBarcodeNull) && (isStatusNull) && (!isListedDateStringNull) && (isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems19(barcode, listedDateString);
//            if ((!isBarcodeNull) && (isStatusNull) && (isListedDateStringNull) && (!isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems18(barcode, consignorName, itemType);
//            if ((!isBarcodeNull) && (isStatusNull) && (isListedDateStringNull) && (!isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems17(barcode, consignorName);
//            if ((!isBarcodeNull) && (isStatusNull) && (isListedDateStringNull) && (isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems16(barcode, itemType);
//            if ((isBarcodeNull) && (!isStatusNull) && (!isListedDateStringNull) && (!isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems15(status, listedDateString, consignorName, itemType);
//            if ((isBarcodeNull) && (!isStatusNull) && (!isListedDateStringNull) && (!isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems14(status, listedDateString, consignorName);
//            if ((isBarcodeNull) && (!isStatusNull) && (!isListedDateStringNull) && (isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems13(status, listedDateString, itemType);
//            if ((isBarcodeNull) && (!isStatusNull) && (!isListedDateStringNull) && (isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems12(status, listedDateString);
//            if ((isBarcodeNull) && (!isStatusNull) && (isListedDateStringNull) && (!isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems11(status, consignorName, itemType);
//            if ((isBarcodeNull) && (!isStatusNull) && (isListedDateStringNull) && (!isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems10(status, consignorName);
//            if ((isBarcodeNull) && (!isStatusNull) && (isListedDateStringNull) && (isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems9(status, itemType);
//            if ((isBarcodeNull) && (!isStatusNull) && (isListedDateStringNull) && (isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems8(status);
//            if ((isBarcodeNull) && (isStatusNull) && (!isListedDateStringNull) && (!isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems7(listedDateString, consignorName, itemType);
//            if ((isBarcodeNull) && (isStatusNull) && (!isListedDateStringNull) && (!isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems6(listedDateString, consignorName);
//            if ((isBarcodeNull) && (isStatusNull) && (!isListedDateStringNull) && (isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems5(listedDateString, itemType);
//            if ((isBarcodeNull) && (isStatusNull) && (!isListedDateStringNull) && (isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems4(listedDateString);
//            if ((isBarcodeNull) && (isStatusNull) && (isListedDateStringNull) && (!isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems3(consignorName, itemType);
//            if ((isBarcodeNull) && (isStatusNull) && (isListedDateStringNull) && (!isConsignorNameNull) && (isItemTypeNull))
//                return SearchItems2(consignorName);
//            if ((isBarcodeNull) && (isStatusNull) && (isListedDateStringNull) && (isConsignorNameNull) && (!isItemTypeNull))
//                return SearchItems1(itemType);
//            return null;
//        }


//        private static Collection<Item> SearchItems31(string barcode)
//        {

//            using (var api = new BusinessContext())
//            {

//                var tempItems = api.GetItemsByPartOfBarcode(barcode);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems30(string barcode, string status, string listedDateString, string consignorName, string itemType)
//        {

//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByBarcodeConsignornameStatusListeddateItemtype(barcode, names[0], names[1], status, listedDate, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }





//        }

//        private static Collection<Item> SearchItems29(string barcode, string status, string listedDateString, string consignorName)
//        {

//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByBarcodeConsignornameStatusListeddate(barcode, names[0], names[1], status, listedDate);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems28(string barcode, string status, string listedDateString, string itemType)
//        {

//            using (var api = new BusinessContext())
//            {

//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByBarcodeStatusListeddateItemtype(barcode, status, listedDate, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems27(string barcode, string status, string listedDateString)
//        {


//            using (var api = new BusinessContext())
//            {

//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByBarcodeStatusListeddate(barcode, status, listedDate);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems26(string barcode, string status, string consignorName, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();

//                var tempItems = api.GetItemsByBarcodeConsignornameStatusItemtype(barcode, names[0], names[1], status, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems25(string barcode, string status, string consignorName)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();

//                var tempItems = api.GetItemsByBarcodeConsignorNameAndStatus(barcode, names[0], names[1], status);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems24(string barcode, string status, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var tempItems = api.GetItemsByBarcodeStatusItemtype(barcode, status, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems23(string barcode, string status)
//        {


//            using (var api = new BusinessContext())
//            {

//                var tempItems = api.GetItemsByBarcodeStatus(barcode, status);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems22(string barcode, string listedDateString, string consignorName, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByBarcodeConsignornameListeddateItemtype(barcode, names[0], names[1], listedDate, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems21(string barcode, string listedDateString, string consignorName)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByBarcodeConsignornameListeddate(barcode, names[0], names[1], listedDate);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems20(string barcode, string listedDateString, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByBarcodeListeddateItemtype(barcode, listedDate, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems19(string barcode, string listedDateString)
//        {


//            using (var api = new BusinessContext())
//            {
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByBarcodeListeddate(barcode, listedDate);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems18(string barcode, string consignorName, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();

//                var tempItems = api.GetItemsByBarcodeConsignornameItemtype(barcode, names[0], names[1], itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems17(string barcode, string consignorName)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();

//                var tempItems = api.GetItemsByBarcodeConsignorName(barcode, names[0], names[1]);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems16(string barcode, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var tempItems = api.GetItemsByBarcodeItemType(barcode, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems15(string status, string listedDateString, string consignorName, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByConsignornameStatusListeddateItemtype(names[0], names[1], status, listedDate, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }





//        }

//        private static Collection<Item> SearchItems14(string status, string listedDateString, string consignorName)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByConsignornameStatusListeddate(names[0], names[1], status, listedDate);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems13(string status, string listedDateString, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByStatusListeddateItemtype(status, listedDate, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems12(string status, string listedDateString)
//        {


//            using (var api = new BusinessContext())
//            {

//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByStatusListeddate(status, listedDate);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems11(string status, string consignorName, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();

//                var tempItems = api.GetItemsByConsignornameStatusItemtype(names[0], names[1], status, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems10(string status, string consignorName)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();

//                var tempItems = api.GetItemsByConsignorNameAndStatus(names[0], names[1], status);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems9(string status, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var tempItems = api.GetItemsByStatusItemtype(status, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems8(string status)
//        {


//            using (var api = new BusinessContext())
//            {

//                var tempItems = api.GetItemsByStatus(status);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems7(string listedDateString, string consignorName, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByConsignornameListeddateItemtype(names[0], names[1], listedDate, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems6(string listedDateString, string consignorName)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByConsignornameListeddate(names[0], names[1], listedDate);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems5(string listedDateString, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByListeddateItemtype(listedDate, itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems4(string listedDateString)
//        {


//            using (var api = new BusinessContext())
//            {
//                DateTime d;
//                DateTime listedDate = DateTime.TryParse(listedDateString, out d) ? d : DateTime.MinValue;

//                var tempItems = api.GetItemsByListeddate(listedDate);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems3(string consignorName, string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();

//                var tempItems = api.GetItemsByConsignornameItemtype(names[0], names[1], itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems2(string consignorName)
//        {


//            using (var api = new BusinessContext())
//            {

//                var names = consignorName.Split();

//                var tempItems = api.GetItemsByConsignorName(names[0], names[1]);

//                var items = new Collection<Item>(LoadLazyObjects(tempItems));

//                return items;

//            }
//        }

//        private static Collection<Item> SearchItems1(string itemType)
//        {


//            using (var api = new BusinessContext())
//            {

//                var tempItems = api.GetItemsByItemType(itemType);

//                //Get lazy loaded objects
//                var items = new Collection<Item>(LoadLazyObjects(tempItems));


//                return items;

//            }
//        }

        ////Make a collection of BarcodeItems with info from a collection of Items 
        //public static TrulyObservableCollection<BarcodeItem> CreateBarcodeItemsList(Collection<Item> items)
        //{
        //    //Temp Collection Definition
        //    var tempBarcodeItems = new TrulyObservableCollection<BarcodeItem>();

        //    if (items == null) return tempBarcodeItems;

        //    //Make the collection
        //    var i = items.GetEnumerator();
        //    while (i.MoveNext())
        //    {
        //        //Create an instance of a BarcodeItem with the current item
        //        var currentBarcodeItem = new BarcodeItem(i.Current);
        //        //Add the new instance to the collection
        //        tempBarcodeItems.Add(currentBarcodeItem);
        //    }

        //    return tempBarcodeItems;
        //}


//        private static List<Item> LoadLazyObjects(List<Item> items)
//        {
//            var tempItems = new List<Item>();

//            //Get lazy loaded objects
//            foreach (var item in items)
//            {
//                var tempItem = item;

//                if (item.Consignor != null)
//                {
//                    tempItem.Consignor = item.Consignor;
//                    tempItem.ConsignorId = item.ConsignorId;
//                    tempItem.Consignor.Consignor_Person = item.Consignor.Consignor_Person;
//                    tempItem.Consignor.StoreCreditPmts_Consignor = item.Consignor.StoreCreditPmts_Consignor;
//                }

//                if (item.ItemSaleTransaction != null)
//                {
//                    tempItem.ItemSaleTransaction = item.ItemSaleTransaction;
//                    tempItem.ItemSaleTransactionId = item.ItemSaleTransactionId;
//                    tempItem.ItemSaleTransaction.CreditTransaction_ItemSale =
//                        item.ItemSaleTransaction.CreditTransaction_ItemSale;
//                }

//                if (item.ConsignorPmt != null)
//                {
//                    tempItem.ConsignorPmt = item.ConsignorPmt;
//                    tempItem.ConsignorPmtId = item.ConsignorPmtId;
//                    tempItem.ConsignorPmt.DebitTransaction_ConsignorPmt = item.ConsignorPmt.DebitTransaction_ConsignorPmt;
//                }

//                if (item.Book != null)
//                {
//                    tempItem.Book = item.Book;
//                    tempItem.Id = item.Id;
//                }
//                else if (item.Game != null)
//                {
//                    tempItem.Game = item.Game;
//                    tempItem.Id = item.Id;
//                }
//                else if (item.Other != null)
//                {
//                    tempItem.Other = item.Other;
//                    tempItem.Id = item.Id;
//                }
//                else if (item.Video != null)
//                {
//                    tempItem.Video = item.Video;
//                    tempItem.Id = item.Id;
//                }
//                else if (item.TeachingAide != null)
//                {
//                    tempItem.TeachingAide = item.TeachingAide;
//                    tempItem.Id = item.Id;
//                }

//                tempItems.Add(tempItem);
//            }

//            return tempItems;
//        }

//    }
//}
