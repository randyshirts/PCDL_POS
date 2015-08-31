using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Barcodes;

namespace PcdWeb.Models.ItemModels
{
    public class PrintBarcodesModel
    {
        public string Barcode { get; set; }
        public string Title { get; set; }
        public bool Print { get; set; }
        public double OriginalPrice { get; set; }
        public string Subject { get; set; }
        public bool Discounted { get; set; }
        public string DateAdded { get; set; }


        public List<PdfBarcodeModel> ConvertToPdfBarcodeModelList(IEnumerable<PrintBarcodesModel> models)
        {
            var pdfList = models.Select(model => new PdfBarcodeModel()
            {
                Barcode = model.Barcode,
                DateListed = DateTime.Parse(model.DateAdded),
                IsDiscountable = model.Discounted,
                IsPrintBarcode = model.Print,
                PriceListed = model.OriginalPrice,
                Subject = model.Subject,
                Title = model.Title
            }).ToList();

            return pdfList;
        }
    }
}
