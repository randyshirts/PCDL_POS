using System;
using System.Linq;
using RocketPos.Common.AmazonAWS;

namespace RocketPos.Common.Helpers
{
    // wrapper class for data returned from Amazon 
    public class AmazonItemLookupInfo
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Binding { get; set; }
        public string Edition { get; set; }
        public string Authors { get; set; }
        public string Manufacturer { get; set; }
        public string Publisher { get; set; }
        public double? LowestUsedPrice { get; set; }
        public double? LowestNewPrice { get; set; }
        public double? TradeInValue { get; set; }
        public Image Image { get; set; }
        public string AmazonLink { get; set; }
        public string Ean { get; set; }
        public string ListPrice { get; set; }


        // method to fill up my wrapper class using an ISBN
        public AmazonItemLookupInfo GetProductByIsbn(AWSECommerceServicePortTypeClient amazonClient, string isbn)
        {
            //Logger.WriteToLog("Entered method: GetProductByISBN");

            ItemLookupRequest request = new ItemLookupRequest
            {
                IdType = ItemLookupRequestIdType.ISBN,
                IdTypeSpecified = true,
                Id = new[] {isbn},
                SearchIndex = "Books",
                ResponseGroup = new[] {"OfferFull", "ItemAttributes"}
            };
            //request.IdType = ItemLookupRequestIdType.ISBN;
            //request.Operation=ItemLookup
            var lookup = new ItemLookup {Request = new[] {request}};
            const string ak2 = "3BM4NOYUORFUQ";
            const string at2 = "40 - 4314";
            lookup.AWSAccessKeyId = ConfigSettings.accessKeyId + ak2;
            lookup.AssociateTag = ConfigSettings.ASSOCIATE_TAG + at2;


            var response = amazonClient.ItemLookup(lookup);

            if (response.Items[0].Item == null) return null;
            //Get the first result with an image
            var myItem = response.Items[0].Item.FirstOrDefault(i => i.ItemAttributes.Title != null);

            if (myItem == null) return null;
            var myInfo = new AmazonItemLookupInfo()
            {
                Authors = myItem.ItemAttributes.Author != null ? String.Join(", ", myItem.ItemAttributes.Author) : null,
                Binding = myItem.ItemAttributes.Binding,
                Edition = myItem.ItemAttributes.Edition,
                Image = myItem.MediumImage,
                Isbn = myItem.ItemAttributes.ISBN,
                Manufacturer = myItem.ItemAttributes.Manufacturer,
                LowestUsedPrice = myItem.OfferSummary.LowestUsedPrice != null ? double.Parse(myItem.OfferSummary.LowestUsedPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                LowestNewPrice = myItem.OfferSummary.LowestNewPrice != null ? double.Parse(myItem.OfferSummary.LowestNewPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                Title = myItem.ItemAttributes.Title,
                TradeInValue = myItem.ItemAttributes.TradeInValue != null ? (double?)double.Parse(myItem.ItemAttributes.TradeInValue.FormattedPrice.Replace("$", "")) : null,
                AmazonLink = response.Items[0].Item[0].DetailPageURL
            };
            if (Isbn == null) Isbn = isbn;
            //Logger.WriteToLog("Exiting method: GetProductByISBN");
            return myInfo;
        }

        // method to fill up my wrapper class using an EAN
        public AmazonItemLookupInfo GetProductByEan(AWSECommerceServicePortTypeClient amazonClient, string ean, string itemType)
        {
            //Logger.WriteToLog("Entered method: GetProductByEAN");

            ItemLookupRequest request = new ItemLookupRequest
            {
                IdType = ItemLookupRequestIdType.EAN,
                IdTypeSpecified = true,
                Id = new[] {ean},
                SearchIndex = AmazonCommonHelpers.GetSearchIndex(itemType),
                ResponseGroup = new[] {"OfferFull", "ItemAttributes"}
            };

            const string ak2 = "3BM4NOYUORFUQ";
            const string at2 = "40 - 4314";
            var lookup = new ItemLookup
            {
                Request = new[] {request},
                AWSAccessKeyId = ConfigSettings.accessKeyId + ak2,
                AssociateTag = ConfigSettings.ASSOCIATE_TAG + at2
            };


            ItemLookupResponse response = amazonClient.ItemLookup(lookup);

            AmazonItemLookupInfo myInfo = null;

            if (response.Items[0].Item != null)
            {
                //Get the first result with an image
                var myItem = response.Items[0].Item.FirstOrDefault(i => i.ItemAttributes.Title != null);

                if (myItem != null)
                {
                    switch(itemType)
                    {
                        case "Book":
                            {
                                myInfo = new AmazonItemLookupInfo()
                                {
                                    Authors = response.Items[0].Item[0].ItemAttributes.Author != null ? String.Join(", ", response.Items[0].Item[0].ItemAttributes.Author) : null,
                                    Binding = myItem.ItemAttributes.Binding,
                                    Edition = myItem.ItemAttributes.Edition,
                                    Ean = myItem.ItemAttributes.EAN,
                                    Isbn = myItem.ItemAttributes.ISBN,
                                    LowestUsedPrice = myItem.OfferSummary.LowestUsedPrice != null ? double.Parse(myItem.OfferSummary.LowestUsedPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                                    LowestNewPrice = myItem.OfferSummary.LowestNewPrice != null ? double.Parse(myItem.OfferSummary.LowestNewPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                                    Title = myItem.ItemAttributes.Title,
                                    TradeInValue = myItem.ItemAttributes.TradeInValue != null ? (double?)double.Parse(myItem.ItemAttributes.TradeInValue.FormattedPrice.Replace("$", "")) : null,
                                    AmazonLink = response.Items[0].Item[0].DetailPageURL
                                };
                                if (Ean == null) Ean = ean;
                                break;
                            }
                        case "Game":
                            {
                                myInfo = new AmazonItemLookupInfo()
                                {
                                    Ean = myItem.ItemAttributes.EAN,
                                    Manufacturer = myItem.ItemAttributes.Manufacturer,
                                    LowestUsedPrice = myItem.OfferSummary.LowestUsedPrice != null ? double.Parse(myItem.OfferSummary.LowestUsedPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                                    LowestNewPrice = myItem.OfferSummary.LowestNewPrice != null ? double.Parse(myItem.OfferSummary.LowestNewPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                                    Title = myItem.ItemAttributes.Title,
                                    TradeInValue = myItem.ItemAttributes.TradeInValue != null ? (double?)double.Parse(myItem.ItemAttributes.TradeInValue.FormattedPrice.Replace("$", "")) : null,
                                    AmazonLink = response.Items[0].Item[0].DetailPageURL
                                };
                                if (Ean == null) Ean = ean;
                                break;
                            }
                        case "Other":
                            {
                                myInfo = new AmazonItemLookupInfo()
                                {
                                    Ean = myItem.ItemAttributes.EAN,
                                    Manufacturer = myItem.ItemAttributes.Manufacturer,
                                    LowestUsedPrice = myItem.OfferSummary.LowestUsedPrice != null ? double.Parse(myItem.OfferSummary.LowestUsedPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                                    LowestNewPrice = myItem.OfferSummary.LowestNewPrice != null ? double.Parse(myItem.OfferSummary.LowestNewPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                                    Title = myItem.ItemAttributes.Title,
                                    TradeInValue = myItem.ItemAttributes.TradeInValue != null ? (double?)double.Parse(myItem.ItemAttributes.TradeInValue.FormattedPrice.Replace("$", "")) : null,
                                    AmazonLink = response.Items[0].Item[0].DetailPageURL
                                };
                                if (Ean == null) Ean = ean;
                                break;
                            }
                        case "TeachingAide":
                            {
                                myInfo = new AmazonItemLookupInfo()
                                {
                                    Ean = myItem.ItemAttributes.EAN,
                                    Manufacturer = myItem.ItemAttributes.Manufacturer,
                                    LowestUsedPrice = myItem.OfferSummary.LowestUsedPrice != null ? double.Parse(myItem.OfferSummary.LowestUsedPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                                    LowestNewPrice = myItem.OfferSummary.LowestNewPrice != null ? double.Parse(myItem.OfferSummary.LowestNewPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                                    Title = myItem.ItemAttributes.Title,
                                    TradeInValue = myItem.ItemAttributes.TradeInValue != null ? (double?)double.Parse(myItem.ItemAttributes.TradeInValue.FormattedPrice.Replace("$", "")) : null,
                                    AmazonLink = response.Items[0].Item[0].DetailPageURL
                                };
                                if (Ean == null) Ean = ean;
                                break;
                            }
                        case "Video":
                            {
                                myInfo = new AmazonItemLookupInfo()
                                {
                                    Ean = myItem.ItemAttributes.EAN,
                                    Publisher = myItem.ItemAttributes.Publisher,
                                    LowestUsedPrice = myItem.OfferSummary.LowestUsedPrice != null ? double.Parse(myItem.OfferSummary.LowestUsedPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                                    LowestNewPrice = myItem.OfferSummary.LowestNewPrice != null ? double.Parse(myItem.OfferSummary.LowestNewPrice.FormattedPrice.Replace("$", "")) : double.Parse(myItem.ItemAttributes.ListPrice.FormattedPrice.Replace("$", "")),
                                    Title = myItem.ItemAttributes.Title,
                                    TradeInValue = myItem.ItemAttributes.TradeInValue != null ? (double?)double.Parse(myItem.ItemAttributes.TradeInValue.FormattedPrice.Replace("$", "")) : null,
                                    AmazonLink = response.Items[0].Item[0].DetailPageURL
                                };
                                if (Ean == null) Ean = ean;
                                break;
                            }
                
                    }
                }
            }
            //Logger.WriteToLog("Exiting method: GetProductByISBN");
            return myInfo;
        }
    }

}
