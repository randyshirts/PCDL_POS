// -----------------------------------------------------------------------------
//  <copyright file="CustomerViewModel.cs" company="DCOM Engineering, LLC">
//      Copyright (c) DCOM Engineering, LLC.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

using System;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using RocketPos.Common.Foundation;
using Inventory.Controller.CustomClasses;
using RocketPos.Common.Helpers;

namespace Inventory.ViewModels.AddItem.ViewModels
{
    //using Spire.Barcode;
    

    //using Google.Apis.Barcodes.v1;

    /// <summary>
    /// A View-Model that represents a customer and its state information.
    /// </summary>
    public class AddItemBarcodesVm : ViewModel
    {
        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance

        //public BarcodeSettings settings;

        public AddItemBarcodesVm()
        {
            //Initialize collections
            DataGridBarcodes = new TrulyObservableCollection<BarcodeItem>();

            //Register for messages to listen to
            Messenger.Default.Register<PropertySetter>(this, AddItemVm.Token, msg => SetDataGridBarcodes(msg.PropertyName, msg.PropertyValue));

        }

        public ActionCommand WindowLoaded
        {
            get
            {
                return new ActionCommand(p => LoadWindow());
            }
        }

        private void LoadWindow()
        {
            //Load ConsignorNames
            //using (var api = new BusinessContext())
            //{
            //    //consignorNameComboValues.InitializeComboBox(api.GetConsignorNames());
            //}
            //MessageBox.Show("Here");
        }

