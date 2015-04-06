using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcdWeb.Models.ItemModels
{
    public class AddItemApiOutput
    {
        public string Barcode { get; set; }
        public string DateAdded { get; set; }
        public string Message { get; set; }
    }
}
