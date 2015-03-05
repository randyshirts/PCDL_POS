using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class GameDto : GenericItemDto<Game>
    {
        public GameDto(Game game)
            : base(game)
        {
        }

        public Game ConvertToGame()
        {
            return new Game()
            {
                Id = Id,
                Title = Title,
                Manufacturer = Manufacturer,
                EAN = Ean,
                ItemImage = ItemImage,
                Items_TItems = Items_TItems,
            };
        }

    }

    
}
