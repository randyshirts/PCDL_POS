using System;
using POS.Controller.Elements;

namespace POS.Controller.Interfaces
{
    public interface ISaleElement
    {
        
        //bool Accept(IItemVisitor visitor);
        double Accept(ITaxVisitor itemVisitor);
        string Accept(IReceiptVisitor visitor);
    }


}
