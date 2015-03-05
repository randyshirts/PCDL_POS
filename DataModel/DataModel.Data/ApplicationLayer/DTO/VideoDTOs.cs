using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class VideoDto : GenericItemDto<Video>
    {
        public VideoDto(Video video)
            : base(video)
        {
            Id = video.Id;
            Title = video.Title;
            Manufacturer = video.Manufacturer;
            Ean = video.EAN;
            ItemImage = video.ItemImage;
            Items_TItems = video.Items_TItems;
            Publisher = video.Publisher;
            VideoFormat = video.VideoFormat;
            AudienceRating = video.AudienceRating;
        }

        public string Publisher { get; set; }
        [Required]
        public string VideoFormat { get; set; }
        public string AudienceRating { get; set; }
        public Video ConvertToVideo()
        {
            return new Video()
            {
                Id = Id,
                Title = Title,
                Manufacturer = Manufacturer,
                EAN = Ean,
                Publisher = Publisher,
                VideoFormat = VideoFormat,
                AudienceRating = AudienceRating,
                ItemImage = ItemImage,
                Items_TItems = Items_TItems
            };
        }

        public class AddNewVideoOutput : IOutputDto
        {
            public int Id { get; set; }
        }

        public class AddNewVideoInput : IInputDto
        {
            public VideoDto Video { get; set; }
        }

        public class GetAllVideosOutput : IOutputDto
        {
            public List<VideoDto> Videos { get; set; }
        }

        public class UpdateVideoOutput : IOutputDto
        {
            public int Id { get; set; }
        }

        public class UpdateVideoInput : IInputDto
        {
            public VideoDto Video { get; set; }
        }
    }

    
}
