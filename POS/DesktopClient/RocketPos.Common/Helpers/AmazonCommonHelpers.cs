using System.ServiceModel;
using RocketPos.Common.AmazonAWS;



namespace RocketPos.Common.Helpers
{   
    public static class AmazonCommonHelpers
    {
        public static AWSECommerceServicePortTypeClient CreateAmazonClient()
        {
            //try
            //{
            const string ak2 = "3BM4NOYUORFUQ";
            const string sk2 = "daC049THzC4mjoq+6ShbLthRoO";

            BasicHttpBinding basicBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            basicBinding.MaxReceivedMessageSize = int.MaxValue;
            var amazonClient = new AWSECommerceServicePortTypeClient(basicBinding, new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

            if (amazonClient.Endpoint == null) return null;
            
   
            amazonClient.ChannelFactory.Endpoint.EndpointBehaviors.Add(new AmazonSigningEndpointBehavior((ConfigSettings.accessKeyId+ak2), (ConfigSettings.secretKey+sk2)));
            return amazonClient;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    return null;
            //}
                         
        }

        //ItemType may be All, Apparel, Appliances, ArtsAndCrafts, Automotive, Baby, Beauty, Blended, Books, Classical,
        //                  Collectibles, DigitalMusic, Grocery, DVD, Electronics, HealthPersonalCare, HomeGarden,
        //                  Industrial, Jewelry, KindleStore, Kitchen, LawnGarden, Magazines, Marketplace, Merchants,
        //                  Miscellaneous, MobileApps, MP3Downloads, Music, MusicalInstruments, MusicTracks, OfficeProducts,
        //                  OutdoorLiving, PCHardware, PetSupplies, Photo, Shoes, Software, SportingGoods, Tools, Toys,
        //                  UnboxVideo, VHS, Video, VideoGames, Watches, Wireless, WirelessAccessories
        // http://docs.aws.amazon.com/AWSECommerceService/latest/DG/USSearchIndexParamForItemsearch.html
        public static string GetSearchIndex(string itemType)
        {
            switch (itemType)
                {
                    case "Game":
                        return "Toys";
                   case "Book":
                        return "Books";
                   case "TeachingAide":
                        return "All";
                   case "Video":
                        return "Video"; //A combination of DVD and VHS
                   case "Other":
                        return "All";
                   default:
                        return "All";
                }
        }

        
    }

    
}
