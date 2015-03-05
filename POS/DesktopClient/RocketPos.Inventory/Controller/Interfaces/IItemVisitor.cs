

using Inventory.Controller.Elements.ItemElements;

namespace Inventory.Controller.Interfaces
{
    public interface IItemVisitor
    {
        //bool Visit(IItemElement);
        int Visit(BookItem bookItem);
        int Visit(GameItem gameItem);
        int Visit(OtherItem otherItem);
        int Visit(VideoItem videoItem);
        int Visit(TeachingAideItem teachingAideItem);
        int Visit(ConsignorItem consignorItem);
    }


}
