using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RocketPos.Common.AmazonAWS;

namespace RocketPos.Common.Helpers
{
    // wrapper class for data returned from Amazon Searches
    public class AmazonItemSearchInfo
    {
        public AmazonItemSearchInfo()
        {
            Isbn = new List<string>();
            Title = new List<string>();
            Ean = new List<string>();
            ListPrice = new List<string>();
            Manufacturer = new List<string>();
            Publisher = new List<string>();
            Format = new List<string>();
            Rating = new List<string>();
            Image = new List<Image>();
            Author = new List<string>();
            Binding = new List<string>();
            NumberOfPages = new List<int>();
            PublicationDate = new List<DateTime>();
            TradeInValue = new List<double>();
        }

        public List<string> Isbn { get; set; }
        public List<string> Title { get; set; }
        public List<Image> Image { get; set; }
        public List<string> Ean { get; set; }
        public List<string> ListPrice { get; set; }
        public List<string> Manufacturer { get; set; }
        public List<string> Publisher { get; set; }
        public List<string> Format { get; set; }
        public List<string> Rating { get; set; }
        public List<string> Author { get; set; }
        public List<string> Binding { get; set; }
        public List<int> NumberOfPages { get; set; }
        public List<DateTime> PublicationDate { get; set; }
        public List<double> TradeInValue { get; set; }

        // method to fill up my wrapper class using an ISBN
        public AmazonItemSearchInfo GetProductByIsbn(AWSECommerceServicePortTypeClient amazonClient, string isbn)
        {
            //Logger.WriteToLog("Entered method: GetProductByISBN");

            //AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();

            //Create ItemSearch Wrapper
            const string ak2 = "3BM4NOYUORFUQ";
            const string at2 = "40 - 4314";
            var search = new ItemSearch
            {
                AWSAccessKeyId = ConfigSettings.accessKeyId + ak2,
                AssociateTag = ConfigSettings.ASSOCIATE_TAG + at2,
            };


            //Create Request object
            var request = new ItemSearchRequest
            {
                SearchIndex = "Books",
                //Power = "isbn : " + isbn + "*",
                Power = "isbn : " + isbn + "* or " + "ean : " + isbn + "*",
                ResponseGroup = new[] {"Medium"},
                //Availability = 0,
                //AvailabilitySpecified = false
                
            };
            //request.Keywords = isbn;

            //request.ResponseGroup = new string[] { "OfferFull", "Images", "ItemAttributes" };

            search.Request = new[] { request };

            //Perform search
            ItemSearchResponse response = amazonClient.ItemSearch(search);

            //Get necessary info from response
            AmazonItemSearchInfo myInfo = null;
            var dateTime = new DateTime();

            //Get a list of results
            if (response.Items[0].Item != null)
            {
                myInfo = new AmazonItemSearchInfo();
                for (int i = 0; i < response.Items[0].Item.Count(); i++)
                {
                    myInfo.Title.Add(response.Items[0].Item[i].ItemAttributes.Title ?? null);
                    myInfo.Image.Add(response.Items[0].Item[i].MediumImage ?? null);
                    myInfo.ListPrice.Add(response.Items[0].Item[i].ItemAttributes.ListPrice != null ? response.Items[0].Item[i].ItemAttributes.ListPrice.FormattedPrice : null);
                    myInfo.Ean.Add(response.Items[0].Item[i].ItemAttributes.EAN ?? null);                   
                    myInfo.Isbn.Add(response.Items[0].Item[i].ItemAttributes.ISBN ?? isbn);
                    if (myInfo.Ean[i].EndsWith(myInfo.Isbn[i]))
                        myInfo.Isbn[i] = myInfo.Ean[i];
                    myInfo.Author.Add(response.Items[0].Item[i].ItemAttributes.Author != null ? response.Items[0].Item[i].ItemAttributes.Author[0] : null);
                    myInfo.Binding.Add(response.Items[0].Item[i].ItemAttributes.Binding ?? null);
                    myInfo.NumberOfPages.Add(response.Items[0].Item[i].ItemAttributes.NumberOfPages != null ? Int32.Parse(response.Items[0].Item[i].ItemAttributes.NumberOfPages) : 0);
                    string myDate = response.Items[0].Item[i].ItemAttributes.PublicationDate;
                    if (myDate != null)
                    {

                        if (myDate.Length == 4)
                            dateTime = DateTime.ParseExact(myDate, "yyyy", CultureInfo.InvariantCulture);
                        else
                            DateTime.TryParse(response.Items[0].Item[i].ItemAttributes.PublicationDate, out dateTime);
                        
                        myInfo.PublicationDate.Add(dateTime);
                    }
                    myInfo.TradeInValue.Add(response.Items[0].Item[i].ItemAttributes.TradeInValue != null ? Double.Parse(response.Items[0].Item[i].ItemAttributes.TradeInValue.FormattedPrice.Replace("$", "")) : 0);
                }
            }

            return myInfo;
        }

