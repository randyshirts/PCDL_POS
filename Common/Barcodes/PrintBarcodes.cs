using System;
using System.Collections.Generic;
using System.Drawing;
using Spire.Pdf;
using Spire.Pdf.Barcode;
using Spire.Pdf.Graphics;
//using RocketPos.Common.Helpers;

namespace Common.Barcodes
{
    public static class PrintBarcodes
    {
        public static PdfDocument PrintBarcodesItems(List<PdfBarcodeModel> barcodes)
        {
            var count = 0;
            //PdfFontBase f;

            //Create a pdf document.  
            var doc = new PdfDocument();
            //margin  
            var unitCvtr = new PdfUnitConvertor();
            var margin = new PdfMargins
            {
                Top = unitCvtr.ConvertUnits(0.00f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point)
            };
            margin.Bottom = margin.Top;
            margin.Left = unitCvtr.ConvertUnits(1.00f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Right = margin.Left;

            var section = doc.Sections.Add();
            section.PageSettings.Margins = margin;
            section.PageSettings.Size = PdfPageSize.A4;

            // Create one page  
            PdfPageBase page = section.Pages.Add();

            var font1 = new PdfTrueTypeFont(new Font("Arial", 12f, FontStyle.Bold), true);
            var text = new PdfTextWidget { Font = font1 };

            float y = 40;
            float x = 0;
            text.Text = "Barcodes Added " + DateTime.Now.ToString("MM/dd/yy") + ":";
            var result = text.Draw(page, x, y);
            y = result.Bounds.Bottom + 8;


            //Get the data from the DataGrid
            foreach (var bi in barcodes)
            {
                var bc = bi.Barcode;

                if (bi.IsPrintBarcode)
                {

                    if (((count%5) == 0) && ((count%10) != 0))
                    {
                        y = 60;
                        x = 270;
                    }

                    if (((count%10) == 0) && ((count != 0)))
                    {
                        // Create one page  
                        page = section.Pages.Add();
                        y = 60;
                        x = 0;
                    }


                    //Draw title
                    text.Text = (bi.Title.Length > 40) ? bi.Title.Remove(40) : bi.Title;
                    result = text.Draw(page, x, y);
                    page = result.Page;
                    y = result.Bounds.Bottom + 2;
                    //x = 0;

                    //draw date, subject, and price
                    text.Text = bi.PriceListed.ToString("c2");
                    result = text.Draw(page, x, y);
                    page = result.Page;
                    x = x + 40;

                    text.Text = bi.Subject;
                    result = text.Draw(page, x, y);
                    page = result.Page;

                    if (bi.IsDiscountable)
                    {
                        x = x + 135;
                        text.Text = bi.DateListed.ToString("MM/dd/yy");
                        result = text.Draw(page, x, y);
                        page = result.Page;                        
                        x = x - 135;
                    }
                    else
                    {
                        x = x + 135;
                        text.Text = "ND";
                        result = text.Draw(page, x, y);
                        page = result.Page;
                        x = x - 135;
                    }
                    y = result.Bounds.Bottom + 2;
                    x = x - 40;

                    //draw Code39Barcode 
                    var barcode = new PdfCode39Barcode(bc)
                    {
                        BarcodeToTextGapHeight = 1f,
                        TextDisplayLocation = TextLocation.Bottom,
                        TextColor = Color.Crimson,
                    };

                    barcode.Draw(page, new PointF(x, y));
                    y = barcode.Bounds.Bottom + 5;

                    count++;
                }

            }


            section.Pages.Add();

            return doc;

            //Save pdf file.  
            //LaunchPdfDocument(doc);

            //Launching the Pdf file.  
            //System.Diagnostics.Process.Start("Barcodes.pdf");

            ////////Examples of other Barcode formats////////////

            //PdfCodabarBarcode barcode1 = new PdfCodabarBarcode(barcode);
            //barcode1.BarcodeToTextGapHeight = 1f;
            //barcode1.EnableCheckDigit = true;
            //barcode1.ShowCheckDigit = true;
            //barcode1.TextDisplayLocation = TextLocation.Bottom;
            //barcode1.TextColor = Color.Green;
            //barcode1.Draw(page, new PointF(0, y));
            //y = barcode1.Bounds.Bottom + 5;

            //PdfCode11Barcode barcode2 = new PdfCode11Barcode("123-4567890");
            //barcode2.BarcodeToTextGapHeight = 1f;
            //barcode2.TextDisplayLocation = TextLocation.Bottom;
            //barcode2.TextColor = Color.OrangeRed;
            //barcode2.Draw(page, new PointF(0, y));
            //y = barcode2.Bounds.Bottom + 5;

            ////draw Code11Barcode  
            //text.Text = "Code11:";
            //result = text.Draw(page, 0, y);
            //page = result.Page;
            //y = result.Bounds.Bottom + 2;

            ////draw Code128-A  
            //text.Text = barcode;
            //result = text.Draw(page, 0, y);
            //page = result.Page;
            //y = result.Bounds.Bottom + 2;

            //PdfCode128ABarcode barcode3 = new PdfCode128ABarcode(barcode);
            //barcode3.BarcodeToTextGapHeight = 1f;
            //barcode3.TextDisplayLocation = TextLocation.Bottom;
            //barcode3.TextColor = System.Drawing.Color.Blue;
            //barcode3.Draw(page, new PointF(0, y));
            //y = barcode3.Bounds.Bottom + 5;


            //draw Code128-B  
            //text.Text = "Code128-B:";
            //result = text.Draw(page, 0, y);
            //page = result.Page;
            //y = result.Bounds.Bottom + 2;

            //PdfCode128BBarcode barcode4 = new PdfCode128BBarcode("Hello 00-123");
            //barcode4.BarcodeToTextGapHeight = 1f;
            //barcode4.TextDisplayLocation = TextLocation.Bottom;
            //barcode4.TextColor = System.Drawing.Color.Blue;
            //barcode4.Draw(page, new PointF(0, y));
            //y = barcode4.Bounds.Bottom + 5;


            ////draw Code32  
            //text.Text = "Code32:";
            //result = text.Draw(page, 0, y);
            //page = result.Page;
            //y = result.Bounds.Bottom + 2;

            //PdfCode32Barcode barcode5 = new PdfCode32Barcode("16273849");
            //barcode5.BarcodeToTextGapHeight = 1f;
            //barcode5.TextDisplayLocation = TextLocation.Bottom;
            //barcode5.TextColor = System.Drawing.Color.Blue;
            //barcode5.Draw(page, new PointF(0, y));
            //y = barcode5.Bounds.Bottom + 5;


            //////////////////Example of Spire.Pdf usage to set settings//////////////////////
            ////DataGridBarcodes.Clear();
            //settings = new BarcodeSettings();
            //string data = "12345";
            //string type = "Code128";

            //settings.Data2D = data;
            ////settings.Data = this.textBoxText.Text;

            //if (comboBoxType.SelectedItem != null)
            //{
            //    type = comboBoxType.SelectedItem.ToString();
            //}
            //settings.Type = (BarCodeType)Enum.Parse(typeof(BarCodeType), type);

            //if (this.checkBoxBorder.Checked)
            //{
            //    if (comboBoxText.SelectedItem != null)
            //    {
            //        settings.HasBorder = true;
            //        settings.BorderDashStyle = (DashStyle)Enum.Parse(typeof(DashStyle), comboBoxText.SelectedItem.ToString());
            //    }
            //}

            //short fontSize = 8;
            //string font = "SimSun";
            //if (this.comboBoxFont.SelectedItem != null)
            //{
            //    font = this.comboBoxFont.SelectedItem.ToString();
            //}

            //if (this.textBoxSize.Text != null && this.textBoxSize.Text.Length > 0 && Int16.TryParse(this.textBoxSize.Text, out fontSize))
            //{
            //    if (font != null && font.Length > 0)
            //    {
            //        settings.TextFont = new System.Drawing.Font(font, fontSize, FontStyle.Bold);
            //    }
            //}

            //short barHeight = 15;
            //if (this.textBoxHeight.Text != null && this.textBoxHeight.Text.Length > 0 && Int16.TryParse(this.textBoxHeight.Text, out barHeight))
            //{
            //    settings.BarHeight = barHeight;
            //}
            //if (this.checkBoxText.Checked)
            //{
            //    settings.ShowText = true;
            //}
            //else
            //{
            //    settings.ShowText = false;
            //}

            //if (this.checkBoxSum.Checked)
            //{
            //    settings.ShowCheckSumChar = true;
            //}
            //else
            //{
            //    settings.ShowCheckSumChar = false;
            //}

            //if (this.comboBoxColor.SelectedItem != null)
            //{
            //    string foreColor = this.comboBoxColor.SelectedItem.ToString();
            //    settings.ForeColor = Color.FromName(foreColor);
            //}

            ////generate the barcode use the settings
            //BarCodeGenerator generator = new BarCodeGenerator(settings);
            //Image barcode = generator.GenerateImage();

            ////save the barcode as an image
            //barcode.Save(@"..\..\..\..\Data\barcode.png");

            ////launch the generated barcode image
            //System.Diagnostics.Process.Start(@"..\..\..\..\Data\barcode.png");

        }

        public static PdfDocument SavePdfDocument(PdfDocument doc)
        {
            doc.SaveToFile("Barcodes.pdf");
            doc.Close();

            return doc;
        }

         public static void LaunchPdfDocument(PdfDocument doc)
        {
            doc.SaveToFile("Barcodes.pdf");
            doc.Close();

            //Launching the Pdf file.  
            System.Diagnostics.Process.Start("Barcodes.pdf");
        }
        
    }
}
