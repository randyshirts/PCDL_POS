using Abp.Application.Services;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IVideoAppService : IApplicationService
    {
        VideoDto.GetAllVideosOutput GetAllVideos();
        VideoDto.AddNewVideoOutput AddNewVideo(VideoDto.AddNewVideoInput input);
        VideoDto.UpdateVideoOutput UpdateVideo(VideoDto.UpdateVideoInput input);
    }
}