        // 
        public AmazonItemSearchInfo GetProductByName(AWSECommerceServicePortTypeClient amazonClient, string name, string itemType)
        {
            //Logger.WriteToLog("Entered method: GetProductByISBN");

            //Create ItemSearch Wrapper
            const string ak2 = "3BM4NOYUORFUQ";
            const string at2 = "40 - 4314";
            var search = new ItemSearch
            {
                AWSAccessKeyId = ConfigSettings.accessKeyId + ak2,
                AssociateTag = ConfigSettings.ASSOCIATE_TAG + at2
            };

            //Create Request object
            var request = new ItemSearchRequest();
            if (itemType == "Video")
                request.Keywords = name + " -Instant";
            else
                request.Keywords = name;
            request.SearchIndex = AmazonCommonHelpers.GetSearchIndex(itemType);


            //request.ResponseGroup = new string[] { "OfferFull", "Images", "ItemAttributes" };
            request.ResponseGroup = new[] { "Medium" };

            search.Request = new[] { request };

            //Perform search
            var response = amazonClient.ItemSearch(search);

            //declare object of this class to store results
            AmazonItemSearchInfo myInfo = null;


            //Get a list of results
            if (response.Items[0].Item != null)
            {
                myInfo = new AmazonItemSearchInfo();
                for (int i = 0; i < response.Items[0].Item.Count(); i++)
                {
                    myInfo.Title.Add(response.Items[0].Item[i].ItemAttributes.Title ?? name);
                    myInfo.Image.Add(response.Items[0].Item[i].MediumImage ?? null);
                    myInfo.ListPrice.Add(response.Items[0].Item[i].ItemAttributes.ListPrice != null ? response.Items[0].Item[i].ItemAttributes.ListPrice.FormattedPrice : null);
                    myInfo.Ean.Add(response.Items[0].Item[i].ItemAttributes.EAN);
                    switch (itemType)
                    {
                        case "Book":
                            {
                                myInfo.Isbn.Add(response.Items[0].Item[i].ItemAttributes.ISBN);
                                myInfo.Author.Add(response.Items[0].Item[i].ItemAttributes.Author[0] ?? null);
                                myInfo.Binding.Add(response.Items[0].Item[i].ItemAttributes.Binding ?? null);
                                myInfo.NumberOfPages.Add(response.Items[0].Item[i].ItemAttributes.NumberOfPages != null ? Int32.Parse(response.Items[0].Item[i].ItemAttributes.NumberOfPages) : 0);
                                myInfo.PublicationDate.Add(response.Items[0].Item[i].ItemAttributes.PublicationDate != null ? DateTime.Parse(response.Items[0].Item[i].ItemAttributes.PublicationDate) : DateTime.Now.Date);
                                myInfo.TradeInValue.Add(response.Items[0].Item[i].ItemAttributes.TradeInValue != null ? Double.Parse(response.Items[0].Item[i].ItemAttributes.TradeInValue.FormattedPrice.Replace("$", "")) : 0);
                                break;
                            }
                        case "Game":
                            {
                                myInfo.Manufacturer.Add(response.Items[0].Item[i].ItemAttributes.Manufacturer);
                                myInfo.Ean.Add(response.Items[0].Item[i].ItemAttributes.EAN);
                                break;
                            }
                        case "Other":
                            {
                                myInfo.Manufacturer.Add(response.Items[0].Item[i].ItemAttributes.Manufacturer);
                                myInfo.Ean.Add(response.Items[0].Item[i].ItemAttributes.EAN);
                                break;
                            }
                        case "TeachingAide":
                            {
                                myInfo.Manufacturer.Add(response.Items[0].Item[i].ItemAttributes.Manufacturer);
                                myInfo.Ean.Add(response.Items[0].Item[i].ItemAttributes.EAN);
                                break;
                            }
                        case "Video":
                            {
                                myInfo.Publisher.Add(response.Items[0].Item[i].ItemAttributes.Publisher);
                                myInfo.Ean.Add(response.Items[0].Item[i].ItemAttributes.EAN);
                                myInfo.Format.Add(response.Items[0].Item[i].ItemAttributes.Binding);
                                myInfo.Rating.Add(response.Items[0].Item[i].ItemAttributes.AudienceRating);
                                break;
                            }
                    }


                }
            }

            return myInfo;
        }
    }

}
