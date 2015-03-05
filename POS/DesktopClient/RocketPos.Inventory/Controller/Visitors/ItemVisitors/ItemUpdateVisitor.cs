using System;
using System.Windows;
using DataModel.Data.ApplicationLayer.WpfControllers;
using Inventory.Controller.Elements.ItemElements;
using Inventory.Controller.Interfaces;

namespace Inventory.Controller.Visitors.ItemVisitors
{
    public class ItemUpdateVisitor : IItemVisitor
    {
        public int Visit(BookItem bookItem)
        {
            try
            {
                //Update db table
                var controller = new BookController();
                var book = bookItem.ConvertBookItemToBook();
                controller.UpdateItem(book);
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
                var controller = new GameController();
                var game = gameItem.ConvertGameItemToGame();
                controller.UpdateItem(game);
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
                var controller = new OtherController();
                var other = otherItem.ConvertOtherItemToOther();
                controller.UpdateItem(other);

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
                var controller = new VideoController();
                var video = videoItem.ConvertVideoItemToVideo();
                controller.UpdateItem(video);
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
                var controller = new TeachingAideController();
                var teachingAide = teachingAideItem.ConvertTeachingAideItemToTeachingAide();
                controller.UpdateItem(teachingAide);
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
            try
            {

                var item = consignorItem.ConvertConsignorItemToItem();

                var itemController = new ItemController();
                var itemId = itemController.UpdateItem(item);
                

                switch (item.ItemType)
                {
                    case "Book":
                        {
                            var controller = new BookController();
                            controller.UpdateItem(item.Book);
                            break;
                        }
                    case "Game":
                        {
                            var controller = new GameController();
                            controller.UpdateItem(item.Game);
                            break;
                        }
                    case "Other":
                        {
                            var controller = new OtherController();
                            controller.UpdateItem(item.Other);
                            break;
                        }
                    case "Video":
                        {
                            var controller = new VideoController();
                            controller.UpdateItem(item.Video);
                            break;
                        }
                    case "TeachingAide":
                        {
                            var controller = new TeachingAideController();
                            controller.UpdateItem(item.TeachingAide);
                            break;
                        }
                }
                return itemId;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Item from ConsignorItem failed - ItemUpdateVisitor");
                return -1;
            }
        }
    }
}
