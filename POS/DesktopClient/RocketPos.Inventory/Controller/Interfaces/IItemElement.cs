using System;
using Inventory.Controller.Elements.ItemElements;

namespace Inventory.Controller.Interfaces
{
    public interface IItemElement
    {
        double ListedPrice { get; set; }
        DateTime ListedDate { get; set; }
        string Subject { get; set; }
        string ItemStatus { get; set; }
        string Condition { get; set; }
        bool IsDiscountable { get; set; }
        string Description { get; set; }
        string Barcode { get; set; }
        double LowestNewPrice { get; set; }
        double LowestUsedPrice { get; set; }
        
        //bool Accept(IItemVisitor visitor);
        int Accept(IItemVisitor itemVisitor);

    }


}
