using System;
using System.IO;
using System.Net.Mail;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;

namespace RocketPos.Common.Helpers
{


    public static class ReceiptPrinterHelper
    {
        //private static PrintQueue PrintQueue { get; set; }

        public static void PrintText(string text)
        {
            //Run print in different thread or else printer error causes UI to lock up
            Task task = Task.Run(PrintTask(text));

            var complete = task.Wait(2000); //Wait two seconds then cancel
            if (!complete)
            {
                try
                {
                    MessageBox.Show("Print Error - \r\n" +
                                    "1) Make Sure the Printer is turned on \r\n" +
                                    "2) Make sure the usb cord is plugged in\r\n" +
                                    "3) Try clearing the printer queue and resetting the printer");
                    task.Dispose();
                    
                }
                catch (Exception ex)
                {

                }
            }

        }

        public static PrintQueue FindPrinter(string printerName)
        {
            var printers = new PrintServer().GetPrintQueues();
            foreach (var printer in printers)
            {
                if (printer.FullName == printerName)
                {
                    return printer;
                    
                }
            }
            //return LocalPrintServer.GetDefaultPrintQueue();
            return LocalPrintServer.GetDefaultPrintQueue();
        }

        private static Action PrintTask(string text)
        {
            return new Action(() => PrintProcess.Print(text));

        }

    }

    static class PrintProcess
    {
        public static void Print(string text)
        {
            var printDlg = new PrintDialog();

            var doc = new FlowDocument(new Paragraph(new Run(text)))
            {
                PagePadding = new Thickness(2),
                FontSize = 10,
                FontFamily = new System.Windows.Media.FontFamily("Consolas")
                //Must be monospaced font for column spacing                  
            };

            var height = doc.PageHeight;

            printDlg.PrintQueue = ReceiptPrinterHelper.FindPrinter("POS-58");

            printDlg.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Print Receipt");


            //Save as xps file           

            //Check to see if directory exists
            var mDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Receipts");
            if(!Directory.Exists(mDirPath))
                Directory.CreateDirectory(mDirPath);

            //Create file name with year first
            var dateNoSlashes = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            var mFilePath = mDirPath + "\\" + dateNoSlashes + "Receipts.txt";

            if (!File.Exists(mFilePath))
            {
                using (var fileStream = new FileStream(mFilePath, FileMode.OpenOrCreate))
                {
                    var inputStream = WpfHelpers.GenerateStreamFromString(text);
                    inputStream.Seek(0, SeekOrigin.Begin);
                    //fileStream.Seek(0, SeekOrigin.End);
                    inputStream.CopyTo(fileStream);
                }
            }
            else
            {
                using (var fileStream = new FileStream(mFilePath, FileMode.Append))
                {
                    //Put space between receipts
                    var outputText = text.Insert(0, "\r\n\r\n\r\n");

                    var inputStream = WpfHelpers.GenerateStreamFromString(outputText);
                    inputStream.Seek(0, SeekOrigin.Begin);
                    //fileStream.Seek(0, SeekOrigin.End);
                    inputStream.WriteTo(fileStream);
                }
            }
        }
    }
}
