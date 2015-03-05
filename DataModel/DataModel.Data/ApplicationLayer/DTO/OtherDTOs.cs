using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class OtherDto : GenericItemDto<Other>
    {
        public OtherDto(Other other)
            : base(other)
        {
        }

        public Other ConvertToOther()
        {
            return new Other()
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
