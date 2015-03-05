

using POS.Controller.Elements;

namespace POS.Controller.Interfaces
{
    public interface ITaxVisitor
    {
        //bool Visit(IItemElement);
        double Visit(SaleItem saleItem);
        //int Visit(GameItem gameItem);
        //int Visit(OtherItem otherItem);
        //int Visit(VideoItem videoItem);
        //int Visit(TeachingAideItem teachingAideItem);
        //int Visit(ConsignorItem consignorItem);
    }


}
