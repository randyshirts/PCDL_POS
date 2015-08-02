using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcdWeb.Models.ItemModels
{
    public class SearchTransactionsApiOutput
    {
        public List<Transaction> Transactions { get; set; }
        public double Balance { get; set; }
        public string Message { get; set; }
    }

    public class Transaction
    {
        public string TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public double Amount { get; set; }
    }
}
