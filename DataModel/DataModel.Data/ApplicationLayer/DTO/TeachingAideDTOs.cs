using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class TeachingAideDto : GenericItemDto<TeachingAide>
    {
        public TeachingAideDto(TeachingAide teachingAide)
            : base(teachingAide)
        {
        }

        public TeachingAide ConvertToTeachingAide()
        {
            return new TeachingAide()
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
