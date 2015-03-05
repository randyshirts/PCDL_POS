using System.Text;
using POS.Controller.Elements;
using POS.Controller.Interfaces;

namespace POS.Controller.Visitors
{
    public class CreateReceiptVisitor : IReceiptVisitor
    {
        private StringBuilder _sb = new StringBuilder();

        private const int FIRST_COL_PAD = 18;
        private const int SECOND_COL_PAD = 0;
        private const int THIRD_COL_PAD = 9;

        public string Visit(SaleItem item)
        {
            var sb = new StringBuilder();

            sb.Append(item.Title.Length > FIRST_COL_PAD
                ? item.Title.Remove(FIRST_COL_PAD).PadRight(FIRST_COL_PAD)
                : item.Title.PadRight(FIRST_COL_PAD));

            sb.AppendLine(string.Format("{0:0.00} A", item.UnitPrice).PadLeft(THIRD_COL_PAD));


            if (item.DateDiscount > 0)
            {
                sb.Append(string.Format("DATE DISC {0:P0}", item.DateDiscount).PadRight(FIRST_COL_PAD + SECOND_COL_PAD));
                sb.AppendLine(string.Format("{0:0.00} A", -(item.UnitPrice*item.DateDiscount)).PadLeft(THIRD_COL_PAD));
                sb.AppendLine();
            }

            if (item.AddlDiscount > 0)
            {
                sb.Append(string.Format("ADDL DISC {0:P0}", item.AddlDiscount).PadRight(FIRST_COL_PAD + SECOND_COL_PAD));
                sb.AppendLine(string.Format("{0:0.00} A", -(item.UnitPrice*item.AddlDiscount)).PadLeft(THIRD_COL_PAD));
                sb.AppendLine();
            }

            return sb.ToString();

        }
    }
}



