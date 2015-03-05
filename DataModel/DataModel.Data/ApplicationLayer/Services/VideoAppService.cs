using System.Linq;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class VideoAppService : GenericItemAppService<Video>, IVideoAppService
    {
        private readonly IVideoRepository _videoRepository;

        public VideoAppService(IVideoRepository videoRepository)
            : base(videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public VideoDto.GetAllVideosOutput GetAllVideos()
        {
            return new VideoDto.GetAllVideosOutput
            {
                Videos = _videoRepository.GetAllVideos().Select(i => new VideoDto(i)).ToList()
            }; 
        }

        public VideoDto.AddNewVideoOutput AddNewVideo(VideoDto.AddNewVideoInput input)
        {
            return new VideoDto.AddNewVideoOutput
            {
                Id = _videoRepository.AddNewItem(input.Video.ConvertToVideo())
            };
        }

        public VideoDto.UpdateVideoOutput UpdateVideo(VideoDto.UpdateVideoInput input)
        {
            return new VideoDto.UpdateVideoOutput
            {
                Id = _videoRepository.UpdateItem(input.Video.ConvertToVideo())
            };
        }
    }
}
