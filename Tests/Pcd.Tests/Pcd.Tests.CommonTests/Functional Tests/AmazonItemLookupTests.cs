using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Common.AmazonAWS;
using Amazon.PAAPI.WCF;
using RocketPos.Common.Helpers;
using System.Threading.Tasks;

namespace RocketPos.Tests.CommonTests.FunctionalTests
{
    [TestClass]
    public class AmazonItemSearchTests
    {

        [TestMethod]
        public void ItemLookupSuccessfullyReturnsBook()
        {
            string isbn = "1561794147";
            AWSECommerceServicePortTypeClient searchClient = null;

            try
            {
                searchClient = AmazonCommonHelpers.CreateAmazonClient();    
                AmazonItemLookupInfo lookup = new AmazonItemLookupInfo();
                lookup = lookup.GetProductByIsbn(searchClient, isbn);

                Assert.IsNotNull(lookup);
                Assert.IsTrue(lookup.Title == "The Way They Learn");
                Assert.IsTrue(lookup.Authors == "Cynthia Ulrich Tobias");
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
         
        }

        [TestMethod]
        public void GetProductByIsbnReturnsNullIfNothingFound()
        {
            string isbn = "156179414";
            AWSECommerceServicePortTypeClient searchClient = null;

            try
            {
                searchClient = AmazonCommonHelpers.CreateAmazonClient();   
                AmazonItemLookupInfo lookup = new AmazonItemLookupInfo();
                lookup = lookup.GetProductByIsbn(searchClient, isbn);

                Assert.IsNull(lookup);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }




    }
}
