using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Barcodes;

namespace RocketPos.Common.Helpers
{
    public static class PdfBarcodeHelpers
    {
        public static List<PdfBarcodeModel> ConvertBarcodeItemsToPdfModels(IEnumerable<BarcodeItem> barcodes)
        {
            var pdfList = barcodes.Select(barcode => new PdfBarcodeModel()
            {
                Barcode = barcode.BarcodeItemBc,
                DateListed = barcode.DateListed,
                IsDiscountable = barcode.IsDiscountable,
                IsPrintBarcode = barcode.IsPrintBarcode,
                PriceListed = barcode.PriceListed,
                Subject = barcode.Subject,
                Title = barcode.Title
            }).ToList();

            return pdfList;
        }
    }
}
