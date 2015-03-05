using System.Collections.Generic;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.WpfControllers
{
    public interface IVideoController
    {
        int UpdateItem(Video updatedVideo);
        int AddNewItem(Video video);
        IEnumerable<Video> GetAllItems();
    }
}
