using System;
using System.Windows;
using DataModel.Data.ApplicationLayer.WpfControllers;
using Inventory.Controller.Elements.ItemElements;
using Inventory.Controller.Interfaces;

namespace Inventory.Controller.Visitors.ItemVisitors
{
    public class ItemDeleteVisitor : IItemVisitor
    {
        public int Visit(BookItem bookItem)
        {
            try
            {
                //Delete db table
                var controller = new ItemController();
                controller.DeleteItemById(bookItem.ItemId);
                
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Item from BookItem failed - ItemDeleteVisitor");
                return -1;
            }

        }


        public int Visit(GameItem gameItem)
        {

            try
            {
                //Delete db table
                var controller = new ItemController();
                controller.DeleteItemById(gameItem.ItemId);
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Item from GameItem failed - ItemDeleteVisitor");
                return -1;
            }


        }

        public int Visit(OtherItem otherItem)
        {

            try
            {
                //Delete db table
                var controller = new ItemController();
                controller.DeleteItemById(otherItem.ItemId);
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Item from OtherItem failed - ItemDeleteVisitor");
                return -1;
            }


        }

        public int Visit(VideoItem videoItem)
        {

            try
            {
                //Delete db table
                var controller = new ItemController();
                controller.DeleteItemById(videoItem.ItemId);
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Item from VideoItem failed - ItemDeleteVisitor");
                return -1;
            }


        }

        public int Visit(TeachingAideItem teachingAideItem)
        {

            try
            {
                //Delete db table
                var controller = new ItemController();
                controller.DeleteItemById(teachingAideItem.ItemId);
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Item from TeachingAideItem failed - ItemDeleteVisitor");
                return -1;
            }

        }

        public int Visit(ConsignorItem consignorItem)
        {
            throw new NotImplementedException();
        }
    }
}
