using System.Collections.Generic;
using System.Linq;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;


namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public class VideoController : GenericItemController<Video>, IVideoController
    {
        public override int AddNewItem(Video video)
        {

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
            }
            return itemInput.Id;
        }

        public override IEnumerable<Video> GetAllItems()
        {
            using (var repo = new VideoRepository())
            {
                var app = new VideoAppService(repo);
                var output = app.GetAllVideos();
                return output.Videos.Select(video => video.ConvertToVideo()).ToList();
            }
        }

        public override int UpdateItem(Video updatedVideo)
        {
            var input = new VideoDto.UpdateVideoInput
            {
                Video = new VideoDto(updatedVideo)
            };
            using (var repo = new VideoRepository())
            {
                var app = new VideoAppService(repo);
                return app.UpdateVideo(input).Id;
            }
        }
    }
}