        /// <summary>
        /// Gets the collection of BarcodeItem entities.
        /// </summary>
        private TrulyObservableCollection<BarcodeItem> _dataGridBarcodes;
        public TrulyObservableCollection<BarcodeItem> DataGridBarcodes
        {
            get { return _dataGridBarcodes; }
            set
            {
                _dataGridBarcodes = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the selectedItem row from the datagrid.
        /// </summary>
        private BarcodeItem _selectedBarcodeItem;
        public BarcodeItem SelectedBarcodeItem
        {
            get { return _selectedBarcodeItem; }
            set
            {
                _selectedBarcodeItem = value;
                OnSelected();
            }
        }

        private void OnSelected()
        {
            //if (SelectedBarcodeItem != null)
            //Messenger.Default.Send(new PropertySetter("BarcodeImage", SelectedBarcodeItem.BarcodeImage), Token);
        }

        public void SetDataGridBarcodes(string name, object barcodes)
        {
            if (name == "DataGridBarcodes")
                DataGridBarcodes = (TrulyObservableCollection<BarcodeItem>)(barcodes);
        }

        public ActionCommand ClearBarcodesCommand
        {
            get
            {
                return new ActionCommand(p => ClearBarcodeGrid());
            }
        }

        private void ClearBarcodeGrid()
        {
            DataGridBarcodes.Clear();
        }

        public ActionCommand PrintBarcodesCommand
        {
            get
            {
                return new ActionCommand(p => PrintBarcodes.PrintBarcodesItems(DataGridBarcodes.ToList()));
            }
        }

        //private PdfFontBase f;

        //private void PrintBarcodes()
        //{
        //    int count = 0;

        //    //Create a pdf document.  
        //    PdfDocument doc = new PdfDocument();
        //    //margin  
        //    PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
        //    PdfMargins margin = new PdfMargins();
        //    margin.Top = unitCvtr.ConvertUnits(2.54f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
        //    margin.Bottom = margin.Top;
        //    margin.Left = unitCvtr.ConvertUnits(3.17f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
        //    margin.Right = margin.Left;

        //    PdfSection section = doc.Sections.Add();
        //    section.PageSettings.Margins = margin;
        //    section.PageSettings.Size = PdfPageSize.A4;

        //    // Create one page  
        //    PdfPageBase page = section.Pages.Add();

        //    PdfTrueTypeFont font1 = new PdfTrueTypeFont(new Font("Arial", 12f, System.Drawing.FontStyle.Bold), true);
        //    PdfTextWidget text = new PdfTextWidget();
        //    text.Font = font1;

        //    //draw Codabar                       
        //    float y = 40;
        //    float x = 0;
        //    text.Text = "Barcodes Added " + DateTime.Now.ToString("MM/dd/yy") + ":";
        //    PdfLayoutResult result = text.Draw(page, x, y);
        //    y = result.Bounds.Bottom + 2;


        //    //Get the data from the DataGrid
        //    foreach (BarcodeItem bi in DataGridBarcodes)
        //    {
        //        if (bi.IsPrintBarcode)
        //        {
        //            var bc = bi.BarcodeItemBc;

        //            if (((count % 5) == 0) && ((count % 10) != 0))
        //            {
        //                y = 40;
        //                x = 220;
        //            }

        //            if (((count % 10) == 0) && ((count != 0)))
        //            {
        //                // Create one page  
        //                page = section.Pages.Add();
        //                y = 40;
        //                x = 0;
        //            }


        //            //draw date and price
        //            text.Text = bi.PriceListed.ToString("c2");
        //            result = text.Draw(page, x, y);
        //            page = result.Page;
        //            x = x + 110;
        //            text.Text = bi.DateListed.ToString("MM/dd/yy");
        //            result = text.Draw(page, x, y);
        //            page = result.Page;
        //            y = result.Bounds.Bottom + 2;
        //            x = x - 110;
        //            //draw Code39Barcode  

        //            PdfCode39Barcode barcode = new PdfCode39Barcode(bc);
        //            barcode.BarcodeToTextGapHeight = 1f;
        //            barcode.TextDisplayLocation = TextLocation.Bottom;
        //            barcode.TextColor = Color.OrangeRed;
        //            //barcode.Text = bi.DateListed.ToString();
        //            barcode.Draw(page, new PointF(x, y));
        //            y = barcode.Bounds.Bottom + 5;

        //            count++;
        //            //draw Code11Barcode  
        //            //text.Text = "Code11:";
        //            //result = text.Draw(page, 0, y);
        //            //page = result.Page;
        //            //y = result.Bounds.Bottom + 2;

        //            //PdfCode11Barcode barcode2 = new PdfCode11Barcode("123-4567890");
        //            //barcode2.BarcodeToTextGapHeight = 1f;
        //            //barcode2.TextDisplayLocation = TextLocation.Bottom;
        //            //barcode2.TextColor = Color.OrangeRed;
        //            //barcode2.Draw(page, new PointF(0, y));
        //            //y = barcode2.Bounds.Bottom + 5;


        //            //draw Code128-A  
        //            //text.Text = bc;
        //            //result = text.Draw(page, 0, y);
        //            //page = result.Page;
        //            //y = result.Bounds.Bottom + 2;

        //            //PdfCode128ABarcode barcode3 = new PdfCode128ABarcode(bc);
        //            //barcode3.BarcodeToTextGapHeight = 1f;
        //            //barcode3.TextDisplayLocation = TextLocation.Bottom;
        //            //barcode3.TextColor = System.Drawing.Color.Blue;
        //            //barcode3.Draw(page, new PointF(0, y));
        //            //y = barcode3.Bounds.Bottom + 5;

        //            //draw Code128-B  
        //            //text.Text = "Code128-B:";
        //            //result = text.Draw(page, 0, y);
        //            //page = result.Page;
        //            //y = result.Bounds.Bottom + 2;

        //            //PdfCode128BBarcode barcode4 = new PdfCode128BBarcode("Hello 00-123");
        //            //barcode4.BarcodeToTextGapHeight = 1f;
        //            //barcode4.TextDisplayLocation = TextLocation.Bottom;
        //            //barcode4.TextColor = System.Drawing.Color.Blue;
        //            //barcode4.Draw(page, new PointF(0, y));
        //            //y = barcode4.Bounds.Bottom + 5;
        //        }


        //    }

        //    page = section.Pages.Add();
        //    y = 10;

        //    //Save pdf file.  
        //    doc.SaveToFile("Barcodes.pdf");
        //    doc.Close();

        //    //Launching the Pdf file.  
        //    System.Diagnostics.Process.Start("Barcodes.pdf");

        //    ////////Examples of other Barcode formats////////////

        //    //PdfCodabarBarcode barcode1 = new PdfCodabarBarcode(barcode);
        //    //barcode1.BarcodeToTextGapHeight = 1f;
        //    //barcode1.EnableCheckDigit = true;
        //    //barcode1.ShowCheckDigit = true;
        //    //barcode1.TextDisplayLocation = TextLocation.Bottom;
        //    //barcode1.TextColor = Color.Green;
        //    //barcode1.Draw(page, new PointF(0, y));
        //    //y = barcode1.Bounds.Bottom + 5;

        //    //PdfCode11Barcode barcode2 = new PdfCode11Barcode("123-4567890");
        //    //barcode2.BarcodeToTextGapHeight = 1f;
        //    //barcode2.TextDisplayLocation = TextLocation.Bottom;
        //    //barcode2.TextColor = Color.OrangeRed;
        //    //barcode2.Draw(page, new PointF(0, y));
        //    //y = barcode2.Bounds.Bottom + 5;

        //    ////draw Code11Barcode  
        //    //text.Text = "Code11:";
        //    //result = text.Draw(page, 0, y);
        //    //page = result.Page;
        //    //y = result.Bounds.Bottom + 2;

        //    ////draw Code128-A  
        //    //text.Text = barcode;
        //    //result = text.Draw(page, 0, y);
        //    //page = result.Page;
        //    //y = result.Bounds.Bottom + 2;

        //    //PdfCode128ABarcode barcode3 = new PdfCode128ABarcode(barcode);
        //    //barcode3.BarcodeToTextGapHeight = 1f;
        //    //barcode3.TextDisplayLocation = TextLocation.Bottom;
        //    //barcode3.TextColor = System.Drawing.Color.Blue;
        //    //barcode3.Draw(page, new PointF(0, y));
        //    //y = barcode3.Bounds.Bottom + 5;


        //    //draw Code128-B  
        //    //text.Text = "Code128-B:";
        //    //result = text.Draw(page, 0, y);
        //    //page = result.Page;
        //    //y = result.Bounds.Bottom + 2;

        //    //PdfCode128BBarcode barcode4 = new PdfCode128BBarcode("Hello 00-123");
        //    //barcode4.BarcodeToTextGapHeight = 1f;
        //    //barcode4.TextDisplayLocation = TextLocation.Bottom;
        //    //barcode4.TextColor = System.Drawing.Color.Blue;
        //    //barcode4.Draw(page, new PointF(0, y));
        //    //y = barcode4.Bounds.Bottom + 5;


        //    ////draw Code32  
        //    //text.Text = "Code32:";
        //    //result = text.Draw(page, 0, y);
        //    //page = result.Page;
        //    //y = result.Bounds.Bottom + 2;

        //    //PdfCode32Barcode barcode5 = new PdfCode32Barcode("16273849");
        //    //barcode5.BarcodeToTextGapHeight = 1f;
        //    //barcode5.TextDisplayLocation = TextLocation.Bottom;
        //    //barcode5.TextColor = System.Drawing.Color.Blue;
        //    //barcode5.Draw(page, new PointF(0, y));
        //    //y = barcode5.Bounds.Bottom + 5;


        //    //////////////////Example of Spire.Pdf usage to set settings//////////////////////
        //    ////DataGridBarcodes.Clear();
        //    //settings = new BarcodeSettings();
        //    //string data = "12345";
        //    //string type = "Code128";

        //    //settings.Data2D = data;
        //    ////settings.Data = this.textBoxText.Text;

        //    //if (comboBoxType.SelectedItem != null)
        //    //{
        //    //    type = comboBoxType.SelectedItem.ToString();
        //    //}
        //    //settings.Type = (BarCodeType)Enum.Parse(typeof(BarCodeType), type);

        //    //if (this.checkBoxBorder.Checked)
        //    //{
        //    //    if (comboBoxText.SelectedItem != null)
        //    //    {
        //    //        settings.HasBorder = true;
        //    //        settings.BorderDashStyle = (DashStyle)Enum.Parse(typeof(DashStyle), comboBoxText.SelectedItem.ToString());
        //    //    }
        //    //}

        //    //short fontSize = 8;
        //    //string font = "SimSun";
        //    //if (this.comboBoxFont.SelectedItem != null)
        //    //{
        //    //    font = this.comboBoxFont.SelectedItem.ToString();
        //    //}

        //    //if (this.textBoxSize.Text != null && this.textBoxSize.Text.Length > 0 && Int16.TryParse(this.textBoxSize.Text, out fontSize))
        //    //{
        //    //    if (font != null && font.Length > 0)
        //    //    {
        //    //        settings.TextFont = new System.Drawing.Font(font, fontSize, FontStyle.Bold);
        //    //    }
        //    //}

        //    //short barHeight = 15;
        //    //if (this.textBoxHeight.Text != null && this.textBoxHeight.Text.Length > 0 && Int16.TryParse(this.textBoxHeight.Text, out barHeight))
        //    //{
        //    //    settings.BarHeight = barHeight;
        //    //}
        //    //if (this.checkBoxText.Checked)
        //    //{
        //    //    settings.ShowText = true;
        //    //}
        //    //else
        //    //{
        //    //    settings.ShowText = false;
        //    //}

        //    //if (this.checkBoxSum.Checked)
        //    //{
        //    //    settings.ShowCheckSumChar = true;
        //    //}
        //    //else
        //    //{
        //    //    settings.ShowCheckSumChar = false;
        //    //}

        //    //if (this.comboBoxColor.SelectedItem != null)
        //    //{
        //    //    string foreColor = this.comboBoxColor.SelectedItem.ToString();
        //    //    settings.ForeColor = Color.FromName(foreColor);
        //    //}

        //    ////generate the barcode use the settings
        //    //BarCodeGenerator generator = new BarCodeGenerator(settings);
        //    //Image barcode = generator.GenerateImage();

        //    ////save the barcode as an image
        //    //barcode.Save(@"..\..\..\..\Data\barcode.png");

        //    ////launch the generated barcode image
        //    //System.Diagnostics.Process.Start(@"..\..\..\..\Data\barcode.png");

        //}

        ///// <summary>
        ///// Gets the command that allows a cell to be updated.
        ///// </summary>
        //public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand
        //{
        //    get
        //    {
        //        return new RelayCommand<DataGridCellEditEndingEventArgs>(UpdateBarcodeItem);
        //    }
        //}

        ///// <summary>
        ///// Gets the command that allows a row to be deleted.
        ///// </summary>
        //public RelayCommand DeleteSelectedCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(DeleteBarcodeItem);
        //    }
        //}

        //private void DeleteBarcodeItem()
        //{

        //    if (MessageBox.Show("Deletions are permanent. Are you sure you want to delete this item? ", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
        //        return;
        //    else
        //    {
        //        if (null != SelectedBarcodeItem)
        //        {
        //            var barcodeItem = DataGridBarcodes.FirstOrDefault(bi => bi.Id == SelectedBarcodeItem.Id);
        //            if (barcodeItem != null)
        //            {
        //                var tempBarcodes = DataGridBarcodes;
        //                var gridIndex = tempBarcodes.IndexOf(barcodeItem);
        //                if ((gridIndex + 1) == tempBarcodes.Count)
        //                    gridIndex--;
        //                tempBarcodes.Remove(barcodeItem);
        //                DataGridBarcodes = tempBarcodes;
        //                SelectedBarcodeItem = DataGridBarcodes.ElementAtOrDefault(gridIndex);

        //                using (var bc = new BusinessContext())
        //                {
        //                    bc.DeleteItemById(barcodeItem.Id);
        //                }
        //            }
        //        }
        //    }


        //}


        //private void UpdateBarcodeItem(DataGridCellEditEndingEventArgs e)
        //{

        //    //using (var bc = new BusinessContext())
        //    //{
        //    //    string text = null;

        //    //    text = WpfHelpers.GetTextFromCellEditingEventArgs(e);

        //    //    //Get the Column header
        //    //    string column = e.Column.Header.ToString();

        //    //    //Add info to new BarcodeItem
        //    //    var barcodeItem = e.Row.Item as BarcodeItem;
        //    //    var success = barcodeItem.SetProperty(column, text);

        //    //    if (IsValid(barcodeItem) && success)
        //    //    {

        //    //        switch (column)
        //    //        {
        //    //            case "Title":
        //    //            case "Author":
        //    //            case "Binding":
        //    //            case "# Of Pages":
        //    //            case "PublicationDate":
        //    //            case "TradeInValue":
        //    //            case "ISBN":
        //    //                {
        //    //                    var barcode = barcodeItem.ConvertBarcodeItemToBarcode();
        //    //                    bc.UpdateBarcode(barcode);
        //    //                    break;
        //    //                }
        //    //            case "Consignor Name":
        //    //            case "Status":
        //    //            case "Subject":
        //    //            case "ListedDate":
        //    //            case "ListedPrice":
        //    //            case "Condition":
        //    //            case "Description":
        //    //                {
        //    //                    var item = barcodeItem.ConvertBarcodeItemToItem();
        //    //                    bc.UpdateItem(item);
        //    //                    break;
        //    //                }
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        e.Cancel = true;

        //    //        MessageBoxResult result = MessageBox.Show("Input Error - '" + text + "' - Try again", "Error");
        //    //    }
        //    //}
        //}

    }
}