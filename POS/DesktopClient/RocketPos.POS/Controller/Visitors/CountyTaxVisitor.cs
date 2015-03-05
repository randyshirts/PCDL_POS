using POS.Controller.Elements;
using POS.Controller.Interfaces;

namespace POS.Controller.Visitors
{
    public class CountyTaxVisitor : ITaxVisitor
    {
        public double Visit(SaleItem saleItem)
        {
            //http://madisoncountyal.gov/about/org/CoDepts/SalesTax.shtml
            
            //County
            return .005;

        }

        //Classes are a service
        //public double Visit(SaleClass saleClass)
        //{
        //    //Do not pay sales tax
        //    return 0;
        //}

        //public double Visit(rentalSpace )
        //{
        //    return 0;
        //}


        
    }
}
