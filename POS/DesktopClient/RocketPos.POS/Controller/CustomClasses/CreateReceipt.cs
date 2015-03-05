using System.Text;

namespace POS.Controller.CustomClasses
{
    public static class CreateReceipt
    {


        private const int FIRST_COL_PAD = 18;
        private const int SECOND_COL_PAD = 0;
        private const int THIRD_COL_PAD = 9;

        public static string CreateHeader()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Play Create Discover LLC");
            sb.AppendLine("1002 Oakwood Ave NE,");
            sb.AppendLine(" Huntsville, AL 35811");
            sb.AppendLine("(256)522-5555");
            sb.AppendLine("www.PlayCreateDiscover.com");
            sb.AppendLine("================");
            
            return sb.ToString();
        }

        public static string CreateFooter(double totalNoTax, double tax, double taxTotal,
                                            double cashReceived, double change, double storeCreditPaid,
                                            double storeCreditBalance, double creditDebitPaid)
        {
            var sb = new StringBuilder();

            sb.AppendLine("================");
            
            //Total Items
            sb.Append(string.Format("Before Tax:").PadRight(FIRST_COL_PAD));
            sb.AppendLine(string.Format("{0:0.00} A", totalNoTax).PadLeft(THIRD_COL_PAD));

            //Total Tax
            sb.Append(string.Format("Sales Tax: {0:P0}", tax).PadRight(FIRST_COL_PAD));
            sb.AppendLine(string.Format("{0:0.00} A", taxTotal).PadLeft(THIRD_COL_PAD));

            //Total Due
            sb.Append(string.Format("Total:").PadRight(FIRST_COL_PAD));
            sb.AppendLine(string.Format("{0:0.00} A", totalNoTax + taxTotal).PadLeft(THIRD_COL_PAD));
            sb.AppendLine();

            //Payment
            if (cashReceived > 0)
            {
                sb.Append(string.Format("Cash Received:").PadRight(FIRST_COL_PAD));
                sb.AppendLine(string.Format("{0:0.00} A", cashReceived).PadLeft(THIRD_COL_PAD));
            }
            if (change > 0)
            {
                sb.Append(string.Format("Change:").PadRight(FIRST_COL_PAD));
                sb.AppendLine(string.Format("{0:0.00} A", change).PadLeft(THIRD_COL_PAD));
            }
            if (storeCreditPaid > 0)
            {
                sb.Append(string.Format("Store Credit Paid:").PadRight(FIRST_COL_PAD));
                sb.AppendLine(string.Format("{0:0.00} A", storeCreditPaid).PadLeft(THIRD_COL_PAD));
 
                sb.Append(string.Format("Remaining Store Credit:").PadRight(FIRST_COL_PAD));
                sb.AppendLine(string.Format("{0:0.00} A", storeCreditBalance).PadLeft(THIRD_COL_PAD));
            }
            if (creditDebitPaid > 0)
            {
                sb.Append(string.Format("Credit/Debit Charge:").PadRight(FIRST_COL_PAD));
                sb.AppendLine(string.Format("{0:0.00} A", creditDebitPaid).PadLeft(THIRD_COL_PAD));
            }
            sb.AppendLine();

            //Summary
            sb.AppendLine("Thank You For Your Support!");
            sb.AppendLine();
            sb.AppendLine("All Sales Are Final");

            return sb.ToString();
        }

    }
}
