using System;
using System.Collections.Generic;
using System.Linq;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class VideoRepository : GenericItemRepositoryBase<Video>, IVideoRepository
    {
        public override int AddNewItem(Video video)
        {
            if (video.Items_TItems.ElementAt(0).Video != null)
            {
                throw new NullReferenceException("Item member Video must be null when adding item or SQL will throw exception saying violated relationship property");
            }

            //Give dummy value for barcode
            if (video.Items_TItems != null)
            {
                video.Items_TItems.ElementAt(0).Barcode = "9999999999999";
            }

            //Validation if a record already exists for this Video
            var oldVideo = GetItemByTitle(video.Title);
            if (oldVideo != null)
            {

                Validation.ItemDuplicate(video, oldVideo);

                //Update Items property
                if (video.Items_TItems != null && video.Items_TItems.Count == 1)
                    oldVideo.Items_TItems.Add(video.Items_TItems.ElementAt(0));     //Add new item to old video record
                else
                    throw new ArgumentException("Can't add more than one item to a Video at the same time, or Video.Items is null");
            }
            //Video doesn't exist so add it
            else
            {
                Validation.StringRequire(video.Title);
                Validation.TitleLength(video.Title);
                Validation.ItemDuplicate(video);
                Validation.EanLength(video.EAN);
                Validation.VideoFormatLength(video.VideoFormat);
                Validation.AudienceRatingLength(video.AudienceRating);
                Context.Videos.Add(video);
            }
            Context.SaveChanges();

            //Set Barcode here because we need Id
            if (video.Items_TItems != null)
            {
                var itemId = video.Items_TItems.ElementAt(0).Id;
                return itemId;
            }
            return -1;
        }

        public IEnumerable<Video> GetAllVideos()
        {
            var query = from b in Context.Videos
                        orderby b.Title
                        select b;

            return query.ToList();
        }
       
    }
}
