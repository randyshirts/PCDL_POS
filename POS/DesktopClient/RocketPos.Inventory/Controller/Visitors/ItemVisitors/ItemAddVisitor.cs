using System;
using System.Collections.ObjectModel;
using System.Windows;
using DataModel.Data.ApplicationLayer.WpfControllers;
using DataModel.Data.DataLayer.Entities;
using Inventory.Controller.Elements.ItemElements;
using Inventory.Controller.Interfaces;

namespace Inventory.Controller.Visitors.ItemVisitors
{
    public class ItemAddVisitor : IItemVisitor
    {
        public int Visit(BookItem bookItem)
        {
            var controller = new ConsignorController();
            var consignor = controller.GetConsignorByFullName(bookItem.ConsignorName);

            var book = new Book
            {
                ISBN = bookItem.Isbn,
                Title = bookItem.Title,
                Author = bookItem.Author,
                Binding = bookItem.Binding,
                PublicationDate = bookItem.PublicationDate,
                NumberOfPages = bookItem.NumberOfPages,
                TradeInValue = bookItem.TradeInValue,
                ItemImage = bookItem.BookImage,

                Items_TItems = new Collection<Item>
                {
                    new Item
                    {
                        ItemType = "Book",
                        ListedPrice = bookItem.ListedPrice,
                        ListedDate = bookItem.ListedDate,
                        Subject = bookItem.Subject,
                        Status = bookItem.ItemStatus,
                        Condition = bookItem.Condition,
                        Description = bookItem.Description,
                        ConsignorId = consignor.Id,
                        IsDiscountable = bookItem.IsDiscountable
                    }
                }

            };

            try
            {
                var bookController = new BookController();
                return bookController.AddNewItem(book);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AddItem-BookException");
                return -1;
            }

        }

        public int Visit(GameItem gameItem)
        {
            var controller = new ConsignorController();
            var consignor = controller.GetConsignorByFullName(gameItem.ConsignorName);
            
            var game = new Game
                    {
                        EAN = gameItem.Ean,
                        Title = gameItem.Title,
                        Manufacturer = gameItem.Manufacturer,
                        ItemImage = gameItem.GameImage,

                        Items_TItems = new Collection<Item>
                    {
                        new Item
                        {
                            ItemType = "Game",
                            ListedPrice = gameItem.ListedPrice,
                            ListedDate = gameItem.ListedDate,
                            Subject = gameItem.Subject,
                            Status = gameItem.ItemStatus,
                            Condition = gameItem.Condition,
                            Description = gameItem.Description,
                            ConsignorId = consignor.Id,
                            IsDiscountable = gameItem.IsDiscountable
                        }
                    }
                    };

            try
            {
                var gameController = new GameController();
                return gameController.AddNewItem(game);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AddItem-BookException");
                return -1;
            }

        }

        public int Visit(OtherItem otherItem)
        {
            var controller = new ConsignorController();
            var consignor = controller.GetConsignorByFullName(otherItem.ConsignorName);

            var other = new Other
                {
                    EAN = otherItem.Ean,
                    Title = otherItem.Title,
                    Manufacturer = otherItem.Manufacturer,
                    ItemImage = otherItem.OtherImage,

                    Items_TItems = new Collection<Item>
                    {
                        new Item
                        {
                            ItemType = "Other",
                            ListedPrice = otherItem.ListedPrice,
                            ListedDate = otherItem.ListedDate,
                            Subject = otherItem.Subject,
                            Status = otherItem.ItemStatus,
                            Condition = otherItem.Condition,
                            Description = otherItem.Description,
                            ConsignorId = consignor.Id,
                            IsDiscountable = otherItem.IsDiscountable
                        }
                    }
                };

            try
            {
                var otherController = new OtherController();
                return otherController.AddNewItem(other);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AddItem-OtherException");
                return -1;
            }

        }

        public int Visit(VideoItem videoItem)
        {
            var controller = new ConsignorController();
            var consignor = controller.GetConsignorByFullName(videoItem.ConsignorName);

            var video = new Video
                {
                    EAN = videoItem.Ean,
                    Title = videoItem.Title,
                    AudienceRating = videoItem.AudienceRating,
                    VideoFormat = videoItem.VideoFormat,
                    ItemImage = videoItem.VideoImage,
                    Publisher = videoItem.Publisher,

                    Items_TItems = new Collection<Item>
                    {
                        new Item
                        {
                            ItemType = "Video",
                            ListedPrice = videoItem.ListedPrice,
                            ListedDate = videoItem.ListedDate,
                            Subject = videoItem.Subject,
                            Status = videoItem.ItemStatus,
                            Condition = videoItem.Condition,
                            Description = videoItem.Description,
                            ConsignorId = consignor.Id,
                            IsDiscountable = videoItem.IsDiscountable
                        }
                    }
                };

            try
            {
                var videoController = new VideoController();
                return videoController.AddNewItem(video);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AddItem-VideoException");
                return -1;
            }

        }

        public int Visit(TeachingAideItem teachingAideItem)
        {
            var controller = new ConsignorController();
            var consignor = controller.GetConsignorByFullName(teachingAideItem.ConsignorName);

            var teachingAide = new TeachingAide
                {
                    EAN = teachingAideItem.Ean,
                    Title = teachingAideItem.Title,
                    Manufacturer = teachingAideItem.Manufacturer,
                    ItemImage = teachingAideItem.TeachingAideImage,

                    Items_TItems = new Collection<Item>
                    {
                        new Item
                        {
                            ItemType = "TeachingAide",
                            ListedPrice = teachingAideItem.ListedPrice,
                            ListedDate = teachingAideItem.ListedDate,
                            Subject = teachingAideItem.Subject,
                            Status = teachingAideItem.ItemStatus,
                            Condition = teachingAideItem.Condition,
                            Description = teachingAideItem.Description,
                            ConsignorId = consignor.Id,
                            IsDiscountable = teachingAideItem.IsDiscountable
                        }
                    }
                };

            try
            {
                var teachingAideController = new TeachingAideController();
                return teachingAideController.AddNewItem(teachingAide);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AddItem-TeachingAideException");
                return -1;
            }

        }

        public int Visit(ConsignorItem consignorItem)
        {
            throw new NotImplementedException();
        }
    }
}
