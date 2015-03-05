using System;
using System.Collections;
using System.Windows;
using Inventory.Controller.Interfaces;
using RocketPos.Common.AmazonAWS;
using RocketPos.Common.Helpers;

namespace Inventory.Controller.CustomClasses
{
    public static class SearchAmazon
    {
        private static readonly AWSECommerceServicePortTypeClient SearchClient;

        static SearchAmazon()
        {
            SearchClient = AmazonCommonHelpers.CreateAmazonClient(); 
        }


        public static AmazonItemSearchInfo SearchAmazonByIsbn(string isbn)
        {
            try
            {
                if (isbn.Length <= 4) return null;
                var search = new AmazonItemSearchInfo();
                if (SearchClient != null)
                {
                    search = search.GetProductByIsbn(SearchClient, isbn);
                }
                else
                {
                    throw new Exception(
                        "SearchAmazonByIsbn - Amazon client is null, connection with amazon failed");
                }
                return search;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SearchAmazonByIsbn Exception");
                return null;
            }

        }

        //Does ItemLookup - returns one or none results
        public static IItemElement QueryAmazonByIsbn(IItemElement itemElement, string isbn)
        {
            try
            {
                var lookup = new AmazonItemLookupInfo();
                lookup = lookup.GetProductByIsbn(SearchClient, isbn);
                if (lookup != null)
                {
                    if (lookup.LowestNewPrice != null) itemElement.LowestNewPrice = (double) lookup.LowestNewPrice;
                    if (lookup.LowestUsedPrice != null) itemElement.LowestUsedPrice = (double) lookup.LowestUsedPrice;
                }
                else
                {
                    itemElement.LowestNewPrice = 0;
                    itemElement.LowestUsedPrice = 0;
                }
                return itemElement;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "QueryAmazonByISBN Exception");
                return null;
            }
        }

        public static AmazonItemSearchInfo SearchAmazonByTitle(string title, string itemType)
        {
            try
            {
                if (title.Length <= 3) return null;
                var search = new AmazonItemSearchInfo();
                if (SearchClient != null)
                {
                    search = search.GetProductByName(SearchClient, title, itemType);
                }
                else
                {
                    throw new Exception(
                        "SearchAmazonByTitle - Amazon client is null, connection with amazon failed");
                }
                return search;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SearchAmazon Exception");
                return null;
            }

        }

        public static IItemElement QueryAmazonByEan(IItemElement itemElement, string ean, string itemType)
        {
            try
            {
                var lookup = new AmazonItemLookupInfo();
                lookup = lookup.GetProductByEan(SearchClient, ean, itemType);
                if (lookup != null)
                {
                    if (lookup.LowestNewPrice != null) itemElement.LowestNewPrice = (double)lookup.LowestNewPrice;
                    if (lookup.LowestUsedPrice != null) itemElement.LowestUsedPrice = (double)lookup.LowestUsedPrice;
                }
                else
                {
                    itemElement.LowestNewPrice = 0;
                    itemElement.LowestUsedPrice = 0;

                }
                return itemElement;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "QueryAmazonByEAN Exception");
                return null;
            }
        }
    }
}
