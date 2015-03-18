using System;
using System.Collections.ObjectModel;
using System.Windows;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.TransactionalLayer.Repositories;
using Inventory.Controller.Elements.ItemElements;
using Inventory.Controller.Interfaces;

namespace Inventory.Controller.Visitors.ItemVisitors
{
    public class ItemTypeUpdateVisitor : IItemVisitor
    {
        public int Visit(BookItem bookItem)
        {
            try
            {
                //Update db table
                var item = bookItem.ConvertBookItemToItem();
                var controller = new ItemController();
                controller.UpdateItem(item);
               
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Item from BookItem failed - ItemUpdateVisitor");
                return -1;
            }

        }


        public int Visit(GameItem gameItem)
        {

            try
            {
                //Update db table
                var item = gameItem.ConvertGameItemToItem();
                var controller = new ItemController();
                controller.UpdateItem(item);
             
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Item from GameItem failed - ItemUpdateVisitor");
                return -1;
            }


        }

        public int Visit(OtherItem otherItem)
        {

            try
            {
                //Update db table
                var item = otherItem.ConvertOtherItemToItem();
                var controller = new ItemController();
                controller.UpdateItem(item);
            
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Item from OtherItem failed - ItemUpdateVisitor");
                return -1;
            }


        }

        public int Visit(VideoItem videoItem)
        {

            try
            {
                //Update db table
                var item = videoItem.ConvertVideoItemToItem();
                var controller = new ItemController();
                controller.UpdateItem(item);
                
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Item from VideoItem failed - ItemUpdateVisitor");
                return -1;
            }


        }

        public int Visit(TeachingAideItem teachingAideItem)
        {

            try
            {
                //Update db table
                var item = teachingAideItem.ConvertTeachingAideItemToItem();
                var controller = new ItemController();
                controller.UpdateItem(item);
                
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Item from TeachingAideItem failed - ItemUpdateVisitor");
                return -1;
            }

        }

        public int Visit(ConsignorItem consignorItem)
        {
            throw new NotImplementedException();
        }
    }
}
