using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Barcodes
{
    public class PdfBarcodeModel
    {
        public string Barcode { get; set; }
        public string Title { get; set; }
        public bool IsPrintBarcode { get; set; }
        public double PriceListed { get; set; }
        public string Subject { get; set; }
        public bool IsDiscountable { get; set; }
        public DateTime DateListed { get; set; }
    }
}
