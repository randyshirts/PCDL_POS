using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RocketPos.Common.Foundation;
using System.Web;
using System.Xml;
using System.Net;
//using System.Diagnostics;

namespace RocketPos.Common.Helpers
{
    class AutoCompleteTextbox : ObservableObjects
    {
    
        

//        private void QueryAmazonByISBN(string ISBN)
//        {
//            string sanitized = HttpUtility.HtmlEncode(ISBN);
//            string url = @"http://webservices.amazon.com/onca/xml? 
//                      Service=AWSECommerceService
//                      &Operation=ItemLookup
//                      &ResponseGroup=Large
//                      &SearchIndex=All
//                      &IdType=" + sanitized
//                      + "&Id=076243631X
//                      &AWSAccessKeyId=[Your_AWSAccessKeyID]
//                      &AssociateTag=[Your_AssociateTag]
//                      &Timestamp=[YYYY-MM-DDThh:mm:ssZ]
//                      &Signature=[Request_Signature]";

//            WebRequest httpWebRequest = HttpWebRequest.Create(url);
//            var webResponse = httpWebRequest.GetResponse();
//            XmlDocument xmlDoc = new XmlDocument();
//            xmlDoc.Load(webResponse.GetResponseStream());
//            var result = xmlDoc.SelectNodes("//CompleteSuggestion");
//            _QueryCollection = result;

//        }
        
    }
}
