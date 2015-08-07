using System;
using POS.Controller.Elements;
using POS.Controller.Interfaces;

namespace POS.Controller.Visitors
{
    public class StateTaxVisitor : ITaxVisitor
    {
        public double Visit(SaleItem saleItem)
        {
            //http://madisoncountyal.gov/about/org/CoDepts/SalesTax.shtml
            
            //State
            if (((DateTime.Now >= DateTime.Parse("08/07/2015")) && (DateTime.Now < DateTime.Parse("08/09/2015")) && (saleItem.SaleAmount <= 30)))
                return 0;

            return .04;

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
