using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Common.AmazonAWS;
using Amazon.PAAPI.WCF;
using RocketPos.Common.Helpers;
using System.Threading.Tasks;

namespace RocketPos.Tests.CommonTests.FunctionalTests
{
    [TestClass]
    public class AmazonItemLookupTests
    {
        [TestMethod]
        public void ClientCreationIsSuccessful()
        {
            try
            {
                AWSECommerceServicePortTypeClient amazonClient = AmazonCommonHelpers.CreateAmazonClient();
                Assert.IsNotNull(amazonClient);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }

            
        }

        [TestMethod]
        public void ItemSearchSuccessfullyReturnsBook()
        {
            string isbn = "1561794147";
            AWSECommerceServicePortTypeClient searchClient = null;

            try
            {
                searchClient = AmazonCommonHelpers.CreateAmazonClient();    
                AmazonItemSearchInfo search = new AmazonItemSearchInfo();
                search = search.GetProductByIsbn(searchClient, isbn);

                Assert.IsNotNull(search);
                Assert.IsTrue(search.Title.Contains("The Way They Learn"));
                Assert.IsTrue(search.Author.Contains("Cynthia Ulrich Tobias"));
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
         
        }

        [TestMethod]
        public void GetProductByNameSuccessfullyReturnsGame()
        {
            string name = "Uno";
            AWSECommerceServicePortTypeClient searchClient = null;

            try
            {
                searchClient = AmazonCommonHelpers.CreateAmazonClient();
                AmazonItemSearchInfo search = new AmazonItemSearchInfo();
                search = search.GetProductByName(searchClient, name, "Game");

                Assert.IsNotNull(search);
                Assert.IsTrue(search.Title.Contains("Original UNO Card Game"));
                Assert.IsTrue(search.Manufacturer.Contains("Mattel Games"));
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }

        }

        [TestMethod]
        public void GetProductByNameSuccessfullyReturnsTeachingAide()
        {
            string name = "Rubik's Cube";
            AWSECommerceServicePortTypeClient searchClient = null;

            try
            {
                searchClient = AmazonCommonHelpers.CreateAmazonClient();
                AmazonItemSearchInfo search = new AmazonItemSearchInfo();
                search = search.GetProductByName(searchClient, name, "TeachingAide");

                Assert.IsNotNull(search);
                Assert.IsTrue(search.Title.Contains("Rubik's Cube"));
                Assert.IsTrue(search.Manufacturer.Contains("Winning Moves Games"));
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }

        }

        [TestMethod]
        public void GetProductByNameSuccessfullyReturnsVideo()
        {
            string name = "Big Jake";
            AWSECommerceServicePortTypeClient searchClient = null;

            try
            {
                searchClient = AmazonCommonHelpers.CreateAmazonClient();
                AmazonItemSearchInfo search = new AmazonItemSearchInfo();
                search = search.GetProductByName(searchClient, name, "Video");

                Assert.IsNotNull(search);
                Assert.IsTrue(search.Title.Contains("Big Jake"));
                Assert.IsTrue(search.Publisher.Contains("Paramount"));
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }

        }

        [TestMethod]
        public void GetProductByIsbnReturnsNullIfNothingFound()
        {
            const string isbn = "156179415123";
            AWSECommerceServicePortTypeClient searchClient = null;

            try
            {
                searchClient = AmazonCommonHelpers.CreateAmazonClient();
                AmazonItemSearchInfo search = new AmazonItemSearchInfo();
                search = search.GetProductByIsbn(searchClient, isbn);

                Assert.IsNull(search);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }

        [TestMethod]
        public void GetProductByNameReturnsNullIfNothingFound()
        {
            string isbn = "156179415123";
            AWSECommerceServicePortTypeClient searchClient = null;

            try
            {
                searchClient = AmazonCommonHelpers.CreateAmazonClient();
                AmazonItemSearchInfo search = new AmazonItemSearchInfo();
                search = search.GetProductByName(searchClient, isbn, "Book");

                Assert.IsNull(search);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }




    }
}
