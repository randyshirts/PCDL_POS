using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcdWeb.Models.ItemModels
{
    public class SearchItemsApiOutput
    {
        public List<SearchItem> Items { get; set; }
        public string Message { get; set; }
    }

    public class SearchItem
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public bool Discount { get; set; }
        public string DateAdded { get; set; }
        public double OriginalPrice { get; set; }
        public double CurrentPrice { get; set; }
        public double SoldPrice { get; set; }
        public double PaymentAmount { get; set; }
        public string Barcode { get; set; }
        public string Subject { get; set; }
    }
}
