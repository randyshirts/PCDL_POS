

using System.Collections.Generic;
using POS.Controller.Elements;

namespace POS.Controller.Interfaces
{
    public interface IReceiptVisitor
    {
        //bool Visit(IItemElement);
        string Visit(SaleItem saleItem);
        //saleClass
        //rentRoom
    }


}
