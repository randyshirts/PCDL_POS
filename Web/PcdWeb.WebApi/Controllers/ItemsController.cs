using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Abp.UI;
using Castle.Core.Internal;
using Common.Barcodes;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;
using DataModel.Data.TransactionalLayer.Repositories;
using PcdWeb.Models.ItemModels;
using Spire.Pdf;

namespace PcdWeb.Controllers
{
    [System.Web.Http.RoutePrefix("api/Items")]
    public class ItemsController : ApiController
    {
        [System.Web.Http.Authorize]
        [System.Web.Http.Route("")]
        public IHttpActionResult Get()
        {
            //ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            //var Name = ClaimsPrincipal.Current.Identity.Name;
            //var Name1 = User.Identity.Name;

            //var userName = principal.Claims.Where(c => c.Type == "sub").Single().Value;

            return Ok("nothing here");
        }


        [System.Web.Http.Authorize]
        [System.Web.Http.Route("PrintNewBarcodes")]
        public HttpResponse PrintNewBarcodes(IEnumerable<PrintBarcodesModel> model)
        {
            var printBarcodesModel = model.FirstOrDefault();
            if (printBarcodesModel != null)
            {
                var pdfBarcodes = printBarcodesModel.ConvertToPdfBarcodeModelList(model);
                var doc = PrintBarcodes.PrintBarcodesItems(pdfBarcodes.ToList());
                
                var ms = new MemoryStream();

                string fileName = "barcodes.pdf";

                doc.SaveToStream(ms);
                var response = HttpContext.Current.Response;
                

                //save stream as file by using spire method
                //doc.SaveToHttpResponse(fileName, response, HttpReadType.Save);

                //save stream as file by using BinaryWrite
                byte[] buf = new byte[ms.Length];  //declare arraysize
                ms.Read(buf, 0, buf.Length);
                response.AddHeader("Accept-Header", ms.Length.ToString());
                response.ContentType = "application/octet-stream";
                response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                response.AddHeader("Content-Length", ms.Length.ToString());
                response.AddHeader("x-filename", fileName);
                response.BinaryWrite(ms.ToArray());
                response.End();

                ms.Flush();
                ms.Close();

                return response;
                
            }
            return null;
            //return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            //return Ok("There was a problem");
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("AddItem")]
        public IHttpActionResult AddItem(AddItemModel model)
        {
            var output = new AddItemApiOutput();
            
            if (!ModelState.IsValid)
            {
                output.Message = "Invalid input";
                return Ok(output);
            }

            //Find Consignor by username
            Consignor consignor;
            using (var repo = new ConsignorRepository())
            {
                var app = new ConsignorAppService(repo);
                consignor = app.GetConsignorByEmail(new GetConsignorByEmailInput() {Email = model.EmailAddress}).Consignor.ConvertToConsignor();
            }

            if (model.ItemType == "book")
            {
                //Add Book
                model.Isbn = model.Isbn.IsNullOrEmpty() ? "9999999999" : model.Isbn;
                output = AddBook(model, consignor);
                output.DateAdded = DateTime.Now.ToShortDateString();
                return Ok(output);
            }

            if (model.ItemType == "video")
            {
                //Add Video
                output = AddVideo(model, consignor);
                output.DateAdded = DateTime.Now.ToShortDateString();
                return Ok(output);
            }

            if ((model.ItemType == "game") || (model.ItemType == "other") || (model.ItemType == "teachingAide"))
            {
                //Add Item
                bool result = false;
                if (model.ItemType == "game")
                {
                    var game = new Game()
                    {
                        Title = model.Title,
                        Items_TItems = new Collection<Item>
                        {
                            new Item
                            {
                                ListedPrice = model.Price,
                                IsDiscountable = model.Discounted == "isDiscounted",
                                ItemType = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.ItemType),
                                ListedDate = DateTime.Now,
                                Status = "Not yet arrived in store",
                                Subject = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.Subject),
                                ConsignorId = consignor.Id,
                                Condition = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.Condition)
                            }
                        }
                    };
                    output = AddItem(model, consignor, game);
                }
                if (model.ItemType == "other")
                {
                    var other = new Other()
                    {
                        Title = model.Title,
                        Items_TItems = new Collection<Item>
                        {
                            new Item
                            {
                                ListedPrice = model.Price,
                                IsDiscountable = model.Discounted == "isDiscounted",
                                ItemType = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.ItemType),
                                ListedDate = DateTime.Now,
                                Status = "Not yet arrived in store",
                                Subject = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.Subject),
                                ConsignorId = consignor.Id,
                                Condition = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.Condition)
                            }
                        }
                    };
                    output = AddItem(model, consignor, other);
                }
                if (model.ItemType == "teachingAide")
                {
                    var teachingAide = new TeachingAide()
                    {
                        Title = model.Title,
                        Items_TItems = new Collection<Item>
                        {
                            new Item
                            {
                                ListedPrice = model.Price,
                                IsDiscountable = model.Discounted == "isDiscounted",
                                ItemType = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.ItemType),
                                ListedDate = DateTime.Now,
                                Status = "Not yet arrived in store",
                                Subject = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.Subject),
                                ConsignorId = consignor.Id,
                                Condition = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.Condition)
                            }
                        }
                    };
                    output = AddItem(model, consignor, teachingAide);
                }
                
                output.DateAdded = DateTime.Now.ToShortDateString();
                return Ok(output);
            }

            return Ok("Addition of book to database failed!");
        }



        #region Helpers

        private AddItemApiOutput AddBook(AddItemModel model, Consignor consignor)
        {
            var output = new AddItemApiOutput();

            var book = new Book()
            {
                ISBN = model.Isbn ?? "9999999999",
                Title = model.Title,
                Items_TItems = new Collection<Item>
                    {
                        new Item
                        {
                            ListedPrice = model.Price,
                            IsDiscountable = model.Discounted == "isDiscounted",
                            ItemType = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.ItemType),
                            ListedDate = DateTime.Now,
                            Status = "Not yet arrived in store",
                            Subject = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.Subject),
                            ConsignorId = consignor.Id,
                            Condition = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.Condition)
                        }
                    }
            };

            //book.Items_TItems.Add(item);


            var itemInput = new GetItemByIdInput();
            var barcodeInput = new SetItemBarcodeInput();
            var updateInput = new UpdateItemInput();

            var input = new AddNewBookInput
            {
                Book = new BookDto(book)
            };
            using (var repo = new BookRepository())
            {
                var app = new BookAppService(repo);
                itemInput.Id = app.AddNewBook(input).Id;
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
                output.Barcode = barcodeOutput.Barcode;
            }

            output.Message = itemInput.Id != 0 ? "success" : "Failed to add item";

            return output;
        }

        private AddItemApiOutput AddVideo(AddItemModel model, Consignor consignor)
        {
            var output = new AddItemApiOutput();
            
            var video = new Video()
            {
                Title = model.Title,
                VideoFormat = model.VideoFormat.ToUpper(),
                Items_TItems = new Collection<Item>
                    {
                        new Item
                        {
                            ListedPrice = model.Price,
                            IsDiscountable = model.Discounted == "isDiscounted",
                            ItemType = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.ItemType),
                            ListedDate = DateTime.Now,
                            Status = "Not yet arrived in store",
                            Subject = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.Subject),
                            ConsignorId = consignor.Id,
                            Condition = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model.Condition)
                        }
                    }
            };

            //book.Items_TItems.Add(item);


            var itemInput = new GetItemByIdInput();
            var barcodeInput = new SetItemBarcodeInput();
            var updateInput = new UpdateItemInput();

            var input = new VideoDto.AddNewVideoInput
            {
                Video = new VideoDto(video)
            };
            using (var repo = new VideoRepository())
            {
                var app = new VideoAppService(repo);
                itemInput.Id = app.AddNewVideo(input).Id;
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
                output.Barcode = barcodeOutput.Barcode;
            }


            output.Message = itemInput.Id != 0 ? "success" : "Failed to add item";

            return output;
        }

        private AddItemApiOutput AddItem<T>(AddItemModel model, Consignor consignor, T genericItem)
             where T : class, IGenericItem
        {
            var output = new AddItemApiOutput();

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
                output.Barcode = barcodeOutput.Barcode;
            }


            output.Message = itemInput.Id != 0 ? "success" : "Failed to add item";

            return output;
        }

        #endregion
    }
}
